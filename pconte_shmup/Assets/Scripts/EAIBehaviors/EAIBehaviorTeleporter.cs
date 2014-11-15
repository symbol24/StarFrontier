using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EAIBehaviorTeleporter : EAIBehaviors {
	//oooh states!
	private enum BossState
	{
		arriving,
		settle,
		normal,
		teleporting,
		dying
	}
	
	private BossState m_CurrentState;

	//arriving speed
	private float m_ArrivingSpeed = 1.0f;

	//offscreen spawn point and in screen arrival point
	public Vector3 m_StartingPoint;
	public Vector3 m_SettlePoint;

	//settle time to build tension
	private float m_SettleDelay = 1.5f;
	private float m_SettleTime = 0.0f;

	//normal shooting
	private float m_NormalShotDelay = 0.1f;
	private float m_NormalShotTimer = 0.0f;
	private float m_NormalGroupingDelay = 0.5f;
	private float m_NormalGroupingTimer = 0.0f;
	private int m_NormalGroupingAmount = 4;
	private int m_NormalGroupingCount = 0;

	//teleporting
	private bool m_IsTeleported = false;
	private float m_TeleportXLimiter = 2.5f;
	private float m_TeleportYLimiterUp = 3.5f;
	private float m_TeleportYLimiterDown = 0.0f;
	private float m_TeleportFadeTimer = 0.3f;

	//sin mouvement
	private float m_SinHorizontalOffset = 0.0f;
	private float m_SinTime = 0.0f;
	
	//dying sinwave
	private float m_DyingSinAmplitude = 0.1f;
	private float m_DyingSinFrequency = 10.0f;

	//bullet
	private ProjectileController m_BulletToShoot;
	private ProjectileController tempBullet;

	private PolygonCollider2D m_polyCollider;
	private SpriteRenderer m_ControllerSprite;

	public override void Init (EnemyController controller)
	{
		base.Init (controller);
		m_ControllerSprite = m_Controller.GetComponent<SpriteRenderer>();
		m_polyCollider = m_Controller.GetComponent<PolygonCollider2D> ();
		if (m_polyCollider == null)	print ("No COLLIDER ERHMAHGERD!");
		//disable collision
		else m_polyCollider.enabled = false;
		
		//setting at spawn point
		m_Controller.transform.position = m_StartingPoint;
		
		//bullet to shoot
		m_BulletToShoot = m_Controller.m_ProjectileToShoot;
		
		//set timers to nao
		m_SettleTime = Time.time;
		m_NormalShotTimer = Time.time;
		m_NormalGroupingTimer = Time.time;
		
		//first state to arriving!
		m_CurrentState = BossState.arriving;
	}

	public override void Start(){
	}

	public override void UpdateBehavior(){
		switch (m_CurrentState) {
		case BossState.arriving:
			m_Controller.transform.Translate(Vector3.down * m_ArrivingSpeed * Time.deltaTime, Space.World);
			if(m_Controller.transform.position.y <= m_SettlePoint.y) {
				m_polyCollider.enabled = false;
				m_CurrentState = BossState.settle;
				m_SettleTime = Time.time + m_SettleDelay;
			}
			break;
		case BossState.settle:
			if(Time.time >= m_SettleTime) {
				m_polyCollider.enabled = true;
				m_CurrentState = BossState.normal;
				m_NormalShotTimer = Time.time + m_NormalShotDelay;
			}
			break;
		case BossState.normal:
			if(Time.time >=m_NormalGroupingTimer && Time.time >= m_NormalShotTimer){
				foreach(GameObject cRef in m_Controller.m_CannonReferances){
					Stack<ProjectileController> StackToUpdate = EntitiesCreator.GetStackToUpdate(m_BulletToShoot, m_Controller.m_GameMgr);
					tempBullet = StackToUpdate.Pop();
					tempBullet.transform.position = cRef.transform.position;
					tempBullet.gameObject.SetActive(true);
				}
				m_NormalGroupingCount++;
				if(m_NormalGroupingCount >= m_NormalGroupingAmount){
					m_NormalGroupingCount = 0;
					m_NormalGroupingTimer = Time.time + m_NormalGroupingDelay;
				}
				m_NormalShotTimer = Time.time + m_NormalShotDelay;
			}

			if(m_Controller.m_CurrentHP <= 0) {
				m_CurrentState = BossState.dying;
				m_polyCollider.enabled = false;
			}

			break;
		case BossState.teleporting:
			if(m_IsTeleported) {
				m_CurrentState = BossState.normal;
				m_polyCollider.enabled = true;
				m_IsTeleported = false;
			}
			break;
		case BossState.dying:
			m_SinHorizontalOffset = SinWaveMotion(m_DyingSinFrequency, m_DyingSinAmplitude, m_SinHorizontalOffset, m_Controller.transform.right);
			break;
				}
	}

	public void TeleporterHit(){
		m_CurrentState = BossState.teleporting;
		m_IsTeleported = false;
		m_polyCollider.enabled = false;
		StartCoroutine(Teleport());
	}

	private IEnumerator Teleport(){
		float newX = Random.Range (-m_TeleportXLimiter, m_TeleportXLimiter);
		float newY = Random.Range (m_TeleportYLimiterDown, m_TeleportYLimiterUp);
		Vector3 newPosition = new Vector3 (newX, newY, 0);

		//fade out
		float speed = 1.0f / m_TeleportFadeTimer;
		for(float t = 0.0f; t < 1.0; t += Time.deltaTime*speed){
			float a = Mathf.Lerp(1.0f, 0.0f, t);
			Color faded = m_ControllerSprite.color;
			faded.a = a;
			m_ControllerSprite.color = faded;
			yield return 0;
		}

		m_Controller.transform.position = newPosition;

		//fade in
		speed = 1.0f / m_TeleportFadeTimer;
		for(float t = 0.0f; t < 1.0; t += Time.deltaTime*speed){
			float a = Mathf.Lerp(0.0f, 1.0f, t);
			Color faded = m_ControllerSprite.color;
			faded.a = a;
			m_ControllerSprite.color = faded;
			yield return 0;
		}
		m_IsTeleported = true;
		yield return 0;
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
}
