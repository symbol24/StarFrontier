using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour{

	public void Init (enemy_controller controller) {
		m_Controller = controller;
	}

	public void Update() {

	}

	public bool test;
	protected enemy_controller m_Controller;
}
