using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemy_controller : MonoBehaviour {

	public Game_Manager gameMgr;
	public float limiterY;
	public int eaiHP;
	public int eaiArmor;
	public float lifeTimer;
	public int scoreValue;
	public GameObject exBlue;
	public string target;
	public EAIBehaviors[] m_BehaviorsPrefabs;
	private EAIBehaviors[] m_BehaviorsInstances;

	void Start(){
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
		m_BehaviorsInstances = new EAIBehaviors[m_BehaviorsPrefabs.Length];
		for(int i = 0; i < m_BehaviorsPrefabs.Length; i++){
			m_BehaviorsInstances[i] = Instantiate(m_BehaviorsPrefabs[i], transform.position, transform.rotation) as EAIBehaviors;
			m_BehaviorsInstances[i].Init(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(gameMgr.currentState == Game_Manager.gameState.playing){
			foreach(EAIBehaviors behavior in m_BehaviorsInstances){
				if(behavior != null){
					behavior.UpdateBehavior();
				}
			}

			if(transform.position.y < limiterY){
				DestroyObjectAndBehaviors();
			}
		}
	}

	//this is going to have to use the game manage for the hit function at some point
	public void Hit(int damage) {
		if (gameObject.renderer.isVisible) {
			eaiHP -= MitigateDamage (damage);
			if (eaiHP <= 0) {
				DestroyObjectAndBehaviors();
				gameMgr.UpdateScore(scoreValue);
			}else{

			}
		}
	}

	public void DestroyObjectAndBehaviors(){
		gameObject.SetActive (false);
		foreach (EAIBehaviors behavior in m_BehaviorsInstances) {
			if(behavior != null){
				Destroy(behavior);
			}
		}
		Destroy (gameObject);
	}
	
	private int MitigateDamage(int damage) {
		if (eaiArmor > damage) {
			return 0;
		}
		return damage - eaiArmor;
	}
	
	public void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.GetComponent<ProjectileController>() != null && coll.gameObject.GetComponent<ProjectileController>().owner == target) {
			foreach(EAIBehaviors behavior in m_BehaviorsInstances){
				if(behavior != null){
					EAIBehaviorEvade evader = behavior.GetComponentInChildren<EAIBehaviorEvade>();
					if(evader != null){
						evader.EvaderHit();
					}
				}
			}
			ProjectileController tempBullet = coll.gameObject.GetComponent<ProjectileController>();
			Instantiate (exBlue, tempBullet.transform.position, tempBullet.transform.rotation);
			Hit(tempBullet.damageValue);
			tempBullet.gameObject.SetActive(false);
			gameMgr.bulletsPlayer.Push(tempBullet);
		}else if (coll.gameObject.GetComponent<Missle_Controller>() != null && coll.gameObject.GetComponent<Missle_Controller>().owner == target) {
			Missle_Controller missile = coll.gameObject.GetComponent<Missle_Controller>();
			Instantiate (exBlue, missile.transform.position, missile.transform.rotation);
			Hit(missile.damageValue);
			missile.gameObject.SetActive(false);
			Destroy(coll.gameObject);
		}
	}	
}
