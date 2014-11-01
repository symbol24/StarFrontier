using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour {
	protected ProjectileController m_Controller;
	public string m_Type = "bullet";
	
	public void Init (ProjectileController controller) {
		m_Controller = controller;
	}

	public virtual void Start(){
		transform.position = m_Controller.transform.position;
		
	}
	
	public virtual void Update(){
		
	}
	
	public virtual void UpdateBehavior(){

		}
}
