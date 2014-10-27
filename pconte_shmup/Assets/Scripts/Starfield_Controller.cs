using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Starfield_Controller : MonoBehaviour {
	public Game_Manager gameMgr;
	public Main_Menu_Controller mainMenuMgr;
	public Star_Controller starPrefab;
	public Stack<Star_Controller> starsTop = new Stack<Star_Controller>();
	public Stack<Star_Controller> starsMid = new Stack<Star_Controller>();
	public Stack<Star_Controller> starsBot = new Stack<Star_Controller>();
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
		if(Application.loadedLevelName == "MainGame"){
			gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
		}else if(Application.loadedLevelName == "MainMenu"){
			mainMenuMgr = GameObject.Find ("MainMenuObj").GetComponent<Main_Menu_Controller> ();
		}
		int colorID = 0;
		Star_Controller tempStar;
		//top stars
		for(int i = 0; i < starCount; i++){
			tempStar = Instantiate(starPrefab, transform.position, transform.rotation) as Star_Controller;
			tempStar.createStar(0, starSpeed[0], starColors[colorID], starScale[0]);
			tempStar.gameObject.SetActive(false);
			starsTop.Push(tempStar);
			if(colorID == 4){
				colorID = 0;
			}else{
				colorID++;
			}
		}
		for(int i = 0; i < initialStars; i++){
			tempStar = starsTop.Pop();
			float tempX = Random.Range(-maxX, maxX);
			float tempY = Random.Range(-maxY, maxY);
			tempStar.transform.position = new Vector2(tempX, tempY);
			tempStar.gameObject.SetActive(true);
		}
		//mid stars
		for(int i = 0; i < starCount; i++){
			tempStar = Instantiate(starPrefab, transform.position, transform.rotation) as Star_Controller;
			tempStar.createStar(1, starSpeed[1], starColors[colorID], starScale[1]);
			tempStar.gameObject.SetActive(false);
			starsMid.Push(tempStar);
			if(colorID == 0){
				colorID++;
			}else if(colorID == 1){
				colorID++;
			}else{
				colorID = 0;
			}
		}
		for(int i = 0; i < initialStars; i++){
			tempStar = starsMid.Pop();
			float tempX = Random.Range(-maxX, maxX);
			float tempY = Random.Range(-maxY, maxY);
			tempStar.transform.position = new Vector2(tempX, tempY);
			tempStar.gameObject.SetActive(true);
		}
		//bottom stars
		for(int i = 0; i < starCount; i++){
			tempStar = Instantiate(starPrefab, transform.position, transform.rotation) as Star_Controller;
			tempStar.createStar(2, starSpeed[2], starColors[colorID], starScale[2]);
			tempStar.gameObject.SetActive(false);
			starsBot.Push(tempStar);
			if(colorID == 0){
				colorID++;
			}else if(colorID == 1){
				colorID++;
			}else{
				colorID = 0;
			}
		}
		for(int i = 0; i < initialStars; i++){
			tempStar = starsBot.Pop();
			float tempX = Random.Range(-maxX, maxX);
			float tempY = Random.Range(-maxY, maxY);
			tempStar.transform.position = new Vector2(tempX, tempY);
			tempStar.gameObject.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q) && !inWarp){
			WarpStars();
		}else if(Input.GetKeyDown(KeyCode.Q) && inWarp){
			UnWarpStars();
		}

		if(Application.loadedLevelName == "MainGame"){
			if(gameMgr != null && gameMgr.currentState == Game_Manager.gameState.playing){
				Stack<Star_Controller> tempsStarStack;
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
			if(mainMenuMgr != null & mainMenuMgr.currentState == Main_Menu_Controller.gameState.playing){
				Stack<Star_Controller> tempsStarStack;
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

	public void WarpStars(){
		allStars = GameObject.FindGameObjectsWithTag ("simpleStar");
		foreach (GameObject gStar in allStars) {
			gStar.GetComponent<Star_Controller>().WarpStar(warpFactor);
		}
		for(int i = 0; i < maxSpawnRate.Length; i++){
			maxSpawnRate[i] = maxSpawnRate[i]/warpFactor;
		}
		inWarp = true;
	}

	public void UnWarpStars(){
		allStars = GameObject.FindGameObjectsWithTag ("simpleStar");
		foreach (GameObject gStar in allStars) {
			gStar.GetComponent<Star_Controller>().UnWarpStar();
		}
		for(int i = 0; i < maxSpawnRate.Length; i++){
			maxSpawnRate[i] = maxSpawnRate[i]*warpFactor;
		}
		inWarp = false;
	}
    
	public void popStar(Stack<Star_Controller> starStack, int parrallaxID){
		Star_Controller tempStar;
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
			if(gStar.GetComponent<Star_Controller>() != null){
				gStar.GetComponent<Star_Controller>().WarpStar(warpFactor);
				if(!inWarp && gStar.GetComponent<Star_Controller>().isInWarp){
					gStar.GetComponent<Star_Controller>().UnWarpStar();
				}
				if(inWarp && !gStar.GetComponent<Star_Controller>().isInWarp){
					gStar.GetComponent<Star_Controller>().WarpStar(warpFactor);
				}
			}
		}
	}
}
