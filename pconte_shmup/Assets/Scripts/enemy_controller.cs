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
	public float nextFire = 0.0F;
	public int scoreValue;
	public GameObject exBlue;
	public string target;
	public List<EAIBehaviors> m_Behaviors;

	void Start(){
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
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
			gameMgr.bulletsPlayer.Push(tempBullet);
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
