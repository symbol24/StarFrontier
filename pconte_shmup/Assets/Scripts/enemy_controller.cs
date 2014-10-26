using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemy_controller : MonoBehaviour {
	public int shipID;
	public Game_Manager gameMgr;
	public float limiterY;
	public int eaiHP;
	public int eaiArmor;
	public float speed;
	public float evadSpeed;
	public float lifeTimer;
	public bullet_controller tempBullet;
	public float offset;
	public float minFireRate;
	public float maxFireRate;
	private float nextFire = 0.0F;
	public int scoreValue;
	public GameObject exBlue;
	public enum enemyState{
		normal,hiding,evading,
	}
	private enemyState state;
	private float stateTimer;
	public float evadeTime;
	public float hideTime;
	private float locator;
	public string target;

	void Start(){
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
		state = enemyState.normal;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameMgr.currentState == Game_Manager.gameState.playing){
			if(shipID == 0){
				EvaderType();
			}else if(shipID == 1){
				TankType();
			}
		}
		if(transform.position.y < limiterY){
			Destroy (gameObject);
		}
	}

	//this is going to have to use the game manage for the hit function at some point
	public void Hit(int damage) {
		if (gameObject.renderer.isVisible) {
			eaiHP -= MitigateDamage (damage);
			if (eaiHP <= 0) {
				Destroy (gameObject);
				gameMgr.UpdateScore(scoreValue);
			}else{
				stateTimer = Time.time + evadeTime;
				locator = transform.position.x;
				state = enemyState.evading;
			}
		}
	}
	
	private int MitigateDamage(int damage) {
		if (eaiArmor > damage) {
			return 0;
		}
		return damage - eaiArmor;
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.GetComponent<bullet_controller>() != null && coll.gameObject.GetComponent<bullet_controller>().owner == target) {
			tempBullet = coll.gameObject.GetComponent<bullet_controller>();
			Instantiate (exBlue, tempBullet.transform.position, tempBullet.transform.rotation);
			Hit(tempBullet.damageValue);
			tempBullet.gameObject.SetActive(false);
			gameMgr.bullets.Push(tempBullet);
		}else if (coll.gameObject.GetComponent<Missle_Controller>() != null && coll.gameObject.GetComponent<Missle_Controller>().owner == target) {
			Missle_Controller missile = coll.gameObject.GetComponent<Missle_Controller>();
			Instantiate (exBlue, missile.transform.position, missile.transform.rotation);
			Hit(missile.damageValue);
			missile.gameObject.SetActive(false);
			Destroy(coll.gameObject);
		}
	}

	//evade mini ai behavior for the pink sprite ship
	private void EvaderType(){
		switch (state) {
		case enemyState.normal:
			transform.Translate (Vector3.down * speed * Time.deltaTime, Space.World);
			
			if (Time.time > nextFire){
				nextFire = Time.time + Random.Range(minFireRate, maxFireRate);
				tempBullet = gameMgr.bulletsEAI.Pop();
				tempBullet.transform.position = new Vector2(transform.position.x, transform.position.y - offset);
				tempBullet.gameObject.SetActive(true);
			}
			break;
		case enemyState.hiding:
			transform.Translate (Vector3.down * Time.deltaTime, Space.World);
			if(Time.time > stateTimer){
				state = enemyState.normal;
			}
			break;
		case enemyState.evading:
			if(Time.time > stateTimer){
				state = enemyState.hiding;
				stateTimer = Time.time + evadeTime;
			}else{
				Vector3 direction = Vector3.left;
				if(locator < 0.0f){
					direction = Vector3.right;
				}
				transform.Translate (direction * evadSpeed * Time.deltaTime, Space.World);
			}
			break;
		}
	}

	//the yellowish sprite only move forwards but is slower and has more life
	private void TankType ()
	{
		transform.Translate (Vector3.down * speed * Time.deltaTime, Space.World);
		
		if (Time.time > nextFire){
			nextFire = Time.time + Random.Range(minFireRate, maxFireRate);
			tempBullet = gameMgr.bulletsEAI.Pop();
			tempBullet.transform.position = new Vector2(transform.position.x, transform.position.y - offset);
			tempBullet.gameObject.SetActive(true);
		}
	}
	
}
