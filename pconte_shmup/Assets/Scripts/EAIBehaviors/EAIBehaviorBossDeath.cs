using UnityEngine;
using System.Collections;

public class EAIBehaviorBossDeath : EAIBehaviors {
	private string m_DeathType = "boss";
	public string m_Type = "death";
	
	// Use this for initialization
	public override void Start () {
		base.Start ();
		m_BehaviorName = m_Type;
	}
	
	// Update is called once per frame
	public override void UpdateBehavior () {
		
	}
}
