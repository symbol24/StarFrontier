using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EAIBehaviorTankBoss : EAIBehaviors {

	//oooh states!
	private enum BossState
		{
		arriving,
		settle,
		firstwave,
		threequaters,
		half,
		onequarter,
		berserk,
		dying
		}

	private BossState m_CurrentState;
	private BossState m_PreviousState;

	//three sets of cannons
	private CannonReferences[] m_CannonList1;
	private CannonReferences[] m_CannonList2;
	private CannonReferences[] m_CannonList3;

	//original rotation points of cannons
	private Transform[] m_CannonTransforms1;
	private Transform[] m_CannonTransforms2;
	private Transform[] m_CannonTransforms3;

	//three bullets
	private ProjectileController[] m_Bullets;
	
	//offscreen spawn point and in screen arrival point
	public Vector3 m_StartingPoint;
	public Vector3 m_SettlePoint;

	//arriving speed
	private float m_ArrivingSpeed = 1.0f;

	//settle time to build tension
	private float m_SettleDelay = 1.5f;
	private float m_SettleTime = 0.0f;
	private float m_SettleSpeed = 2.0f;
	private bool m_SettleDelaySet = false;

	//first wave - only first set of weapons are active
	private float m_FirstWaveShotDelay = 0.1f;
	private float m_FirstWaveGroupingDelay = 2.0f;
	private int m_FirstWaveBulletUsedID = 0;
	private int m_FirstWaveNumberOfGroupedShots = 5;

	//three wuater health - 2 sets of cannons in use - first wave used for first weapons as well
	private float m_ThreeQuartersShotDelay = 0.2f;
	private float m_ThreeQuartersGroupingDelay = 2.0f;
	private int m_ThreeQuartersUsedID = 1;
	private int m_ThreeQaurtersNumberOfGroupedShots = 4;
	private float m_ThreeQaurtersSinAmplitude = 1.0f;
	private float m_ThreeQaurtersSinFrequency = 0.2f;

	//sin mouvement
	private float m_SinHorizontalOffset = 0.0f;
	private float m_SinTime = 0.0f;

	//half health - third weapon is now active
	private float m_HalfMouvementSpeed = 1.0f;
	private float m_HalfShotDelay = 0.3f;
	private float m_HalfGroupingDelay = 0.5f;
	private int m_HalfUsedID = 2;
	private int m_HalfNumberOfGroupedShots = 3;
	public Vector3[] m_HalfPoints;
	private int m_HalfPointsID = 0;


	//one quarter health - all weapons converge for a large single shot all together
	private float m_OneQuarterMouvementSpeed = 1.0f;
	private float m_OneQuarterShotDelay = 0.1f;
	private float m_OneQuarterGroupingDelay = 0.5f;
	private int m_OneQuarterUsedID = 2;
	private int m_OneQuarterNumberOfGroupedShots = 6;
	public Vector3[] m_OneQuarterPoints;
	private int m_OneQuarterPointsID = 0;

	//berserk mode 10% health - weapons are now berserk and send bullets everywhere
	private float m_BerserkMouvementSpeed = 3.0f;
	private float m_BerserkShotDelay = 0.1f;
	private float m_BerserkGroupingDelay = 0.3f;
	private int m_BerserkUsedID = 1;
	private int m_BerserkNumberOfGroupedShots = 10;
	public Vector3[] m_BerserkPoints;
	private int m_BerserkPointsID = 0;

	//time management
	private float m_Set1NextShot = 0.0f;
	private float m_Set1NextGroupingShot = 0.0f;
	private float m_Set2NextShot = 0.0f;
	private float m_Set2NextGroupingShot = 0.0f;
	private float m_Set3NextShot = 0.0f;
	private float m_Set3NextGroupingShot = 0.0f;
	private int m_Group1ShotCount = 0;
	private int m_Group2ShotCount = 0;
	private int m_Group3ShotCount = 0;

	//stuff!
	private PolygonCollider2D m_polyCollider;


	// Use this for initialization
	public override void Start(){
		m_polyCollider = m_Controller.GetComponent<PolygonCollider2D> ();
		if (m_polyCollider == null)	print ("No COLLIDER ERHMAHGERD!");
		//disable collision
		else m_polyCollider.enabled = false;
		
		//setting the cannons in the proper sets
		SetupCannons();

		//get bullets
		m_Bullets = m_Controller.m_ListOfProjectilesToShoot;

		//set timers to nao
		m_SettleTime = Time.time;
		m_Set1NextShot = Time.time;
		m_Set1NextGroupingShot = Time.time;
		m_Set2NextShot = Time.time;
		m_Set2NextGroupingShot = Time.time;
		m_Set3NextShot = Time.time;
		m_Set3NextGroupingShot = Time.time;

		//setting at spawn point
		m_Controller.transform.position = m_StartingPoint;

		//first state to arriving!
		m_CurrentState = BossState.arriving;
	}
	
	// Update is called once per frame
	public override void UpdateBehavior() {
		bool reached = false;

		switch (m_CurrentState) {
		case BossState.arriving:
			m_Controller.transform.Translate(Vector3.down * m_ArrivingSpeed * Time.deltaTime, Space.World);
			if(m_Controller.transform.position.y <= m_SettlePoint.y) {
				m_polyCollider.enabled = false;
				m_CurrentState = BossState.settle;
				m_PreviousState = BossState.arriving;
			}
			break;

		case BossState.settle:
			
			bool settled = MoveToPoint(m_SettlePoint, m_SettleSpeed);

			if(settled && !m_SettleDelaySet){
				m_SettleTime = Time.time + m_SettleDelay;
				m_SettleDelaySet = true;
			}

			if(settled && m_SettleDelaySet){
				if(Time.time >= m_SettleTime){
					if( m_PreviousState == BossState.arriving){
						m_CurrentState = BossState.firstwave;
						m_polyCollider.enabled = true;
					}else if(m_PreviousState == BossState.firstwave){
						m_CurrentState = BossState.threequaters;
						m_polyCollider.enabled = true;
					}else if(m_PreviousState == BossState.threequaters){
						m_CurrentState = BossState.half;
						m_polyCollider.enabled = true;
					}else if(m_PreviousState == BossState.half){
						m_CurrentState = BossState.onequarter;
						m_polyCollider.enabled = true;
					}else if(m_PreviousState == BossState.onequarter){
						m_CurrentState = BossState.berserk;
						m_polyCollider.enabled = true;
					}
					m_SettleDelaySet = false;
				}
			}

			break;

		case BossState.firstwave:
			ShootFromTheseCannons(1, m_CannonList1, m_Bullets[m_FirstWaveBulletUsedID], m_FirstWaveShotDelay, m_FirstWaveGroupingDelay, m_FirstWaveNumberOfGroupedShots);
			if(m_Controller.m_CurrentHP <= (m_Controller.m_EaiHP*0.75f)) {
				m_CurrentState = BossState.settle;
				m_PreviousState = BossState.firstwave;
				m_polyCollider.enabled = false;
			}
			break;

		case BossState.threequaters:
			ShootFromTheseCannons(1, m_CannonList1, m_Bullets[m_FirstWaveBulletUsedID], m_FirstWaveShotDelay, m_FirstWaveGroupingDelay, m_FirstWaveNumberOfGroupedShots);
			ShootFromTheseCannons(2, m_CannonList2, m_Bullets[m_ThreeQuartersUsedID], m_ThreeQuartersShotDelay, m_ThreeQuartersGroupingDelay, m_ThreeQaurtersNumberOfGroupedShots);
			m_SinHorizontalOffset = SinWaveMotion(m_ThreeQaurtersSinFrequency, m_ThreeQaurtersSinAmplitude, m_SinHorizontalOffset, m_Controller.transform.right);
			if(m_Controller.m_CurrentHP <= (m_Controller.m_EaiHP*0.50f)) {
				m_CurrentState = BossState.settle;
				m_PreviousState = BossState.threequaters;
				m_polyCollider.enabled = false;
				m_SinHorizontalOffset = 0.0f;
			}
			break;

		case BossState.half:
			reached = MoveToPoint(m_HalfPoints[m_HalfPointsID], m_HalfMouvementSpeed);

			ShootFromTheseCannons(1, m_CannonList1, m_Bullets[m_FirstWaveBulletUsedID], m_FirstWaveShotDelay, m_FirstWaveGroupingDelay, m_FirstWaveNumberOfGroupedShots);
			ShootFromTheseCannons(2, m_CannonList2, m_Bullets[m_ThreeQuartersUsedID], m_ThreeQuartersShotDelay, m_ThreeQuartersGroupingDelay, m_ThreeQaurtersNumberOfGroupedShots);
			ShootFromTheseCannons(3, m_CannonList2, m_Bullets[m_HalfUsedID], m_HalfShotDelay, m_HalfGroupingDelay, m_HalfNumberOfGroupedShots);

			if(reached) m_HalfPointsID++;
			if(reached && m_HalfPointsID > 2) m_HalfPointsID = 0;

			if(m_Controller.m_CurrentHP <= (m_Controller.m_EaiHP*0.25f)) {
				m_CurrentState = BossState.settle;
				m_PreviousState = BossState.half;
				m_polyCollider.enabled = false;
			}
			break;

		case BossState.onequarter:
			reached = MoveToPoint(m_OneQuarterPoints[m_OneQuarterPointsID], m_OneQuarterMouvementSpeed);

			ShootFromTheseCannons(1, m_CannonList1, m_Bullets[m_OneQuarterUsedID], m_ThreeQuartersShotDelay, m_ThreeQuartersGroupingDelay, m_ThreeQaurtersNumberOfGroupedShots);
			ShootFromTheseCannons(2, m_CannonList2, m_Bullets[m_OneQuarterUsedID], m_HalfShotDelay, m_HalfGroupingDelay, m_HalfNumberOfGroupedShots);
			ShootFromTheseCannons(3, m_CannonList2, m_Bullets[m_OneQuarterUsedID], m_OneQuarterShotDelay, m_OneQuarterGroupingDelay, m_OneQuarterNumberOfGroupedShots);

			if(reached) m_OneQuarterPointsID++;
			if(reached && m_OneQuarterPointsID > 3) m_OneQuarterPointsID = 0;

			if(m_Controller.m_CurrentHP <= (m_Controller.m_EaiHP*0.10f)) {
				m_CurrentState = BossState.settle;
				m_PreviousState = BossState.onequarter;
				m_polyCollider.enabled = false;
			}
			break;

		case BossState.berserk:
			reached = MoveToPoint(m_BerserkPoints[m_BerserkPointsID], m_BerserkMouvementSpeed);

			ShootFromTheseCannons(1, m_CannonList1, m_Bullets[m_BerserkUsedID], m_HalfShotDelay, m_HalfGroupingDelay, m_HalfNumberOfGroupedShots);
			ShootFromTheseCannons(2, m_CannonList2, m_Bullets[m_BerserkUsedID], m_OneQuarterShotDelay, m_OneQuarterGroupingDelay, m_OneQuarterNumberOfGroupedShots);
			ShootFromTheseCannons(3, m_CannonList2, m_Bullets[m_BerserkUsedID], m_BerserkShotDelay, m_BerserkGroupingDelay, m_BerserkNumberOfGroupedShots);

			if(reached) m_BerserkPointsID++;
			if(reached && m_BerserkPointsID > 3) m_BerserkPointsID = 0;

			break;
		
		case BossState.dying:

			break; 
		}
	}

	private bool MoveToPoint (Vector3 targetPoint, float speed){
		bool positionReached = false;
		Vector3 currentPosition = m_Controller.transform.position;
		//move to target point
		if(Vector3.Distance(currentPosition, targetPoint) > 0.1f){
			Vector3 directionOfTravel = targetPoint - currentPosition;
			directionOfTravel.Normalize();
			m_Controller.transform.Translate((directionOfTravel.x * speed * Time.deltaTime),
			                                 (directionOfTravel.y * speed * Time.deltaTime),
			                                 (directionOfTravel.z * speed * Time.deltaTime),
			                                 Space.World);
		}else{
			m_Controller.transform.position = Vector3.MoveTowards(currentPosition, targetPoint, speed * Time.deltaTime);
			positionReached = true;
		}
		
		return positionReached;
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
		
		m_CannonTransforms1 = new Transform[set1];
		m_CannonTransforms2 = new Transform[set2];
		m_CannonTransforms3 = new Transform[set3];
		
		set1 = 0;
		set2 = 0;
		set3 = 0;
		
		foreach(CannonReferences cref in cannons){
			if(cref.m_name == "Set1"){
				m_CannonList1[set1] = cref;
				m_CannonTransforms1[set1] = cref.transform;
				set1++;
			}else if(cref.m_name == "Set2"){
				m_CannonList2[set2] = cref;
				m_CannonTransforms2[set2] = cref.transform;
				set2++;
			}else{
				m_CannonList3[set3] = cref;
				m_CannonTransforms3[set3] = cref.transform;
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
				Stack<ProjectileController> StackToUpdate = EntitiesCreator.GetStackToUpdate(bullets, m_Controller.m_GameMgr);
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

	private float SinWaveMotion(float sinFrequency, float sinAmplitude, float offset, Vector3 direction){
		m_SinTime += Time.deltaTime;
		
		//remove offset
		m_Controller.transform.position -= offset * direction;
		
		//adjust horizontally
		offset = Mathf.Sin (m_SinTime * sinFrequency * 2 * Mathf.PI) * sinAmplitude;
		
		m_Controller.transform.position += offset * direction;

		return offset;
	}

	private IEnumerator DeathExplosions(int amountOfExplosions){
		Transform shipTransform = m_Controller.transform;
		GameObject explosion = m_Controller.m_BlueExplosion;
		float explosionAnimationTime = explosion.animation.clip.length;
		Vector3 mins = m_Controller.renderer.bounds.min;
		Vector3 maxs = m_Controller.renderer.bounds.max;
		for(int i = 0; i < amountOfExplosions; i++){
			float xOffset = (Random.Range(mins.x, maxs.x));
			float yOffset = (Random.Range(mins.y, maxs.y));
			Vector3 newPos = shipTransform.transform.position + new Vector3(xOffset, yOffset, 0);			               
			explosion = Instantiate (explosion, newPos, shipTransform.transform.rotation) as GameObject;
			Destroy(explosion, explosionAnimationTime);
			yield return new WaitForSeconds(explosionAnimationTime);
		}
		yield return null;
	}
}
