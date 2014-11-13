using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour {
	public string m_Owner = "player";
	public int m_ProjectileEnergyValue = 0;
	private EnergySystemController m_EnergyBar;

	void Start(){
		m_EnergyBar = GameObject.FindObjectOfType(typeof(EnergySystemController)) as EnergySystemController;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		
		ProjectileController tempBullet = coll.gameObject.GetComponent<ProjectileController>();
		if (tempBullet!= null && tempBullet.m_Target == m_Owner) {
			m_ProjectileEnergyValue = tempBullet.m_EnergyValue;
			tempBullet.pushBullet(tempBullet);
			m_EnergyBar.ChangeEnergyTotal("add", m_ProjectileEnergyValue);
		}
	}
}
