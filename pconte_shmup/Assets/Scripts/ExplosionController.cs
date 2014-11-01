using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {
	public float lifeTimer;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, lifeTimer);
	}

}
