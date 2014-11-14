using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	public GameManager m_GameManager;

	//the player basic info
	public int playerHP = 1;
	private int currentHP = 1;
	public int playerArmor = 0;
	public float speed = 5.0f;
	public float horLimit = 0.0f;
	public float vertLimit = 0.0f;
	private Vector2 velocity = Vector2.zero;
	private Animator anim;
	private bool isDead = false;
	private Vector3 startingPosition;

	//the explosion to use when a bullet hits
	public GameObject pinkExplosionPrefab;

	//the cannons!
	public GameObject cannonPoint;
	public CannonController[] cannons;
	public CannonController[] instanciatedCannons;
	public CannonController currentCannon;
	public int cannonID;
	public float cannonSelectionDelay = 0.2f;
	private float cannonSelectionTimer = 0.0f;
	private int cannonSelectionDirection = 0;

	//the shields used for absorbtion effect
	public GameObject m_Shield;

	//hit comparerererer
	public string target;

	void Start(){
		startingPosition = transform.position;
		anim = GetComponent<Animator>();
		m_GameManager = GameObject.Find ("GameManagerObj").GetComponent<GameManager> ();

		//instantiating the first cannon
		instanciatedCannons = new CannonController[cannons.Length];
		for (int i = 0; i < cannons.Length; i++) {
			instanciatedCannons[i] = Instantiate (cannons[i], cannonPoint.transform.position, cannonPoint.transform.rotation) as CannonController;
			instanciatedCannons[i].transform.parent = transform;
			if(i == cannonID) {
				currentCannon = instanciatedCannons[i];
				currentCannon.m_IsAvailable = true;
			}else {
				instanciatedCannons[i].gameObject.SetActive(false);
				instanciatedCannons[i].m_IsAvailable = false;
			}
		}
		currentHP = playerHP;
	}
	
	void Update () {
		if(m_GameManager.m_CurrentState == GameManager.gameState.playing && !isDead){

			//cannon selection
			cannonSelectionDirection = -1;
			if(Input.GetKeyDown(KeyCode.C) || Input.GetKey(m_GameManager.m_CannonSelectionButtons[0])){
				if(Time.time >= cannonSelectionTimer){
					cannonSelectionDirection = 0;
					cannonSelectionTimer = Time.time + cannonSelectionDelay;
				}
			}else if(Input.GetKeyDown(KeyCode.V) || Input.GetKey(m_GameManager.m_CannonSelectionButtons[1])){
				if(Time.time >= cannonSelectionTimer){
					cannonSelectionDirection = 1;
					cannonSelectionTimer = Time.time + cannonSelectionDelay;
				}
			}
			if(cannonSelectionDirection != -1) ChangeSelectedCannon(cannonSelectionDirection);

			//move
			velocity.x = m_GameManager.m_HorValue * speed;
			velocity.y = m_GameManager.m_VertValue * speed;

			//changing the ship from side to side and idle
			if (m_GameManager.m_HorValue > 0.0f) {
				anim.SetInteger("direction",1);
			} else if (m_GameManager.m_HorValue < 0.0f) {
				anim.SetInteger("direction", -1);
			}else{
				anim.SetInteger("direction", 0);
			}

			//move the ship, cannon and shields
			velocity = Vector2.ClampMagnitude(velocity, speed * Time.deltaTime);
			transform.Translate (velocity, Space.World);

			//clamping to screen size
			float clampedLimitX = Mathf.Clamp(transform.position.x, -horLimit, horLimit);
			float clampedLimitY = Mathf.Clamp(transform.position.y, -vertLimit, vertLimit);			
			transform.position = new Vector3 (clampedLimitX, clampedLimitY, 0.0f);
		}
	}

	//checking health to change amount of shields and changing amount of lives if needed
	public void CheckHealth(){
		if(currentHP <= 0){
			currentHP = playerHP;
			StartCoroutine(m_GameManager.DeathExplosion(currentHP));
		}else{
			m_Shield.SetActive (false);
		}
	}

	public void RepositionShip(){
		transform.position = startingPosition;
		currentCannon.transform.position = cannonPoint.transform.position;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		
		ProjectileController tempBullet = coll.gameObject.GetComponent<ProjectileController>();
		if (tempBullet!= null && tempBullet.m_Owner == target) {
			Instantiate (pinkExplosionPrefab, tempBullet.transform.position, tempBullet.transform.rotation);
			currentHP = m_GameManager.Hit(tempBullet.m_DamageValue, currentHP, playerArmor);
			CheckHealth();
			tempBullet.pushBullet(tempBullet);
		}
	}

	private void ChangeSelectedCannon(int direction){
		int previousID = cannonID;
		bool foundCannon = false;
		if (direction == 0 && cannonID > 0) {
			while(!foundCannon && cannonID > 0){
				cannonID--;
				if(instanciatedCannons[cannonID].m_IsAvailable) foundCannon = true;
			}
			if(!foundCannon) cannonID = previousID;
		}else if(direction == 1 && cannonID < instanciatedCannons.Length-1){
			while(!foundCannon && cannonID < (instanciatedCannons.Length-1)){
				cannonID++;
				if(instanciatedCannons[cannonID].m_IsAvailable) foundCannon = true;
			}
			if(!foundCannon) cannonID = previousID;
		}
		if (cannonID != previousID) {
			currentCannon.gameObject.SetActive(false);
			currentCannon = instanciatedCannons[cannonID];
			currentCannon.gameObject.SetActive(true);
		}
	}

	public void ActivateCannon(int newCannonID){
		instanciatedCannons [newCannonID].m_IsAvailable = true;
		cannonID = newCannonID;
		currentCannon.gameObject.SetActive(false);
		currentCannon = instanciatedCannons[cannonID];
		currentCannon.gameObject.SetActive(true);
	}
}
