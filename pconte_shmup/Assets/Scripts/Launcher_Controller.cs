using UnityEngine;
using System.Collections;

public class Launcher_Controller : MonoBehaviour {

	public Game_Manager gameMgr;
	public float offset;
	public float fireRate = 0.5F;
	private float nextFire = 0.0F;
	public KeyCode fireButton;
	public GameObject referance;
	public Missle_Controller missilePrefab;
	
	void Start(){
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameMgr.currentState == Game_Manager.gameState.playing){
			if ((Input.GetKey(KeyCode.Space) || Input.GetKey(fireButton)) && Time.time > nextFire){
				nextFire = Time.time + fireRate;
				Missle_Controller missile = Instantiate(missilePrefab, referance.transform.position, referance.transform.rotation) as Missle_Controller;
			}
		}
	}
}
