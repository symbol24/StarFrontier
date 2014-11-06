using UnityEngine;
using System.Collections;

public class EAIBehaviorMoveLeft : EAIBehaviors {
	private float m_Speed = 1.0f;

	// Use this for initialization
	public override void Start(){
		m_Speed = m_Controller.m_MouvementSpeed;
	
	}
	
	// Update is called once per frame
	public override void UpdateBehavior() {
		
		m_Controller.transform.Translate (Vector3.left * m_Speed * Time.deltaTime, Space.World);
	
	}
}
