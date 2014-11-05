using UnityEngine;
using System.Collections;

public class ProjectileBehanvoirPlayerBeam : ProjectileBehavior {
	public float m_VerticalOffset = 5.0f;
	private CannonController cannonFiring;


	// Use this for initialization
	public override void Start () {
		m_Controller.m_Type = "beam";
		cannonFiring = FindObjectOfType(typeof(CannonController)) as CannonController;
		m_Controller.transform.parent = cannonFiring.transform;
		Vector3 positionOffset = m_Controller.transform.position;
		positionOffset.y = positionOffset.y + m_VerticalOffset;
		m_Controller.transform.position = positionOffset;
	}
	
	// Update is called once per frame
	public override void Update () {
	
	}

	
	public override void UpdateBehavior (){
		Vector2 endOfLine = cannonFiring.transform.position;
		endOfLine.y += m_VerticalOffset;
		RaycastHit2D lineHit = Physics2D.Linecast (cannonFiring.transform.position, endOfLine);
		print (lineHit.distance.ToString());
	}
}
