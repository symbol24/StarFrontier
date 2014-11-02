using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EAIBehaviorSimpleShoot : EAIBehaviors {
	public float m_MinFireRate = 1.0f;
	public float m_MaxFireRate = 2.0f;
	public float m_NextFire = 0.0F;
	private ProjectileController m_BulletToShoot;
	private ProjectileController tempBullet;
	public float m_Offset;

	// Use this for initialization
	public override void Start(){		
		base.Start ();
		m_NextFire = Time.time + m_MinFireRate;
		m_BulletToShoot = m_Controller.m_ProjectileToShoot;
	}

	// Update is called once per frame
	public override void UpdateBehavior() {
		if (Time.time > m_NextFire){
			m_NextFire = Time.time + Random.Range(m_MinFireRate, m_MaxFireRate);
			Stack<ProjectileController> StackToUpdate = EntitiesCreator.GetStackToUpdate(m_BulletToShoot, m_Controller.gameMgr);
			tempBullet = StackToUpdate.Pop();
			tempBullet.transform.position = new Vector2(m_Controller.transform.position.x, m_Controller.transform.position.y - m_Offset);
			tempBullet.gameObject.SetActive(true);
		}
	}
}
