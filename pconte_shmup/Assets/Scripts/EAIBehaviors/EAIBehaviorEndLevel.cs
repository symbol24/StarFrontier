using UnityEngine;
using System.Collections;

public class EAIBehaviorEndLevel : EAIBehaviors {
	
	// Update is called once per frame
	public override void Init (EnemyController controller) {
		base.Init (controller);
		m_Controller.m_GameMgr.ChangeLevel();
	}
}
