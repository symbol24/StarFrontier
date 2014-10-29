using UnityEngine;
using System.Collections;

public class Enemy_Spawner_Controller : MonoBehaviour {
	private Game_Manager gameMgr;

	//the enemy ships
	public enemy_controller[] eai;
	private int eaiSelector = 0;
	private int eaiCounter = 0;
	public int[] eaiSelRange;
	public float limiterY;
	public float limiterX;
	public float minSpawnRate;
	public float maxSpawnRate;
	private float nextSpawn = 0.0F;



	// Use this for initialization
	void Start () {
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameMgr != null && gameMgr.currentState == Game_Manager.gameState.playing){
			//spawn enemies
			if (Time.time > nextSpawn){
				nextSpawn = Time.time + Random.Range(minSpawnRate, maxSpawnRate);
				float tempX = Random.Range(-limiterX, limiterX);
				enemy_controller eaiClone;
				eaiClone = Instantiate(eai[eaiSelector], new Vector2(tempX, transform.position.y + limiterY), transform.rotation) as enemy_controller;
				eaiCounter++;
				
				//addind a tank into the mix from time to time
				if(eaiSelector != 1 && eaiCounter >= Random.Range(eaiSelRange[0], eaiSelRange[1])){
					eaiSelector = 1;
				}else{
					eaiSelector = 0;
				}
			}
		}
	}
}
