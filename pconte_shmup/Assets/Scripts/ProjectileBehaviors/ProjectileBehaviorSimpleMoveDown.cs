using UnityEngine;
using System.Collections;

public class ProjectileBehaviorSimpleMoveDown : ProjectileBehavior {
	public float m_BulletSpeed = 5.0f;

	// Use this for initialization
	public override void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {

	}

	public override void UpdateBehavior (){
		base.UpdateBehavior ();
	 	m_Controller.transform.Translate (Vector3.down * m_BulletSpeed * Time.deltaTime, Space.World);
	}
}
