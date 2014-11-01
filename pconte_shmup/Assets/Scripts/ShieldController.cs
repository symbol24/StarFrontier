using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour {
	public string m_Owner = "player";

	void OnTriggerEnter2D(Collider2D coll) {
		
		ProjectileController tempBullet = coll.gameObject.GetComponent<ProjectileController>();
		if (tempBullet!= null && tempBullet.m_Target == m_Owner) {
			tempBullet.pushBullet(tempBullet);
		}
	}
}
