using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntitiesCreator : MonoBehaviour {
	public static Stack<ProjectileController> bullets = new Stack<ProjectileController>();
	public static Stack<StarController> stars = new Stack<StarController>();

	public static Stack<ProjectileController> CreatAStackOfBullets(ProjectileController bulletPrefabToUse, int amountOfBullets){
		bullets = new Stack<ProjectileController>();

		for(int i = 0; i < amountOfBullets; i++){
			ProjectileController oneBullet = Instantiate(bulletPrefabToUse, bulletPrefabToUse.transform.position, bulletPrefabToUse.transform.rotation) as ProjectileController;
			oneBullet.gameObject.SetActive(false);
			bullets.Push(oneBullet);
		}

		return bullets;
	}

	public static Stack<StarController> CreatAStackOfStars(int starId, StarController starPrefab, int amountOfStars, Color[] starColors, float[] starSpeed, Vector2[] starScale, float maxX, float maxY){
		stars = new Stack<StarController>();
		int colorID = 0;
		for(int i = 0; i < amountOfStars; i++){
			StarController oneStar = Instantiate(starPrefab, starPrefab.transform.position, starPrefab.transform.rotation) as StarController;
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

	public static string GetTypeOfBullet(ProjectileController currentBullet){
		string bulletType = "bullet";
		foreach(ProjectileBehavior behavior in currentBullet.m_ProjectileBehaviorInstances){
			if(behavior.m_Type != "bullet"){
				bulletType = behavior.m_Type;
			}
		}
		return bulletType;
	}
	
	public static Stack<ProjectileController> GetStackToUpdate(ProjectileController currentBullet, GameManager gameManager){
		Stack<ProjectileController> StackToUpdate = new Stack<ProjectileController>();
		string bulletType = GetTypeOfBullet(currentBullet);
		if(currentBullet.m_Owner == "player"){
			if(bulletType == "missile"){
				StackToUpdate = gameManager.m_MissilesPlayer;
			}else{
				StackToUpdate = gameManager.m_BulletsPlayer;
			}
		}else if(currentBullet.m_Owner == "enemy"){
			StackToUpdate = gameManager.m_BulletsEAI;
		}
		return StackToUpdate;
	}
}