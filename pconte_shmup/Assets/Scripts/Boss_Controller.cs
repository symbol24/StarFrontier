using UnityEngine;
using System.Collections;

public class Boss_Controller : MonoBehaviour {
	private Game_Manager gameMgr;
	public float limiterY;
	public int eaiHP;
	public int eaiArmor;
	public float speed;
	public bullet_controller tempBullet;
	public float offset;
	public float minFireRate;
	public float maxFireRate;
	private float nextFire = 0.0F;
	public int scoreValue;
	public GameObject exBlue;
	public string target;
	public GameObject[] cannonReferances;

	// Use this for initialization
	void Start () {
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
		nextFire = Time.time + minFireRate;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameMgr.currentState == Game_Manager.gameState.playing) {
			ShootAllCannons();
		}
	}

	private void ShootAllCannons(){
		if (Time.time > nextFire){
			foreach(GameObject cref in cannonReferances){
			nextFire = Time.time + Random.Range(minFireRate, maxFireRate);
			tempBullet = gameMgr.bulletsEAI.Pop();
				tempBullet.transform.position = new Vector2(cref.transform.position.x, cref.transform.position.y - offset);
			tempBullet.gameObject.SetActive(true);
			}
		}
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

	//this is going to have to use the game manage for the hit function at some point
	public void Hit(int damage) {
		if (gameObject.activeInHierarchy) {
			eaiHP -= MitigateDamage (damage);
			if (eaiHP <= 0) {
				Destroy (gameObject);
				gameMgr.UpdateScore(scoreValue);
			}
		}
	}
	
	private int MitigateDamage(int damage) {
		if (eaiArmor > damage) {
			return 0;
		}
		return damage - eaiArmor;
	}
}
