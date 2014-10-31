using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cannon_controller : MonoBehaviour {
	private Game_Manager gameMgr;
	public float offset;
	public float fireRate = 0.5F;
	private float nextFire = 0.0F;
	public KeyCode fireButton;
	public GameObject[] referance;
//	private GameObject tempEAI;
//	private GameObject[] allEAI;

	void Start(){
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
	}

	// Update is called once per frame
	void Update () {
		if(gameMgr.currentState == Game_Manager.gameState.playing){
			if ((Input.GetKey(KeyCode.Space) || Input.GetKey(fireButton)) && Time.time > nextFire){
				nextFire = Time.time + fireRate;
				foreach(GameObject refer in referance){
					ProjectileController tempBullet = gameMgr.bulletsPlayer.Pop();
					tempBullet.transform.position = new Vector2(refer.transform.position.x, refer.transform.position.y);
					tempBullet.transform.rotation = refer.transform.rotation;
					tempBullet.gameObject.SetActive(true);
				}
			}
		}
	}
}
