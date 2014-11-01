using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarController : MonoBehaviour {
	private GameManager gameMgr;
	public MainMenuController mainMenuMgr;
	public StarfieldController starfield;
	public float speed;
	public float reseter;
	private Color myColor;
	private Vector2 vScale;
	private SpriteRenderer mySprite;
	public int id;
	private float origSpeed;
	public int warpFactor = 1;
	public bool isInWarp = false;

	//simple way to set default values without a constroctor
	public void createStar(int zId, float zSpeed, Color zColor, Vector2 zScale){
		id = zId;
		myColor = zColor;
		speed = zSpeed;
		origSpeed = zSpeed;
		vScale = zScale;
	}

	// Use this for initialization
	void Start () {
		if(Application.loadedLevelName == "MainGame"){
			gameMgr = GameObject.Find ("GameManagerObj").GetComponent<GameManager> ();
		}else if(Application.loadedLevelName == "MainMenu"){
			mainMenuMgr = GameObject.Find ("MainMenuObj").GetComponent<MainMenuController> ();
		}
		starfield = GameObject.Find ("Starfield").GetComponent<StarfieldController> ();
		mySprite = gameObject.GetComponent<SpriteRenderer>();
		//setting the alpha to the level of parrallaxing
		if(id == 0){
			myColor.a = 0.8f;
		}
		if(id == 1){
			myColor.a = 0.6f;
		}
		if(id == 2){
			myColor.a = 0.4f;
		}
		mySprite.color = myColor;

	}
	
	// Update is called once per frame
	void Update () {

		if((Application.loadedLevelName == "MainGame" || Application.loadedLevelName == "boss_prototype" ) && gameMgr != null && gameMgr.m_CurrentState == GameManager.gameState.playing){
			MoveStar();
		}else if(Application.loadedLevelName == "MainMenu" && mainMenuMgr != null & mainMenuMgr.currentState == MainMenuController.gameState.playing){
			MoveStar();
		}
	}

	public void MoveStar (){
		transform.Translate (Vector3.down * speed * warpFactor * Time.deltaTime, Space.World);
		if(transform.position.y <= -reseter){
			gameObject.SetActive(false);
			if(id == 0){
				starfield.starsTop.Push (gameObject.GetComponent<StarController>());
			}else if(id == 1){
				starfield.starsMid.Push (gameObject.GetComponent<StarController>());
			}else{
				starfield.starsBot.Push (gameObject.GetComponent<StarController>());
			}
		}
	}
	
	public void WarpStar(int myWarpFactor){
		if(!isInWarp){
			speed = speed * myWarpFactor;
			transform.localScale = new Vector2 (transform.localScale.x, transform.localScale.y * myWarpFactor);
			isInWarp = true;
		}
	}

	public void UnWarpStar(){
		if(isInWarp){
			speed = origSpeed;
			transform.localScale = vScale;
			isInWarp = false;
		}
	}
}
