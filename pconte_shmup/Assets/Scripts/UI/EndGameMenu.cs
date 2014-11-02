using UnityEngine;
using System.Collections;

public class EndGameMenu : MonoBehaviour {
	private GameManager m_GameManager;
	public GameObject m_EndGameScreen;
	public GameObject m_EndGameSelector;
	public GUIText m_EndGameMessage;
	public GUIText m_EndGameScore;
	public Vector3[] m_EndGameMenuLocations;
	private int m_MenuPosID = 0;
	private float m_MenuTimer = 0.0f;
	private string m_CurrentLevelName;

	// Use this for initialization
	void Start () {
		m_GameManager = GameObject.Find ("GameManagerObj").GetComponent<GameManager> ();
		m_MenuTimer = Time.time;
		m_CurrentLevelName = Application.loadedLevelName;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_GameManager.m_CurrentState == GameManager.gameState.gameover){
			//check if display active, else activate
			if(!m_EndGameScreen.activeInHierarchy){
				m_EndGameScreen.SetActive(true);
			}

			//gameover menu controls
			if(m_MenuTimer < Time.time && (m_GameManager.m_VertValue > m_GameManager.m_MenuDeadSpot || (m_GameManager.m_VertValue < -m_GameManager.m_MenuDeadSpot))){
				m_MenuTimer = Time.time + m_GameManager.m_MenuDelayTimer;//to add a delay in input to prevent inputs taht are too quick
				MoveEndGameSelector(m_GameManager.m_VertValue);
			}
			if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(m_GameManager.m_ConfirmButton)){
				ConfirmEndGameSelect();
			}
		}
	}

	//gameover menu controller
	private void MoveEndGameSelector(float vertValue){
		Vector3 newPos = m_EndGameSelector.transform.localPosition;
		if(m_MenuPosID == 0 && vertValue < -m_GameManager.m_MenuDeadSpot){
			newPos = m_EndGameMenuLocations[1];
			m_MenuPosID = 1;
		}else if(m_MenuPosID == 1 && vertValue > m_GameManager.m_MenuDeadSpot){
			newPos = m_EndGameMenuLocations[0];
			m_MenuPosID = 0;
		}
		m_EndGameSelector.transform.localPosition = newPos;
	}

	private void ConfirmEndGameSelect (){
		if(m_MenuPosID == 0){
			Application.LoadLevel(m_CurrentLevelName);
		}else{
			Application.LoadLevel("MainMenu");
		}
	}

	public void DisplayGameOverScreen(string GameOverMessage){
		
		m_EndGameMessage.text = GameOverMessage;
		m_EndGameScore.text = m_GameManager.m_TotalScore.ToString ();
	}

}
