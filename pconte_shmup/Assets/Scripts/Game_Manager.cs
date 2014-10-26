using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game_Manager : MonoBehaviour {
	//the player ship
	public main_ship playerShip;

	//the enemy ships
	public enemy_controller[] eai;
	private int eaiSelector = 0;
	private int eaiCounter = 0;
	public int[] eaiSelRange;
	public float limiterY;
	public float limiterX;
	public float minSpawnRate;
	public float maxSpawnRate;
	private float nextSpawn = 0.0F;

	//the player and enemy bullets
	public int bulletAmount;
	public bullet_controller bulletPrefab;
	public bullet_controller bulletPrefabEAI;
	public Stack<bullet_controller> bullets = new Stack<bullet_controller>();
	public Stack<bullet_controller> bulletsEAI = new Stack<bullet_controller>();
	private bullet_controller tempBullet;

	//game state enum
	public enum gameState{
		playing, paused, gameover,dead
	}
	public gameState currentState;
	
	//life icons top left of screen
	public int numbLives;
	public GameObject lifeT;
	public GameObject[] lifeIcons;
	private GameObject tempLifeIcon;

	//explosions when dead
	public GameObject longPinkExplosionPrefab;
	public int numberOfExplosions;
	public float explosionOffset;
	public float explosionAnimationTime;
	public float deathControlDelay;

	//score info
	public GUIText scoreText;
	private int totalScore = 0;
	private int totalKills = 0;
	public int targetScore;

	//menu info and controls
	public KeyCode pauseButton;
	public GameObject pauseScreen;
	public GameObject pauseSelector;
	public GameObject endGameScreen;
	public GameObject endGameSelector;
	public GUIText endGameMessage;
	public GUIText endGameScore;
	public string victoryMessage;
	public string loseMessage;
	private float vertValue;
	public Vector3[] pauseMenuLocations;
	public Vector3[] endGameMenuLocations;
	private int menuPosID = 0;
	public float deadSpot;
	public float pauseDelayTimer;
	private float pauseTimer = 0.0f;
	public KeyCode confirm;

	
	void Start(){
		//creating bullets into stacks
		for(int i = 0; i < bulletAmount; i++){
			tempBullet = Instantiate(bulletPrefab, new Vector2(transform.position.x, transform.position.y), transform.rotation) as bullet_controller;
			tempBullet.gameObject.SetActive(false);
			bullets.Push(tempBullet);
		}
		
		for(int i = 0; i < bulletAmount; i++){
			tempBullet = Instantiate(bulletPrefabEAI, new Vector2(transform.position.x, transform.position.y), transform.rotation) as bullet_controller;
			tempBullet.gameObject.SetActive(false);
			bulletsEAI.Push(tempBullet);
		}

		//creating the life icons at top of screen
		for(int i = 0; i < numbLives; i++){
			tempLifeIcon = Instantiate(lifeT, new Vector2(lifeT.transform.position.x - i, lifeT.transform.position.y), lifeT.transform.rotation) as GameObject;
			lifeIcons[i] = tempLifeIcon;
		}

		//setting the game state to playing
		currentState = gameState.playing;

		//for some reason at work, i had to force the time scale to have the game play once the start is passed
		Time.timeScale = 1.0f;
	}

	
	// Update is called once per frame
	void Update () {
		if(currentState == gameState.playing){
			//spawn enemies
			if (Time.time > nextSpawn){
				nextSpawn = Time.time + Random.Range(minSpawnRate, maxSpawnRate);
				float tempX = Random.Range(-limiterX, limiterX);
				enemy_controller eaiClone;
				eaiClone = Instantiate(eai[eaiSelector], new Vector2(tempX, transform.position.y + limiterY), transform.rotation) as enemy_controller;
				eaiCounter++;

				//addind a tank into the mix from time to time
				if(eaiSelector != 1 && eaiCounter >= Random.Range(eaiSelRange[0], eaiSelRange[1])){
					eaiSelector = 1;
				}else{
					eaiSelector = 0;
				}
			}

			//open and close pause menu
			if(Input.GetKeyDown("p") || Input.GetKeyDown(pauseButton)){
				PauseGame();
			}
			if(totalScore >= targetScore){
				EndGame (victoryMessage);
			}
		}else if(currentState == gameState.paused){
			//pause menu controls
			vertValue = Input.GetAxis("Vertical");
			if(pauseTimer < Time.time && (vertValue > deadSpot || vertValue < -deadSpot)){
				pauseTimer = Time.time + pauseDelayTimer;//to add a delay in input to prevent inputs taht are too quick
				MovePauseSelector(vertValue);
			}
			if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(confirm)){
				ConfirmPauseSelect();
			}

			//open and close pause menu
			if(Input.GetKeyDown("p") || Input.GetKeyDown(pauseButton)){
				PauseGame();
			}
		}else if(currentState == gameState.gameover){
			//gameover menu controls
			vertValue = Input.GetAxis("Vertical");
			if(pauseTimer < Time.time && (vertValue > deadSpot || vertValue < -deadSpot)){
				pauseTimer = Time.time + pauseDelayTimer;//to add a delay in input to prevent inputs taht are too quick
				MoveEndGameSelector(vertValue);
			}
			if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(confirm)){
				ConfirmEndGameSelect();
			}
		}


	}

	//setting the gamestate to paused or playing
	public void PauseGame(){
		switch(currentState){
		case gameState.playing:
			currentState = gameState.paused;
			pauseScreen.SetActive(true);
			break;
		case gameState.paused:
			currentState = gameState.playing;
			pauseScreen.SetActive(false);
			break;
		default:
			currentState = gameState.paused;
			break;
		}
	}

	//pause menu controller
	private void MovePauseSelector (float vertValue){
		Vector3 newPos = pauseSelector.transform.localPosition;
		if(menuPosID == 0 && vertValue < -deadSpot){
			newPos = pauseMenuLocations[1];
			menuPosID = 1;
		}else if(menuPosID == 1 && vertValue < -deadSpot){
			newPos= pauseMenuLocations[2];
			menuPosID = 2;
		}else if(menuPosID == 1 && vertValue > deadSpot){
			newPos = pauseMenuLocations[0];
			menuPosID = 0;
		}else if(menuPosID == 2 && vertValue > deadSpot){
			newPos = pauseMenuLocations[1];
			menuPosID = 1;
		}
		pauseSelector.transform.localPosition = newPos;
	}

	private void ConfirmPauseSelect (){
		if(menuPosID == 0){
			PauseGame();
		}else if(menuPosID == 1){
			Application.LoadLevel("MainGame");
		}else{
			Application.LoadLevel("MainMenu");
		}
	}

	//reduce the amount of lives, remove a visible life icon and trigger endgame
	public void DecreaseLives(){
		numbLives -= 1;
		lifeIcons[numbLives].SetActive(false);
		if(numbLives <= 0){
			EndGame(loseMessage);
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
		totalScore += score;
		scoreText.text = totalScore.ToString ();
	}

	//endgame process
	public void EndGame(string message){
		currentState = gameState.gameover;
		endGameMessage.text = message;
		endGameScore.text = totalScore.ToString ();
		endGameScreen.SetActive(true);
	}

	//gameover menu controller
	private void MoveEndGameSelector(float vertValue){
		Vector3 newPos = endGameSelector.transform.localPosition;
		if(menuPosID == 0 && vertValue < -deadSpot){
			newPos = endGameMenuLocations[1];
			menuPosID = 1;
		}else if(menuPosID == 1 && vertValue > deadSpot){
			newPos = endGameMenuLocations[0];
			menuPosID = 0;
		}
		endGameSelector.transform.localPosition = newPos;
	}

	private void ConfirmEndGameSelect (){
		if(menuPosID == 0){
			Application.LoadLevel("MainGame");
		}else{
			Application.LoadLevel("MainMenu");
		}
	}

	public IEnumerator DeathExplosion(int currentHP, GameObject[] shields){
		currentState = gameState.dead;
		Transform transformForExplosion = playerShip.transform;
		playerShip.renderer.enabled = false;
		playerShip.inUseCannon.SetActive (false);
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
		if(numbLives > 0){
			playerShip.RepositionShip();
			playerShip.renderer.enabled = true;
			playerShip.inUseCannon.SetActive (true);
			for(int y = 0; y < currentHP; y++){
				if(shields[y] != null){
					shields[y].transform.position = playerShip.transform.position;
					shields[y].SetActive(true);
				}
			}
			currentState = gameState.playing;
		}
	}
}

