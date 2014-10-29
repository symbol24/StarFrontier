using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entities_Creator : MonoBehaviour {
	public static Stack<bullet_controller> bullets = new Stack<bullet_controller>();
	public static Stack<Star_Controller> stars = new Stack<Star_Controller>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static Stack<bullet_controller> CreatAStackOfBullets(bullet_controller bulletPrefabToUse, int amountOfBullets){
		bullets = new Stack<bullet_controller>();

		for(int i = 0; i < amountOfBullets; i++){
			bullet_controller oneBullet = Instantiate(bulletPrefabToUse, bulletPrefabToUse.transform.position, bulletPrefabToUse.transform.rotation) as bullet_controller;
			oneBullet.gameObject.SetActive(false);
			bullets.Push(oneBullet);
		}

		return bullets;
	}

	public static Stack<Star_Controller> CreatAStackOfStars(int starId, Star_Controller starPrefab, int amountOfStars, Color[] starColors, float[] starSpeed, Vector2[] starScale, float maxX, float maxY){
		stars = new Stack<Star_Controller>();
		int colorID = 0;
		for(int i = 0; i < amountOfStars; i++){
			Star_Controller oneStar = Instantiate(starPrefab, starPrefab.transform.position, starPrefab.transform.rotation) as Star_Controller;
			float tempX = Random.Range(-maxX, maxX);
			float tempY = Random.Range(-maxY, maxY);
			oneStar.transform.position = new Vector2(tempX, tempY);
			oneStar.createStar(starId, starSpeed[starId], starColors[colorID], starScale[starId]);
			oneStar.gameObject.SetActive(false);
			stars.Push(oneStar);

			if(colorID == 4){
				colorID = 0;
			}else{
				colorID++;
			}
		}
		return stars;
	}
}