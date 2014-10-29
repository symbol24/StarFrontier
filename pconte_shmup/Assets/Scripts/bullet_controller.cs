using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bullet_controller : MonoBehaviour {
	private Game_Manager gameMgr;
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
			transform.Translate (Vector3.up * speed * Time.deltaTime, Space.World);

			//putting the bullets back into their respective STACK
			if(owner == "player" && gameObject.activeInHierarchy && !gameObject.renderer.isVisible){
				pushBullet(gameMgr.bulletsPlayer);

			}
			if(owner == "enemy" && gameObject.activeInHierarchy && !gameObject.renderer.isVisible){
				pushBullet(gameMgr.bulletsEAI);
				
			}
		}
	}

	void pushBullet(Stack<bullet_controller> BulletStack){
		gameObject.SetActive(false);
		BulletStack.Push(gameObject.GetComponent<bullet_controller>());
	}
}
