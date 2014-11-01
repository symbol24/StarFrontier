using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	public GameManager gameMgr;

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
	public GameObject[] cannons;
	public GameObject inUseCannon;
	public int cannonID;

	//the shields used for absorbtion effect
	public GameObject m_Shield;

	//hit comparerererer
	public string target;

	void Start(){
		startingPosition = transform.position;
		anim = GetComponent<Animator>();
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<GameManager> ();

		//instantiating the first cannon
		inUseCannon = Instantiate (cannons[cannonID], cannonPoint.transform.position, cannonPoint.transform.rotation) as GameObject;
		inUseCannon.transform.parent = transform;
		currentHP = playerHP;
	}
	
	void Update () {
		if(gameMgr.m_CurrentState == GameManager.gameState.playing && !isDead){

			//move
			velocity.x = gameMgr.m_HorValue * speed;
			velocity.y = gameMgr.m_VertValue * speed;

			//changing the ship from side to side and idle
			if (gameMgr.m_HorValue > 0.0f) {
				anim.SetInteger("direction",1);
			} else if (gameMgr.m_HorValue < 0.0f) {
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
			StartCoroutine(gameMgr.DeathExplosion(currentHP));
		}else{
			m_Shield.SetActive (false);
		}
	}

	public void RepositionShip(){
		transform.position = startingPosition;
		inUseCannon.transform.position = cannonPoint.transform.position;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		
		ProjectileController tempBullet = coll.gameObject.GetComponent<ProjectileController>();
		if (tempBullet!= null && tempBullet.m_Owner == target) {
			Instantiate (pinkExplosionPrefab, tempBullet.transform.position, tempBullet.transform.rotation);
			currentHP = gameMgr.Hit(tempBullet.m_DamageValue, currentHP, playerArmor);
			CheckHealth();
			tempBullet.pushBullet(tempBullet);
		}
	}
}
