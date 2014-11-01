using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonController : MonoBehaviour {
	private GameManager m_GameManager;
	public float m_FireRate = 0.05F;
	private float m_NextFire = 0.0F;
	public GameObject[] m_ReferencePointForBullet;
	public ProjectileController m_ProjectileToShootPrefab;
	private Stack<ProjectileController> m_StackToUse;

	void Start(){
		m_GameManager = GameObject.Find ("GameManagerObj").GetComponent<GameManager> ();
		m_StackToUse = EntitiesCreator.GetStackToUpdate(m_ProjectileToShootPrefab, m_GameManager);
	}

	// Update is called once per frame
	void Update () {
		if(m_GameManager.m_CurrentState == GameManager.gameState.playing){
			if ((Input.GetKey(KeyCode.Space) || Input.GetKey(m_GameManager.m_ShootButton))){
				if(!m_GameManager.m_isShooting){
					m_GameManager.SwitchShieldStatus(true);
				}
				if(Time.time > m_NextFire){
				m_NextFire = Time.time + m_FireRate;
				foreach(GameObject refer in m_ReferencePointForBullet){
					PopABullet(refer, m_ProjectileToShootPrefab);
					}
				}
			}else{
				if(m_GameManager.m_isShooting){
					m_GameManager.SwitchShieldStatus(false);
				}
			}
		}
	}

	void PopABullet(GameObject refereance, ProjectileController bulletTemplate){
		ProjectileController tempBullet = m_StackToUse.Pop();
		tempBullet.transform.position = new Vector2(refereance.transform.position.x, refereance.transform.position.y);
		tempBullet.transform.rotation = refereance.transform.rotation;
		tempBullet.gameObject.SetActive(true);
	}
}
