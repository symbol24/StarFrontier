using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	private CannonReferences[] m_CannonList1;
	private CannonReferences[] m_CannonList2;
	private CannonReferences[] m_CannonList3;

	//three bullets
	private ProjectileController[] m_Bullets;
	
	//offscreen spawn point and in screen arrival point
	public Vector3 m_StartingPoint;
	public Vector3 m_SettlePoint;

	//arriving speed
	private float m_ArrivingSpeed = 1.0f;

	//settle time to build tension
	private float m_SettleTime = 2.0f;

	//first wave - only first set of weapons are active
	public float m_FirstWaveMouvementSpeed = 1.0f;
	public float m_FirstWaveShotDelay = 0.3f;
	public float m_FirstWaveGroupingDelay = 2.0f;
	public int m_FirstWaveBulletUsedID = 0;
	public int m_FirstWaveNumberOfGroupedShots = 3;

	//three wuater health - 2 sets of cannons in use - first wave used for first weapons as well
	public float m_ThreeQuartersMouvementSpeed = 1.0f;
	public float m_ThreeQuartersShotDelay = 0.3f;
	public float m_ThreeQuartersGroupingDelay = 2.0f;
	public int m_ThreeQuartersUsedID = 1;
	public int m_ThreeQaurtersNumberOfGroupedShots = 3;
	public float m_SinSpeed = 1.0f;
	public float m_SinAmplitude = 1.0f;
	public float m_SinFrequency = 1.0f;
	private float m_SinHorizontalOffset = 0.0f;
	private float m_SinTime = 0.0f;

	//half health - third weapon is now active
	public float m_HalfMouvementSpeed = 1.0f;
	public float m_HalfShotDelay = 0.3f;
	public float m_HalfGroupingDelay = 2.0f;
	public int m_HalfUsedID = 2;
	public int m_HalfNumberOfGroupedShots = 3;


	//one quarter health - all weapons converge for a large single shot all together
	public float m_OneQuarterMouvementSpeed = 1.0f;
	public float m_OneQuarterShotDelay = 1.0f;
	public int m_OneQuarterUsedID = 2;
	public int m_OneQuarterNumberOfGroupedShots = 3;

	//berserk mode 10% health - weapons are now berserk and send bullets everywhere
	public float m_BerserkMouvementSpeed = 1.0f;
	public float m_BerserShotDelay = 1.0f;
	public float m_StartRotation = 0.0f;
	public float m_EndRotation = 20.0f;
	private float[] m_CurrentRotationDifference;
	public float m_RotationTime = 0.5f;
	public float m_TimeUntilNextRotation = 0.0f;

	//time management
	private float m_Set1NextShot = 0.0f;
	private float m_Set1NextGroupingShot = 0.0f;
	private float m_Set2NextShot = 0.0f;
	private float m_Set2NextGroupingShot = 0.0f;
	private float m_Set3NextShot = 0.0f;
	private float m_Set3NextGroupingShot = 0.0f;
	private float m_Timer = 0.0f;
	private int m_Group1ShotCount = 0;
	private int m_Group2ShotCount = 0;
	private int m_Group3ShotCount = 0;

	//stuff!
	private PolygonCollider2D m_polyCollider;


	// Use this for initialization
	public override void Start(){
		m_polyCollider = m_Controller.GetComponent<PolygonCollider2D> ();
		if (m_polyCollider == null)	print ("No COLLIDER ERHMAHGERD!");
		
		//setting the cannons in the proper sets
		SetupCannons();

		m_Bullets = m_Controller.m_ListOfProjectilesToShoot;
		
		m_Set1NextShot = Time.time;
		m_Set1NextGroupingShot = Time.time;
		m_Set2NextShot = Time.time;
		m_Set2NextGroupingShot = Time.time;
		m_Set3NextShot = Time.time;
		m_Set3NextGroupingShot = Time.time;

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
			m_Controller.transform.Translate(Vector3.down * m_ArrivingSpeed * Time.deltaTime, Space.World);
			if(m_Controller.transform.position.y <= m_SettlePoint.y) {
				m_polyCollider.enabled = true;
				m_CurrentState = MiniBossSate.settle;
				m_Timer = Time.time + m_SettleTime;
			}
			break;

		case MiniBossSate.settle:
			if(Time.time >= m_Timer) m_CurrentState = MiniBossSate.firstwave;
			break;

		case MiniBossSate.firstwave:
			ShootFromTheseCannons(1, m_CannonList1, m_Bullets[m_FirstWaveBulletUsedID], m_FirstWaveShotDelay, m_FirstWaveGroupingDelay, m_FirstWaveNumberOfGroupedShots);
			if(m_Controller.m_currentHP <= (m_Controller.eaiHP*0.75)) {
				m_CurrentState = MiniBossSate.threequaters;
			}
			break;

		case MiniBossSate.threequaters:
			ShootFromTheseCannons(1, m_CannonList1, m_Bullets[m_FirstWaveBulletUsedID], m_FirstWaveShotDelay, m_FirstWaveGroupingDelay, m_FirstWaveNumberOfGroupedShots);
			ShootFromTheseCannons(2, m_CannonList2, m_Bullets[m_ThreeQuartersUsedID], m_ThreeQuartersShotDelay, m_ThreeQuartersGroupingDelay, m_ThreeQaurtersNumberOfGroupedShots);
			SinWaveMotion();
			break;

		case MiniBossSate.half:
			break;

		case MiniBossSate.onequarter:
			break;

		case MiniBossSate.berserk:
			break;

		}
	}

	private void SetupCannons ()
	{
		CannonReferences[] cannons = FindObjectsOfType (typeof(CannonReferences)) as CannonReferences[];
		int set1 = 0;
		int set2 = 0;
		int set3 = 0;
		
		foreach(CannonReferences cref in cannons){
			if(cref.m_name == "Set1"){
				set1++;
			}else if(cref.m_name == "Set2"){
				set2++;
			}else{
				set3++;
			}
		}
		
		m_CannonList1 = new CannonReferences[set1];
		m_CannonList2 = new CannonReferences[set2];
		m_CannonList3 = new CannonReferences[set3];
		
		set1 = 0;
		set2 = 0;
		set3 = 0;
		
		foreach(CannonReferences cref in cannons){
			if(cref.m_name == "Set1"){
				m_CannonList1[set1] = cref;
				set1++;
			}else if(cref.m_name == "Set2"){
				m_CannonList2[set2] = cref;
				set2++;
			}else{
				m_CannonList3[set3] = cref;
				set3++;
			}
		}
	}

	private void ShootFromTheseCannons(int groupID, CannonReferences[] theseCannons, ProjectileController bullets, float ShotDelay, float groupDelay, int groupingCount){
		float nextShotTimer = 0.0f;
		float groupShotTimer = 0.0f;
		int currentGroupingCount = 0;

		if(groupID == 1){
			nextShotTimer = m_Set1NextShot;
			groupShotTimer = m_Set1NextGroupingShot;
			currentGroupingCount = m_Group1ShotCount;
		}else if(groupID == 2){
			nextShotTimer = m_Set2NextShot;
			groupShotTimer = m_Set2NextGroupingShot;
			currentGroupingCount = m_Group2ShotCount;
		}else{
			nextShotTimer = m_Set3NextShot;
			groupShotTimer = m_Set3NextGroupingShot;
			currentGroupingCount = m_Group3ShotCount;
		}

		if (Time.time > groupShotTimer && Time.time > nextShotTimer){
			nextShotTimer = Time.time + ShotDelay;
			foreach(CannonReferences cRef in theseCannons){
				Stack<ProjectileController> StackToUpdate = EntitiesCreator.GetStackToUpdate(bullets, m_Controller.gameMgr);
				ProjectileController tempBullet = StackToUpdate.Pop();
				tempBullet.transform.position = new Vector2(cRef.transform.position.x, cRef.transform.position.y);
				tempBullet.transform.rotation = cRef.transform.rotation;
				tempBullet.gameObject.SetActive(true);
			}
			
			currentGroupingCount++;
			if(currentGroupingCount >= groupingCount){
				currentGroupingCount = 0;
				groupShotTimer = Time.time + groupDelay;
				if(groupID == 1){
					m_Set1NextGroupingShot = groupShotTimer;
				}else if(groupID == 2){
					m_Set2NextGroupingShot = groupShotTimer;
				}else{
					m_Set3NextGroupingShot = groupShotTimer;
				}

			}
			if(groupID == 1){
				m_Set1NextShot = nextShotTimer;
				m_Group1ShotCount = currentGroupingCount;
			}else if(groupID == 2){
				m_Set2NextShot = nextShotTimer;
				m_Group2ShotCount = currentGroupingCount;
			}else{
				m_Set3NextShot = nextShotTimer;
				m_Group3ShotCount = currentGroupingCount;
			}
		}

	}

	private void SinWaveMotion(){
		m_SinTime += Time.deltaTime;
		
		//remove offset
		m_Controller.transform.position -= m_SinHorizontalOffset * m_Controller.transform.right;
		
		//adjust horizontally
		m_SinHorizontalOffset = Mathf.Sin (m_SinTime * m_SinFrequency * 2 * Mathf.PI) * m_SinAmplitude;
		
		m_Controller.transform.position += m_SinHorizontalOffset * m_Controller.transform.right;
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
