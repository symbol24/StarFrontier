using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {

	public GameManager m_GameMgr;
	public float m_LimiterY = -4.9f;
	public int m_EaiHP = 1;
	public int m_CurrentHP;
	public int m_EaiArmor = 0;
	public int m_ScoreValue = 100;
	public GameObject m_BlueExplosion;
	public string m_Target = "player";
	public ProjectileController m_ProjectileToShoot;
	public ProjectileController[] m_ListOfProjectilesToShoot;
	public float m_ShootDelay = 1.0f;
	public EAIBehaviors[] m_BehaviorsPrefabs;
	private EAIBehaviors[] m_BehaviorsInstances;
	private EAIBehaviors m_DeathBehavior;
	public GameObject[] m_CannonReferances;
	public float m_MouvementSpeed = 1.0f;
	public GameObject m_HealthBar;

	void Start(){
		m_GameMgr = GameObject.Find ("GameManagerObj").GetComponent<GameManager> ();

		//instanciate all behaviours attached to EAI
		m_BehaviorsInstances = new EAIBehaviors[m_BehaviorsPrefabs.Length];
		for(int i = 0; i < m_BehaviorsPrefabs.Length; i++){
			m_BehaviorsInstances[i] = Instantiate(m_BehaviorsPrefabs[i], transform.position, transform.rotation) as EAIBehaviors;
			m_BehaviorsInstances[i].Init(this);
		}

		foreach (EAIBehaviors behavior in m_BehaviorsInstances) {
			if(behavior.m_BehaviorName == "death") m_DeathBehavior = behavior;
		}
		if(m_DeathBehavior == null) print("THERE IS NO DEATH BEHAVIOR!!!!");
		m_CurrentHP = m_EaiHP;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_GameMgr.m_CurrentState == GameManager.gameState.playing){
			foreach(EAIBehaviors behavior in m_BehaviorsInstances){
				if(behavior != null){
					behavior.UpdateBehavior();
				}
			}

			if(transform.position.y < m_LimiterY){
				DestroyObjectAndBehaviors();
			}
		}
	}

	private void checkHealth(){
		if (m_CurrentHP <= 0) {
			//DestroyObjectAndBehaviors();
			if(m_DeathBehavior.m_BehaviorDeathType == "boss")
				m_DeathBehavior.StartExplosions(50);
		}
	}

	public void DestroyObjectAndBehaviors(){
		gameObject.SetActive (false);
		m_GameMgr.UpdateScore(m_ScoreValue);
		foreach (EAIBehaviors behavior in m_BehaviorsInstances) {
			if(behavior != null){
				Destroy(behavior);
			}
		}
		Destroy (gameObject);
	}

	
	public void OnTriggerEnter2D(Collider2D coll) {
		ProjectileController tempBullet = coll.gameObject.GetComponent<ProjectileController>();
		if (tempBullet!= null && tempBullet.m_Owner == m_Target) {
			foreach(EAIBehaviors behavior in m_BehaviorsInstances){
				if(behavior != null){
					EAIBehaviorEvade evader = behavior.GetComponentInChildren<EAIBehaviorEvade>();
					if(evader != null){
						evader.EvaderHit();
					}
					EAIBehaviorTeleporter teleporter = behavior.GetComponentInChildren<EAIBehaviorTeleporter>();
					if(teleporter != null){
						teleporter.TeleporterHit();
					}
				}
			}
			Instantiate (m_BlueExplosion, tempBullet.transform.position, tempBullet.transform.rotation);
			m_CurrentHP = m_GameMgr.Hit(tempBullet.m_DamageValue, m_CurrentHP, m_EaiArmor);
			checkHealth();
			tempBullet.pushBullet(tempBullet);
		}
	}	
}
