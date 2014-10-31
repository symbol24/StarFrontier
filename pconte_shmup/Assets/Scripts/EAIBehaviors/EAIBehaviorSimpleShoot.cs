using UnityEngine;
using System.Collections;

public class EAIBehaviorSimpleShoot : EAIBehaviors {
	public float minFireRate = 1.0f;
	public float maxFireRate = 2.0f;
	public float nextFire = 0.0F;
	public ProjectileController tempBullet;
	public float offset;

	// Use this for initialization
	public override void Start(){		
		base.Start ();
		nextFire = Time.time + minFireRate;
	}

	// Update is called once per frame
	public override void UpdateBehavior() {
		if (Time.time > nextFire){
			nextFire = Time.time + Random.Range(minFireRate, maxFireRate);
			tempBullet = m_Controller.gameMgr.bulletsEAI.Pop();
			tempBullet.transform.position = new Vector2(m_Controller.transform.position.x, m_Controller.transform.position.y - offset);
			tempBullet.gameObject.SetActive(true);
		}
	}
}
