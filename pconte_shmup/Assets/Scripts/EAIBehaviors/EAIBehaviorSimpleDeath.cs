using UnityEngine;
using System.Collections;

public class EAIBehaviorSimpleDeath : EAIBehaviors {
	private string m_DeathType = "simple";
	public string m_Type = "death";

	// Use this for initialization
	public override void Start () {
		m_BehaviorName = m_Type;
	}
	
	// Update is called once per frame
	public override void UpdateBehavior () {
	
	}
}
