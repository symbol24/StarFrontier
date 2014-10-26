using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bullet_controller : MonoBehaviour {
	public Game_Manager gameMgr;
	public int damageValue;
	public float speed;
	public string target;
	public string owner;

	void Start(){
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
	}

	// Update is called once per frame
	void Update () {
		if(gameMgr.currentState == Game_Manager.gameState.playing){
			transform.Translate (transform.up * speed * Time.deltaTime, Space.World);

			//putting the bullets back into their respective STACK
			if(owner == "player" && gameObject.activeInHierarchy && !gameObject.renderer.isVisible){
				gameObject.SetActive(false);
				gameMgr.bullets.Push(gameObject.GetComponent<bullet_controller>());

			}
			if(owner == "enemy" && gameObject.activeInHierarchy && !gameObject.renderer.isVisible){
				gameObject.SetActive(false);
				gameMgr.bulletsEAI.Push(gameObject.GetComponent<bullet_controller>());
				
			}
		}
	}
}
