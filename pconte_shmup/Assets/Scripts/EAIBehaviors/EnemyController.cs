using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {

	public GameManager gameMgr;
	public float limiterY;
	public int eaiHP;
	public int m_currentHP;
	public int eaiArmor;
	public int scoreValue;
	public GameObject exBlue;
	public string target;
	public EAIBehaviors[] m_BehaviorsPrefabs;
	private EAIBehaviors[] m_BehaviorsInstances;
	public GameObject[] m_CannonReferances;

	void Start(){
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<GameManager> ();
		m_BehaviorsInstances = new EAIBehaviors[m_BehaviorsPrefabs.Length];
		for(int i = 0; i < m_BehaviorsPrefabs.Length; i++){
			m_BehaviorsInstances[i] = Instantiate(m_BehaviorsPrefabs[i], transform.position, transform.rotation) as EAIBehaviors;
			m_BehaviorsInstances[i].Init(this);
		}
		m_currentHP = eaiHP;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameMgr.m_CurrentState == GameManager.gameState.playing){
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
		m_currentHP -= MitigateDamage (damage);
		if (m_currentHP <= 0) {
			DestroyObjectAndBehaviors();
			gameMgr.UpdateScore(scoreValue);
		}else{

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

		ProjectileController tempBullet = coll.gameObject.GetComponent<ProjectileController>();
		if (tempBullet!= null && tempBullet.m_Owner == target) {
			foreach(EAIBehaviors behavior in m_BehaviorsInstances){
				if(behavior != null){
					EAIBehaviorEvade evader = behavior.GetComponentInChildren<EAIBehaviorEvade>();
					if(evader != null){
						evader.EvaderHit();
					}
				}
			}
			Instantiate (exBlue, tempBullet.transform.position, tempBullet.transform.rotation);
			Hit(tempBullet.m_DamageValue);
			tempBullet.pushBullet(tempBullet);
		}
	}	
}
