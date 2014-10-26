using UnityEngine;
using System.Collections;

public class Score_Controller : MonoBehaviour {
	public int totalScore;
	
	// Update is called once per frame
	void Update () {
		guiText.text = totalScore.ToString();	
	}

	public void UpdateScore(int score){
		totalScore += score;
	}
}
