using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class main_ship : MonoBehaviour {
	private Game_Manager gameMgr;

	//the player basic info
	public int playerHP;
	private int currentHP;
	public int playerArmor;
	public float speed;
	public float horLimit;
	public float vertLimit;
	private Vector2 velocity = Vector2.zero;
	private Animator anim;
	private bool isDead = false;
	public Vector3 startingPosition;

	//control variables
	private float vertValue;
	private float horValue;
	public float deadSpot;

	//the explosion to use when a bullet hits
	public GameObject pinkExplosionPrefab;

	//the cannons!
	public GameObject cannonPoint;
	public GameObject[] cannons;
	public GameObject inUseCannon;
	public int cannonID;

	//the shields used to display player's health
	public GameObject prefabShield;
	public GameObject[] shields;
	public float shieldScaler;

	//hit comparerererer
	public string target;

	void Start(){
		startingPosition = transform.position;
		anim = GetComponent<Animator>();
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
		//instantiating the first cannon
		inUseCannon = Instantiate (cannons[cannonID], cannonPoint.transform.position, cannonPoint.transform.rotation) as GameObject;
		inUseCannon.transform.parent = transform;
		currentHP = playerHP;
		//instantiate the shilds for HP
		for (int i = 0; i < currentHP; i++) {
			shields[i] = Instantiate(prefabShield, transform.position, transform.rotation) as GameObject;
			float newScale = shieldScaler * i;
			shields[i].transform.localScale += new Vector3(newScale, newScale, 0);
			shields[i].transform.parent = transform;
		}
	}
	
	void Update () {
		if(gameMgr.currentState == Game_Manager.gameState.playing && !isDead){
			velocity = Vector2.zero;
			//get both controller and keyboard axis's
			vertValue = Input.GetAxis("Vertical");
			horValue = Input.GetAxis("Horizontal");

			//moving to the limits
			velocity.x = horValue * speed;
			velocity.y = vertValue * speed;

			//changing the ship from side to side and idle
			if (horValue > 0.0f) {
				anim.SetInteger("direction",1);
			} else if (horValue < 0.0f) {
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
		shields[currentHP].SetActive (false);
		if(currentHP <= 0){
			currentHP = playerHP;
			StartCoroutine(gameMgr.DeathExplosion(currentHP, shields));
		}
	}

	public void RepositionShip(){
		transform.position = startingPosition;
		inUseCannon.transform.position = cannonPoint.transform.position;
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.GetComponent<ProjectileController>() != null && coll.gameObject.GetComponent<ProjectileController>().owner == target) {
			ProjectileController tempBullet = coll.gameObject.GetComponent<ProjectileController>();
			tempBullet.gameObject.SetActive(false);
			gameMgr.bulletsEAI.Push(tempBullet);
			Instantiate (pinkExplosionPrefab, tempBullet.transform.position, tempBullet.transform.rotation);
			currentHP = gameMgr.Hit(tempBullet.damageValue, currentHP, playerArmor);
			CheckHealth();
		}else if (coll.gameObject.GetComponent<Missle_Controller>() != null && coll.gameObject.GetComponent<Missle_Controller>().owner == target) {
			Missle_Controller missile = coll.gameObject.GetComponent<Missle_Controller>();
			Instantiate (pinkExplosionPrefab, missile.transform.position, missile.transform.rotation);
			currentHP = gameMgr.Hit(missile.damageValue, currentHP, playerArmor);
			missile.gameObject.SetActive(false);
			Destroy(coll.gameObject);
		}
	}
}
