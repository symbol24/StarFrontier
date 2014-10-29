using UnityEngine;
using System.Collections;

public class EAIBehaviorEvade : EAIBehaviors {
	
	public enum enemyState{
		normal,hiding,evading,
	}
	private enemyState m_State;
	private float m_StateTimer;
	public float m_EvadeTimer;
	public float m_HideTimer;
	public float m_Locator;

	protected override void Start(){
		base.Start ();
		m_State = enemyState.normal;
	}

	protected override void Update() {
		base.Update ();
		print(m_State);
		switch (m_State) {
		case enemyState.normal:
			transform.Translate (Vector3.down * m_Controller.speed * Time.deltaTime, Space.World);
			
			if (Time.time > m_Controller.nextFire){
				m_Controller.nextFire = Time.time + Random.Range(m_Controller.minFireRate, m_Controller.maxFireRate);
				m_Controller.tempBullet = m_Controller.gameMgr.bulletsEAI.Pop();
				m_Controller.tempBullet.transform.position = new Vector2(transform.position.x, transform.position.y - m_Controller.offset);
				m_Controller.tempBullet.gameObject.SetActive(true);
			}
			break;
		case enemyState.hiding:
			transform.Translate (Vector3.down * Time.deltaTime, Space.World);
			if(Time.time > m_StateTimer){
				m_State = enemyState.normal;
			}
			break;
		case enemyState.evading:
			if(Time.time > m_StateTimer){
				m_State = enemyState.hiding;
				m_StateTimer = Time.time + m_EvadeTimer;
			}else{
				Vector3 direction = Vector3.left;
				if(m_Locator < 0.0f){
					direction = Vector3.right;
				}
				transform.Translate (direction * m_Controller.evadSpeed * Time.deltaTime, Space.World);
			}
			break;
		}

	}

	public float m_EvadeTime = 2.0f;

	public void EvaderHit(){
		m_StateTimer = Time.time + m_EvadeTimer;
		m_Locator = transform.position.x;
		m_State = enemyState.evading;
	}
}
