using UnityEngine;
using System.Collections;

public class EAIBehaviorSimpleDeath : EAIBehaviors {
	private string m_DeathType = "simple";
	public string m_Type = "death";
	
	public override void Init(EnemyController controller){
		base.Init (controller);
		m_BehaviorName = m_Type;
		m_BehaviorDeathType = m_DeathType;
	}

	// Update is called once per frame
	public override void UpdateBehavior () {
	
	}
}
