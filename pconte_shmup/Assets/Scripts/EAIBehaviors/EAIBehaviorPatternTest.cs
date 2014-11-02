using UnityEngine;
using System.Collections;

public class EAIBehaviorPatternTest : EAIBehaviors {
	public float m_StartRotation = 0.0f;
	public float m_EndRotation = 20.0f;
	private float[] m_CurrentRotationDifference;
	public float m_RotationTime = 0.5f;
	public float m_TimeUntilNextRotation = 0.0f;

	// Use this for initialization
	public override void Start () {
		m_CurrentRotationDifference = new float[m_Controller.m_CannonReferances.Length];
		StartCoroutine ("RotateForth");
	}
	
	// Update is called once per frame
	public override void Update () {
	
	}

	public override void UpdateBehavior() {

	}

	private IEnumerator RotateForth(){

		float myTime = 0.0f;

		while(myTime < m_RotationTime){
			foreach(GameObject cRef in m_Controller.m_CannonReferances){
				cRef.transform.RotateAround(cRef.transform.position, cRef.transform.forward, Time.deltaTime*(m_EndRotation - m_StartRotation));
			}
			myTime += Time.deltaTime;
			yield return null;
		}

		StartCoroutine ("RotateBack");
//		for(int i = 0; i < m_Controller.m_CannonReferances.Length; i++){
//			m_CurrentRotationDifference[i] = m_Controller.m_CannonReferances[i].transform.rotation.z - m_EndRotation;
//
//			Vector3 tempV = new Vector3(0,0,0);
//			float angle = Mathf.Atan2(tempV.y, tempV.x)*Mathf.Rad2Deg;
//			Quaternion rotQ = new Quaternion ();
//			rotQ.eulerAngles = new Vector3(0,0,angle-90);
//			m_Controller.m_CannonReferances[i].transform.rotation = rotQ;
//		}

	}

	private IEnumerator RotateBack(){
		
		float myTime = 0.0f;
		
		while(myTime < m_RotationTime){
			foreach(GameObject cRef in m_Controller.m_CannonReferances){
				cRef.transform.RotateAround(cRef.transform.position, cRef.transform.forward, -Time.deltaTime*(m_EndRotation - m_StartRotation));
			}
			myTime += Time.deltaTime;
			yield return null;
		}
		
		StartCoroutine ("RotateForth");
	}

}
