using UnityEngine;
using System.Collections;

public class EAIBehaviorTankMiniBoss : EAIBehaviors {
	private enum MiniBossSate
		{
		arriving,
		settle,
		firstwave,
		threequaters,
		half,
		onequarter,
		berserk
		}

	private MiniBossSate m_CurrentState;

	//three sets of cannons
	private GameObject[] m_CannonSet1 = new GameObject[2];
	private GameObject[] m_CannonSet2 = new GameObject[4];
	private GameObject[] m_CannonSet3 = new GameObject[2];

	//three bullets
	private ProjectileController[] m_Bullets;
	
	//offscreen spawn point and in screen arrival point
	public Vector3 m_StartingPoint;
	public Vector3 m_SettlePoint;

	//arriving speed
	public float m_ArrivingSpeed = 1.0f;

	//first wave - only first set of weapons are active
	public float m_FirstWaveMouvementSpeed = 1.0f;
	public float m_FirstWaveShotDelay = 0.3f;
	public float m_FirstWaveGroupingDelay = 2.0f;
	public int m_FirstWaveBulletUsedID = 0;

	//three wuater health - 2 sets of cannons in use - first wave used for first weapons as well
	public float m_ThreeQuartersMouvementSpeed = 1.0f;
	public float m_ThreeQuartersShotDelay = 0.3f;
	public float m_ThreeQuartersGroupingDelay = 2.0f;
	public int m_ThreeQuartersUsedID = 1;

	//half health - third weapon is now active
	public float m_HalfMouvementSpeed = 1.0f;
	public float m_HalfShotDelay = 0.3f;
	public float m_HalfGroupingDelay = 2.0f;
	public int m_HalfUsedID = 2;


	//one quarter health - all weapons converge for a large single shot all together
	public float m_OneQuarterMouvementSpeed = 1.0f;
	public float m_OneQuarterShotDelay = 1.0f;
	public int m_OneQuarterUsedID = 2;

	//berserk mode 10% health - weapons are now berserk and send bullets everywhere
	public float m_BerserkMouvementSpeed = 1.0f;
	public float m_BerserShotDelay = 1.0f;
	public float m_StartRotation = 0.0f;
	public float m_EndRotation = 20.0f;
	private float[] m_CurrentRotationDifference;
	public float m_RotationTime = 0.5f;
	public float m_TimeUntilNextRotation = 0.0f;


	private float m_NextShot = 0.0f;
	private float m_NextGroupingShot = 0.0f;
	private float m_CurrentShotDelay = 1.0f;

	// Use this for initialization
	public override void Start(){
		//setting the cannons in the proper sets
		int y = 0;
		for (int i = 0; i < m_Controller.m_CannonReferances.Length; i++) {
			if(i < 2){
				m_CannonSet1[y] = m_Controller.m_CannonReferances[i];
				y++;
				if(y == 2) y = 0;
			}else if(i < 6){
				m_CannonSet2[y] = m_Controller.m_CannonReferances[i];
				y++;
				if(y == 4) y = 0;
			}else{
				m_CannonSet3[y] = m_Controller.m_CannonReferances[i];
				y++;
			}
		}
		
		m_NextShot = Time.time;
		m_NextGroupingShot = Time.time;

		//setting at spawn point
		m_Controller.transform.position = m_StartingPoint;



		//for berserk waves
		m_CurrentRotationDifference = new float[m_Controller.m_CannonReferances.Length];
		//StartCoroutine ("BerserkRotateForth");

		m_CurrentState = MiniBossSate.arriving;
	}
	
	// Update is called once per frame
	public override void UpdateBehavior() {
		switch (m_CurrentState) {
		case MiniBossSate.arriving:
			break;
		case MiniBossSate.settle:
			break;
		case MiniBossSate.firstwave:
			break;
		case MiniBossSate.threequaters:
			break;
		case MiniBossSate.half:
			break;
		case MiniBossSate.onequarter:
			break;
		case MiniBossSate.berserk:
			break;

		}
	}



	private IEnumerator BerserkRotateForth(){
		
		float myTime = 0.0f;
		
		while(myTime < m_RotationTime){
			foreach(GameObject cRef in m_Controller.m_CannonReferances){
				cRef.transform.RotateAround(cRef.transform.position, cRef.transform.forward, Time.deltaTime*(m_EndRotation - m_StartRotation));
			}
			myTime += Time.deltaTime;
			yield return null;
		}
		
		StartCoroutine ("BerserkRotateBack");
		
	}
	
	private IEnumerator BerserkRotateBack(){
		
		float myTime = 0.0f;
		
		while(myTime < m_RotationTime){
			foreach(GameObject cRef in m_Controller.m_CannonReferances){
				cRef.transform.RotateAround(cRef.transform.position, cRef.transform.forward, -Time.deltaTime*(m_EndRotation - m_StartRotation));
			}
			myTime += Time.deltaTime;
			yield return null;
		}
		
		StartCoroutine ("BerserkRotateForth");
	}
}
