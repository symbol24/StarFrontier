using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileController : MonoBehaviour {
	private Game_Manager gameMgr;
	public int damageValue;
	public float speed;
	public string target;
	public string owner;
	public ProjectileBehavior[] m_ProjectileBehaviorPrefabs;
	private ProjectileBehavior[] m_ProjectileBehaviorInstances;

	void Start(){
		gameMgr = GameObject.Find ("GameManagerObj").GetComponent<Game_Manager> ();
		m_ProjectileBehaviorInstances = new ProjectileBehavior[m_ProjectileBehaviorPrefabs.Length];
		for(int i = 0; i < m_ProjectileBehaviorPrefabs.Length; i++){
			m_ProjectileBehaviorInstances[i] = Instantiate(m_ProjectileBehaviorPrefabs[i], transform.position, transform.rotation) as ProjectileBehavior;
			m_ProjectileBehaviorInstances[i].Init(this);
		}
	}

	// Update is called once per frame
	void Update () {
		if(gameMgr.currentState == Game_Manager.gameState.playing){
			foreach(ProjectileBehavior behavior in m_ProjectileBehaviorInstances){
				behavior.UpdateBehavior();
			}

			//putting the bullets back into their respective STACK
			if(owner == "player" && gameObject.activeInHierarchy && !gameObject.renderer.isVisible){
				pushBullet(gameMgr.bulletsPlayer);

			}
			if(owner == "enemy" && gameObject.activeInHierarchy && !gameObject.renderer.isVisible){
				pushBullet(gameMgr.bulletsEAI);
				
			}
		}
	}

	void pushBullet(Stack<ProjectileController> BulletStack){
		gameObject.SetActive(false);
		BulletStack.Push(gameObject.GetComponent<ProjectileController>());
	}
}
