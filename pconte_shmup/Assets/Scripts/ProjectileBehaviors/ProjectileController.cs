using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileController : MonoBehaviour {
	private GameManager m_GameManager;
	public int m_DamageValue = 2;
	public float m_Speed = 5.0f;
	public string m_Target = "enemy";
	public string m_Owner = "player";
	public string m_Type = "";
	public ProjectileBehavior[] m_ProjectileBehaviorPrefabs;
	public ProjectileBehavior[] m_ProjectileBehaviorInstances;

	void Start(){
		m_GameManager = GameObject.Find ("GameManagerObj").GetComponent<GameManager> ();
		m_ProjectileBehaviorInstances = new ProjectileBehavior[m_ProjectileBehaviorPrefabs.Length];
		for(int i = 0; i < m_ProjectileBehaviorPrefabs.Length; i++){
			m_ProjectileBehaviorInstances[i] = Instantiate(m_ProjectileBehaviorPrefabs[i], transform.position, transform.rotation) as ProjectileBehavior;
			m_ProjectileBehaviorInstances[i].Init(this);
		}
	}

	// Update is called once per frame
	void Update () {
		if(m_GameManager.m_CurrentState == GameManager.gameState.playing){
			foreach(ProjectileBehavior behavior in m_ProjectileBehaviorInstances){
				behavior.UpdateBehavior();
			}
			if(m_Type != "beam"){
				//putting the bullets back into their respective STACK
				if(gameObject.activeInHierarchy && !gameObject.renderer.isVisible){
					pushBullet(this);
				}
			}
		}
	}

	public void pushBullet(ProjectileController currentBullet){
		Stack<ProjectileController> StackToUpdate = EntitiesCreator.GetStackToUpdate(currentBullet, m_GameManager);

		currentBullet.gameObject.SetActive(false);
		StackToUpdate.Push(this);
	}
}
