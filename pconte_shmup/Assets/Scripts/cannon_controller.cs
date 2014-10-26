using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cannon_controller : MonoBehaviour {
	public Game_Manager gameMgr;
	public float offset;
	public float fireRate = 0.5F;
	private float nextFire = 0.0F;
	public KeyCode fireButton;
	public GameObject referance;
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
				bullet_controller tempBullet = gameMgr.bullets.Pop();
				tempBullet.transform.position = new Vector2(referance.transform.position.x, referance.transform.position.y);
				tempBullet.transform.rotation = referance.transform.rotation;
				tempBullet.gameObject.SetActive(true);
			}
		}
	}


	//reference for homing shots
//	void HomingShots(){
//		allEAI = GameObject.FindGameObjectsWithTag("enemyship");
//		foreach(GameObject gTemp in allEAI){
//			float distance = (referance.transform.position - gTemp.transform.position).sqrMagnitude;
//			if(tempEAI != null){
//				if(gTemp.transform.position.y > referance.transform.position.y && distance > 0 && gTemp.transform.position.sqrMagnitude < tempEAI.transform.position.sqrMagnitude){
//					tempEAI = gTemp;
//				}
//			}else{
//				tempEAI = gTemp;
//			}
//		}
//		if (tempEAI != null) {
//			Vector3 tempV = (tempEAI.transform.position - referance.transform.position).normalized;
//			float angle = Mathf.Atan2(tempV.y, tempV.x)*Mathf.Rad2Deg;
//			Quaternion rotQ = new Quaternion ();
//			rotQ.eulerAngles = new Vector3(0,0,angle-90);
//			transform.rotation = rotQ;
//		}
//	}
}
