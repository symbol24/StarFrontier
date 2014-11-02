﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EAIBehaviorShootAllTest : EAIBehaviors {
	public float m_MinFireRate = 1.0f;
	public float m_MaxFireRate = 2.0f;
	public float m_NextFire = 0.0F;
	private ProjectileController m_BulletToShoot;
	private ProjectileController tempBullet;
	
	// Use this for initialization
	public override void Start(){		
		base.Start ();
		m_NextFire = Time.time + m_MinFireRate;
		m_BulletToShoot = m_Controller.m_ProjectileToShoot;
	}
	
	// Update is called once per frame
	public override void UpdateBehavior() {
		if (Time.time > m_NextFire){
			m_NextFire = Time.time + m_MinFireRate;
			foreach(GameObject cRef in m_Controller.m_CannonReferances){
				Stack<ProjectileController> StackToUpdate = EntitiesCreator.GetStackToUpdate(m_BulletToShoot, m_Controller.gameMgr);
				tempBullet = StackToUpdate.Pop();
				tempBullet.transform.position = new Vector2(cRef.transform.position.x, cRef.transform.position.y);
				tempBullet.transform.rotation = cRef.transform.rotation;
				tempBullet.gameObject.SetActive(true);
			}
		}
	}
}