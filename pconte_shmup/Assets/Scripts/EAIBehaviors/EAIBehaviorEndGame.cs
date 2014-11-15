using UnityEngine;
using System.Collections;

public class EAIBehaviorEndGame : EAIBehaviors {
	private string m_EndGameMessage = "Victoly!!";

	// Use this for initialization
	public override void Init (EnemyController controller) {
		base.Init (controller);
		m_Controller.m_GameMgr.SetGameOver(m_EndGameMessage);
	}
}
