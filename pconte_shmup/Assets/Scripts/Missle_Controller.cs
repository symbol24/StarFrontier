using UnityEngine;
using System.Collections;

public class Missle_Controller : MonoBehaviour {
	public Game_Manager gameMgr;
	public int damageValue;
	public float speed;
	public string target;
	public string owner;
	private enemy_controller homingTargetOne;

	//TO BE UPDATE WITH HOMING ALSO
	void Start(){
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
		Object[] allEAI = GameObject.FindObjectsOfType (typeof(enemy_controller));
		foreach (enemy_controller ec in allEAI) {
			if(ec != null){
				float distance = (transform.position - ec.transform.position).sqrMagnitude;
				if(homingTargetOne != null){
					if(ec.transform.position.y > transform.position.y && distance > 0 && ec.transform.position.sqrMagnitude < homingTargetOne.transform.position.sqrMagnitude){
						homingTargetOne = ec;
					}
				}else{
					homingTargetOne = ec;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(gameMgr.currentState == Game_Manager.gameState.playing){

			if (homingTargetOne != null) {
				Vector3 tempV = (homingTargetOne.transform.position - transform.position).normalized;
				float angle = Mathf.Atan2(tempV.y, tempV.x)*Mathf.Rad2Deg;
				Quaternion rotQ = new Quaternion ();
				rotQ.eulerAngles = new Vector3(0,0,angle-90);
				transform.rotation = rotQ;
			}

			transform.Translate (transform.up * speed * Time.deltaTime, Space.World);
			
			//putting the bullets back into their respective STACK
			if(gameObject.activeInHierarchy && !gameObject.renderer.isVisible){
				Destroy (gameObject);
			}

		}
	}
}
