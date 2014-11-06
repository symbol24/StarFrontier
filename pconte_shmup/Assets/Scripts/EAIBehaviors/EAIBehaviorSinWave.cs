using UnityEngine;
using System.Collections;

public class EAIBehaviorSinWave : EAIBehaviors {
	public float m_Speed = 1.0f;
	public float m_SinAmplitude = 1.0f;
	public float m_SinFrequency = 1.0f;
	private float m_HorizontalOffset = 0.0f;
	private float m_SinTime = 0.0f;

	// Use this for initialization
	public override void Start(){
		m_Speed = m_Controller.m_MouvementSpeed;
	}
	
	// Update is called once per frame
	public override void UpdateBehavior() {
		m_SinTime += Time.deltaTime;

		//remove offset
		m_Controller.transform.position -= m_HorizontalOffset * m_Controller.transform.right;

		//move down
		m_Controller.transform.position += Vector3.down * m_Speed * Time.deltaTime;

		//adjust horizontally
		m_HorizontalOffset = Mathf.Sin (m_SinTime * m_SinFrequency * 2 * Mathf.PI) * m_SinAmplitude;

		m_Controller.transform.position += m_HorizontalOffset * m_Controller.transform.right;

	}
}
