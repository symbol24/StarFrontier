using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {
	public int m_UnlockCannonID = 0;
	private float m_Speed = 0.5f;	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.down * m_Speed * Time.deltaTime, Space.World);
	}

	void OnTriggerEnter2D(Collider2D col){
		PlayerController player = col.gameObject.GetComponent<PlayerController>();
		ShieldController shield = col.gameObject.GetComponent<ShieldController>();
		if(player != null || shield != null){
			player = GameObject.FindObjectOfType(typeof(PlayerController)) as PlayerController;
			player.ActivateCannon(m_UnlockCannonID);
		}
		Destroy (gameObject);
	}
}
