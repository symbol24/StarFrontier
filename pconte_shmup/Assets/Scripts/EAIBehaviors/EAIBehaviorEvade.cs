using UnityEngine;
using System.Collections;

public class EAIBehaviorEvade : EAIBehaviors {
	
	public float evadSpeed = 5.0f;

	public enum enemyState{
		normal,hiding,evading,
	}
	private enemyState m_State;
	private float m_StateTimer;
	public float m_EvadeTimer;
	public float m_HideTimer;
	public float m_Locator;

	public override void Start(){
		base.Start ();
		m_State = enemyState.normal;
		transform.position = m_Controller.transform.position;
	}

	public override void UpdateBehavior() {
		switch (m_State) {
		case enemyState.normal:

			break;
		case enemyState.hiding:
			m_Controller.transform.Translate (Vector3.down * Time.deltaTime, Space.World);
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
				m_Controller.transform.Translate (direction * evadSpeed * Time.deltaTime, Space.World);
			}
			break;
		}
	}

	public void EvaderHit(){
		m_StateTimer = Time.time + m_EvadeTimer;
		m_Locator = m_Controller.transform.position.x;
		m_State = enemyState.evading;
	}
}
