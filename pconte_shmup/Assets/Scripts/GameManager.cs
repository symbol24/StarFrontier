using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	//the player ship
	public PlayerController m_PlayerShip;

	//the player and enemy bullets
	public int m_BulletAmount;
	public ProjectileController[] m_ProjectilePrefabs;
	public Stack<ProjectileController>[] m_ProjectileStacks;

	//game state enum
	public enum gameState{
		playing, paused, gameover,dead
	}
	public gameState m_CurrentState;
	
	//life icons top left of screen
	public int m_NumberOfLives;
	public GameObject m_LifeIconPrefab;
	public GameObject[] m_LifeIconsDisplayed;

	//explosions when dead
	public GameObject longPinkExplosionPrefab;
	public int numberOfExplosions;
	public float explosionOffset;
	public float explosionAnimationTime;
	public float deathControlDelay;

	//score info
	public GUIText m_ScoreGUI;
	public int m_TotalScore = 0;
	private int m_TotalKills = 0;
	public int m_TargetScore;

	//controls
	public KeyCode m_PauseButton;
	public KeyCode m_ConfirmButton;
	public KeyCode m_ShootButton;
	public float m_VertValue;
	public float m_HorValue;
	public float m_MenuDelayTimer = 0.0f;
	public float m_MenuDeadSpot = 0.1f;

	//end game messages
	public string m_VictoryMessage;
	public string m_LoseMessage;

	//for firing status and shield management
	public bool m_isShooting = false;

	
	void Start(){
		//creating bullets into stacks
		m_ProjectileStacks = new Stack<ProjectileController>[m_ProjectilePrefabs.Length];
		for(int i = 0; i < m_ProjectilePrefabs.Length; i++){
			m_ProjectileStacks[i] = EntitiesCreator.CreatAStackOfBullets (m_ProjectilePrefabs[i], m_BulletAmount);
		}

		//creating the life icons at top of screen
		m_LifeIconsDisplayed = new GameObject[m_NumberOfLives];
		for(int i = 0; i < m_NumberOfLives; i++){
			GameObject tempLifeIcon = Instantiate(m_LifeIconPrefab, new Vector2(m_LifeIconPrefab.transform.position.x - i, m_LifeIconPrefab.transform.position.y), m_LifeIconPrefab.transform.rotation) as GameObject;
			m_LifeIconsDisplayed[i] = tempLifeIcon;
		}

		//setting the game state to playing
		m_CurrentState = gameState.playing;

		//for some reason at work, i had to force the time scale to have the game play once the start is passed
		Time.timeScale = 1.0f;
	}

	
	// Update is called once per frame
	void Update () {
		
		//get both controller and keyboard axis's
		m_VertValue = Input.GetAxis("Vertical");
		m_HorValue = Input.GetAxis("Horizontal");
	}

	//reduce the amount of lives, remove a visible life icon and trigger endgame
	public void DecreaseLives(){
		m_NumberOfLives -= 1;
		m_LifeIconsDisplayed[m_NumberOfLives].SetActive(false);
		if(m_NumberOfLives <= 0){
			SetGameOver(m_LoseMessage);
		}
	}

	//hit and mitigate damage together yay!
	public int Hit(int damage, int hp, int armor) {
		if(damage > armor){
			damage -= armor;
		}else{
			damage = 0;
		}
		return hp - damage;
	}
	
	public void UpdateScore(int score){
		m_TotalKills++;
		m_TotalScore += score;
		m_ScoreGUI.text = m_TotalScore.ToString ();
		if(m_TotalScore >= m_TargetScore){
			SetGameOver (m_VictoryMessage);
		}
	}

	//endgame process
	public void SetGameOver(string message){
		m_CurrentState = gameState.gameover;
		EndGameMenu egm = GetComponent<EndGameMenu> ();
		if (egm != null) {
			egm.DisplayGameOverScreen(message);
		}
	}

	public void SwitchShieldStatus(bool isShooting){
		if(isShooting){
			m_isShooting = isShooting;
			m_PlayerShip.m_Shield.SetActive(false);
			BoxCollider2D playerShipCollider = m_PlayerShip.GetComponent<BoxCollider2D>();
			if(playerShipCollider != null){
				playerShipCollider.enabled = true;
			}
		}else{
			m_isShooting = isShooting;
			m_PlayerShip.m_Shield.SetActive(true);
			BoxCollider2D playerShipCollider = m_PlayerShip.GetComponent<BoxCollider2D>();
			if(playerShipCollider != null){
				playerShipCollider.enabled = false;
			}
		}
	}

	public IEnumerator DeathExplosion(int currentHP){
		m_CurrentState = gameState.dead;
		Transform transformForExplosion = m_PlayerShip.transform;
		m_PlayerShip.renderer.enabled = false;
		m_PlayerShip.inUseCannon.SetActive (false);
		GameObject explosion;
		for(int i = 0; i < numberOfExplosions; i++){
			float xOffset = (Random.Range(-explosionOffset, explosionOffset));
			Vector3 newPos = transformForExplosion.transform.position + new Vector3(xOffset, -(explosionOffset * i), 0);			               
			explosion = Instantiate (longPinkExplosionPrefab, newPos, transformForExplosion.transform.rotation) as GameObject;
			Destroy(explosion, explosionAnimationTime);
			yield return new WaitForSeconds(explosionAnimationTime/2);
		}
		yield return new WaitForSeconds(deathControlDelay);
		DecreaseLives();
		if(m_NumberOfLives > 0){
			m_PlayerShip.RepositionShip();
			m_PlayerShip.renderer.enabled = true;
			m_PlayerShip.inUseCannon.SetActive (true);
			m_CurrentState = gameState.playing;
		}
	}
}

