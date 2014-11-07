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
