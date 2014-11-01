﻿using UnityEngine;
using System.Collections;

public class ProjectileBehaviorSimpleMoveUp : ProjectileBehavior {
	public float m_BulletSpeed = 5.0f;

	// Use this for initialization
	public override void Start () {
	
	}
	
	// Update is called once per frame
	public override void Update () {
	
	}

	public override void UpdateBehavior (){
		base.UpdateBehavior ();
		m_Controller.transform.Translate (Vector2.up * m_BulletSpeed * Time.deltaTime, Space.Self);
	}
}