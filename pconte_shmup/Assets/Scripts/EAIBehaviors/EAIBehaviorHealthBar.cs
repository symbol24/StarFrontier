using UnityEngine;
using System.Collections;

public class EAIBehaviorHealthBar : EAIBehaviors {
	public GameObject m_HealthBar;
	private float m_originalScaleX = 0.0f;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		m_HealthBar = m_Controller.m_HealthBar;
		m_originalScaleX = m_HealthBar.transform.localScale.x;
	}
	
	// Update is called once per frame
	public override void Update () {
		
	}
	
	
	public override void UpdateBehavior() {
		float currentHPFloat = m_Controller.m_CurrentHP;
		float startingHPFloat = m_Controller.m_EaiHP;
		float healthPercent = (currentHPFloat / startingHPFloat) * m_originalScaleX;
		Vector3 newScale = new Vector3 (healthPercent, m_HealthBar.transform.localScale.y, m_HealthBar.transform.localScale.z);
		if(m_HealthBar != null){
			m_HealthBar.transform.localScale = newScale;
		}
	}
}
