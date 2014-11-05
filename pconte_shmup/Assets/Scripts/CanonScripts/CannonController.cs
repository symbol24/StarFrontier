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
	private ProjectileController m_BeamInstance;
	private bool m_FiringBeam = false;

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
				if(m_ProjectileToShootPrefab.m_Type == "beam"){
					if(!m_FiringBeam){
						m_BeamInstance = Instantiate(m_ProjectileToShootPrefab, transform.position, transform.rotation) as ProjectileController;
						m_BeamInstance.gameObject.SetActive(true);
						m_FiringBeam = true;
					}else{
						m_BeamInstance.gameObject.SetActive(true);

					}
				}else{
					if(Time.time > m_NextFire){
					m_NextFire = Time.time + m_FireRate;
					foreach(GameObject refer in m_ReferencePointForBullet){
						PopABullet(refer, m_ProjectileToShootPrefab);
						}
					}
				}
			}else{
				if(m_GameManager.m_isShooting){
					m_GameManager.SwitchShieldStatus(false);
					m_FiringBeam = false;
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
