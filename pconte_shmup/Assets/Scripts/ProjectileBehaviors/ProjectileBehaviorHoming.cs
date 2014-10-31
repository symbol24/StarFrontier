using UnityEngine;
using System.Collections;

public class ProjectileBehaviorHoming : ProjectileBehavior {

	private enemy_controller m_HomingTargetOne;
	public float m_Speed = 5.0f;
	public string type = "missile";

	// Use this for initialization
	public override void Start () {
		Object[] allEAI = GameObject.FindObjectsOfType (typeof(enemy_controller));
		foreach (enemy_controller ec in allEAI) {
			if(ec != null){
				float distance = (m_Controller.transform.position - ec.transform.position).sqrMagnitude;
				if(m_HomingTargetOne != null){
					if(ec.transform.position.y > m_Controller.transform.position.y && distance > 0 && ec.transform.position.sqrMagnitude < m_HomingTargetOne.transform.position.sqrMagnitude){
						m_HomingTargetOne = ec;
					}
				}else{
					m_HomingTargetOne = ec;
				}
			}
		}
	}
	
	// Update is called once per frame
	public override void Update () {
		
	}
	
	public override void UpdateBehavior (){
		if (m_HomingTargetOne != null) {
			Vector3 tempV = (m_HomingTargetOne.transform.position - m_Controller.transform.position).normalized;
			float angle = Mathf.Atan2(tempV.y, tempV.x)*Mathf.Rad2Deg;
			Quaternion rotQ = new Quaternion ();
			rotQ.eulerAngles = new Vector3(0,0,angle-90);
			m_Controller.transform.rotation = rotQ;
		}

		m_Controller.transform.Translate (m_Controller.transform.up * m_Speed * Time.deltaTime, Space.World);
	}
}
