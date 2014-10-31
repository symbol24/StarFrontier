using UnityEngine;
using System.Collections;

public class Launcher_Controller : MonoBehaviour {

	private Game_Manager gameMgr;
	public float offset;
	public float fireRate = 0.5F;
	private float nextFire = 0.0F;
	public KeyCode fireButton;
	public GameObject[] referance;
	public Missle_Controller missilePrefab;
	
	void Start(){
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameMgr.currentState == Game_Manager.gameState.playing){
			if ((Input.GetKey(KeyCode.Space) || Input.GetKey(fireButton)) && Time.time > nextFire){
				nextFire = Time.time + fireRate;
				foreach(GameObject refer in referance){
					Missle_Controller missile = Instantiate(missilePrefab, refer.transform.position, refer.transform.rotation) as Missle_Controller;
				}
			}
		}
	}
}
