using UnityEngine;
using System.Collections;

public class EAIBehaviors : MonoBehaviour{
	protected enemy_controller m_Controller;

	public void Init (enemy_controller controller) {
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
