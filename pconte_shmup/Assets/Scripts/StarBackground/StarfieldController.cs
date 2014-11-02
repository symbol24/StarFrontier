using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarfieldController : MonoBehaviour {
	private GameManager gameMgr;
	public MainMenuController mainMenuMgr;
	public StarController starPrefab;
	public Stack<StarController> starsTop = new Stack<StarController>();
	public Stack<StarController> starsMid = new Stack<StarController>();
	public Stack<StarController> starsBot = new Stack<StarController>();
	public int starCount;
	public int initialStars;
	public float maxY;
	public float maxX;
	public float[] maxSpawnRate;
	public float[] nextSpawn;
	private float previousX;
	public float spaceX;
	public Color[] starColors;
	public float[] starSpeed;
	public Vector2[] starScale;
	private GameObject[] allStars;
	public int warpFactor;
	public bool inWarp = false;

	// Use this for initialization
	void Start () {
		if(Application.loadedLevelName == "MainGame" || Application.loadedLevelName == "boss_prototype"){
			gameMgr = GameObject.Find ("GameManagerObj").GetComponent<GameManager> ();
		}else if(Application.loadedLevelName == "MainMenu"){
			mainMenuMgr = GameObject.Find ("MainMenuObj").GetComponent<MainMenuController> ();
		}

		starsTop = EntitiesCreator.CreatAStackOfStars (0, starPrefab, starCount, starColors, starSpeed, starScale, maxX, maxY);
		starsMid = EntitiesCreator.CreatAStackOfStars (1, starPrefab, starCount, starColors, starSpeed, starScale, maxX, maxY);
		starsBot = EntitiesCreator.CreatAStackOfStars (2, starPrefab, starCount, starColors, starSpeed, starScale, maxX, maxY);

		CreateInitialStars ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q) && !inWarp){
			WarpStars();
		}else if(Input.GetKeyDown(KeyCode.Q) && inWarp){
			UnWarpStars();
		}

		if(Application.loadedLevelName == "MainGame" || Application.loadedLevelName == "boss_prototype"){
			if(gameMgr != null && gameMgr.m_CurrentState == GameManager.gameState.playing){
				Stack<StarController> tempsStarStack;
				for(int i = 0; i < nextSpawn.Length; i++){
					if(Time.time > nextSpawn[i]){
						if(i == 0){
							tempsStarStack = starsTop;
						}else if(i == 1){
							tempsStarStack = starsMid;
						}else{
							tempsStarStack = starsBot;
						}
						popStar(tempsStarStack, i);
					}
				}
			}
		}else if(Application.loadedLevelName == "MainMenu"){
			if(mainMenuMgr != null & mainMenuMgr.currentState == MainMenuController.gameState.playing){
				Stack<StarController> tempsStarStack;
				for(int i = 0; i < nextSpawn.Length; i++){
					if(Time.time > nextSpawn[i]){
						if(i == 0){
							tempsStarStack = starsTop;
						}else if(i == 1){
							tempsStarStack = starsMid;
						}else{
							tempsStarStack = starsBot;
						}
						popStar(tempsStarStack, i);
					}
				}
			}
		}
	}

	private void CreateInitialStars(){
		
		StarController tempStar;
	
		//top stars
		for(int i = 0; i < initialStars; i++){
			tempStar = starsTop.Pop();
			tempStar.gameObject.SetActive(true);
		}
		//mid stars
		
		for(int i = 0; i < initialStars; i++){
			tempStar = starsMid.Pop();
			tempStar.gameObject.SetActive(true);
		}
		//bottom stars
		
		for(int i = 0; i < initialStars; i++){
			tempStar = starsBot.Pop();
			tempStar.gameObject.SetActive(true);
		}
	}

	public void WarpStars(){
		allStars = GameObject.FindGameObjectsWithTag ("simpleStar");
		foreach (GameObject gStar in allStars) {
			gStar.GetComponent<StarController>().WarpStar(warpFactor);
		}
		for(int i = 0; i < maxSpawnRate.Length; i++){
			maxSpawnRate[i] = maxSpawnRate[i]/warpFactor;
		}
		inWarp = true;
	}

	public void UnWarpStars(){
		allStars = GameObject.FindGameObjectsWithTag ("simpleStar");
		foreach (GameObject gStar in allStars) {
			gStar.GetComponent<StarController>().UnWarpStar();
		}
		for(int i = 0; i < maxSpawnRate.Length; i++){
			maxSpawnRate[i] = maxSpawnRate[i]*warpFactor;
		}
		inWarp = false;
	}
    
	public void popStar(Stack<StarController> starStack, int parrallaxID){
		StarController tempStar;
		float tempX = Random.Range(-maxX, maxX);
		nextSpawn[parrallaxID] = Time.time + maxSpawnRate[parrallaxID];
		tempStar = starStack.Pop();
		tempStar.transform.position = new Vector2(tempX, maxY);
		if(!inWarp && tempStar.isInWarp){
			tempStar.UnWarpStar();
		}
		if(inWarp && !tempStar.isInWarp){
			tempStar.WarpStar(warpFactor);
		}
		ValidateStarsInWarp ();
		tempStar.gameObject.SetActive(true);
	}

	public void ValidateStarsInWarp(){
		allStars = GameObject.FindGameObjectsWithTag ("simpleStar");
		foreach (GameObject gStar in allStars) {
			if(gStar.GetComponent<StarController>() != null){
				gStar.GetComponent<StarController>().WarpStar(warpFactor);
				if(!inWarp && gStar.GetComponent<StarController>().isInWarp){
					gStar.GetComponent<StarController>().UnWarpStar();
				}
				if(inWarp && !gStar.GetComponent<StarController>().isInWarp){
					gStar.GetComponent<StarController>().WarpStar(warpFactor);
				}
			}
		}
	}
}
