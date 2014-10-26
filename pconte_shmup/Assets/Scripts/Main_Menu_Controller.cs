using UnityEngine;
using System.Collections;

public class Main_Menu_Controller : MonoBehaviour {
	//game state enum
	public enum gameState{
		playing, paused, gameover
	}
	public gameState currentState;



	// Use this for initialization
	void Start () {
		//setting the game state to playing
		currentState = gameState.playing;
		
		//for some reason at work, i had to force the time scale to have the game play once the start is passed
		Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
