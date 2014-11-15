using UnityEngine;
using System.Collections;

public class EAIBehaviorStopSpawners : EAIBehaviors {

	public override void Init(EnemyController controller){
		base.Init (controller);
		EnemySpawnController[] allSpawnControllers = GameObject.FindObjectsOfType(typeof(EnemySpawnController)) as EnemySpawnController[];
		foreach (EnemySpawnController thisSpwnController in allSpawnControllers) {
			if(thisSpwnController != null){
				thisSpwnController.StopSpawners();
			}
		}
	}
}
