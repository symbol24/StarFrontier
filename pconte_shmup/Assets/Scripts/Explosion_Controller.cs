using UnityEngine;
using System.Collections;

public class Explosion_Controller : MonoBehaviour {
	public float lifeTimer;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, lifeTimer);
	}

}
