using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	private GameManager m_GameManager;
	public GameObject m_PauseScreenOverlay;
	public GameObject m_PauseSelector;
	public Vector3[] m_PauseMenuLocations;
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
		if(m_GameManager.m_CurrentState == GameManager.gameState.playing){
			//pause the game on button press
			if(Input.GetKeyDown("p") || Input.GetKeyDown(m_GameManager.m_PauseButton)){
				m_GameManager.m_CurrentState = PauseGame(m_GameManager.m_CurrentState);
			}
		}else if(m_GameManager.m_CurrentState == GameManager.gameState.paused){
		//pause menu controls
			if(m_GameManager.m_MenuDelayTimer < Time.time && (m_GameManager.m_VertValue > m_GameManager.m_MenuDeadSpot || m_GameManager.m_VertValue < -m_GameManager.m_MenuDeadSpot)){
				m_GameManager.m_MenuDelayTimer = Time.time + m_MenuTimer;//to add a delay in input to prevent inputs taht are too quick
				MovePauseSelector(m_GameManager.m_VertValue);
			}
			if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(m_GameManager.m_ConfirmButton)){
				ConfirmPauseSelect();
			}
			
			//open and close pause menu
			if(Input.GetKeyDown("p") || Input.GetKeyDown(m_GameManager.m_PauseButton)){
				m_GameManager.m_CurrentState = PauseGame(m_GameManager.m_CurrentState);
			}
		}
	
	}

	//setting the gamestate to paused or playing
	public GameManager.gameState PauseGame(GameManager.gameState currentState){
		switch(currentState){
		case GameManager.gameState.playing:
			currentState = GameManager.gameState.paused;
			m_PauseScreenOverlay.SetActive(true);
			break;
		case GameManager.gameState.paused:
			currentState = GameManager.gameState.playing;
			m_PauseScreenOverlay.SetActive(false);
			break;
		default:
			currentState = GameManager.gameState.paused;
			break;
		}

		return currentState;
	}
	
	//pause menu controller
	private void MovePauseSelector (float vertValue){
		Vector3 newPos = m_PauseSelector.transform.localPosition;
		if(m_MenuPosID == 0 && vertValue < -m_GameManager.m_MenuDeadSpot){
			newPos = m_PauseMenuLocations[1];
			m_MenuPosID = 1;
		}else if(m_MenuPosID == 1 && vertValue < -m_GameManager.m_MenuDeadSpot){
			newPos= m_PauseMenuLocations[2];
			m_MenuPosID = 2;
		}else if(m_MenuPosID == 1 && vertValue > m_GameManager.m_MenuDeadSpot){
			newPos = m_PauseMenuLocations[0];
			m_MenuPosID = 0;
		}else if(m_MenuPosID == 2 && vertValue > m_GameManager.m_MenuDeadSpot){
			newPos = m_PauseMenuLocations[1];
			m_MenuPosID = 1;
		}
		m_PauseSelector.transform.localPosition = newPos;
	}
	
	private void ConfirmPauseSelect (){
		if(m_MenuPosID == 0){
			m_GameManager.m_CurrentState = PauseGame(m_GameManager.m_CurrentState);
		}else if(m_MenuPosID == 1){
			Application.LoadLevel(m_CurrentLevelName);
		}else{
			Application.LoadLevel("MainMenu");
		}
	}
}
