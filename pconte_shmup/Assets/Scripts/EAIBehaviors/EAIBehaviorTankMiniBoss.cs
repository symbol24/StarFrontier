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
	public GameObject[] m_CannonSet1;
	public GameObject[] m_CannonSet2;
	public GameObject[] m_CannonSet3;

	//three bullets
	public ProjectileController[] m_Bullets;
	
	//offscreen spawn point and in screen arrival point
	public Vector3 m_StartingPoint;
	public Vector3 m_SettlePoint;

	//arriving speed
	public float m_ArrivingSpeed = 1.0f;

	//first wave
	public float m_FirstWaveSpeed = 1.0f;

	//three wuater health
	public float m_ThreeQuartersSpeed = 1.0f;

	//half health
	public float m_HalfSpeed = 1.0f;

	//one quarter health
	public float m_OneQuarterSpeed = 1.0f;

	//berserk mode 10% health
	public float m_BerserkSpeed = 1.0f;


	private float m_CurrentFireRate = 1.0f;

	// Use this for initialization
	public override void Start(){	
	
	}
	
	// Update is called once per frame
	public override void UpdateBehavior() {
	
	}
}
