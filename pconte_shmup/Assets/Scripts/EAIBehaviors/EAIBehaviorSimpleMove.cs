using UnityEngine;
using System.Collections;

public class EAIBehaviorSimpleMove : EAIBehaviors {
	public float speed = 1;

	// Use this for initialization
	public override void Start(){
		base.Start ();
	}
	
	// Update is called once per frame
	public override void UpdateBehavior() {
		
		m_Controller.transform.Translate (Vector3.down * speed * Time.deltaTime, Space.World);
	}
}
