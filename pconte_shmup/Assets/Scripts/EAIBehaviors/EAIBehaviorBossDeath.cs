using UnityEngine;
using System.Collections;

public class EAIBehaviorBossDeath : EAIBehaviors {
	private string m_DeathType = "boss";
	private string m_Type = "death";
	private Animator m_Animator;
	private SpriteRenderer m_ControllerSprite;
	private Color m_AlphaColor;
	private float m_ExplosionDecreasingCount;

	public override void Init(EnemyController controller){
		base.Init (controller);
		m_BehaviorName = m_Type;
		m_BehaviorDeathType = m_DeathType;
		m_Animator = m_Controller.m_BlueExplosion.GetComponent<Animator> ();
		if (m_Animator == null)
						print ("Can't find an animator in explosion");

		m_ControllerSprite = m_Controller.GetComponent<SpriteRenderer>();
		m_AlphaColor = m_ControllerSprite.color;
	}
	
	// Use this for initialization
	public override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	public override void UpdateBehavior () {
		
	}

	public override void StartExplosions(int explosionCount){
		StartCoroutine (DeathExplosions (explosionCount));
	}

	private IEnumerator DeathExplosions(float amountOfExplosions){
		m_ExplosionDecreasingCount = amountOfExplosions;
		Transform shipTransform = m_Controller.transform;
		GameObject explosion = m_Controller.m_BlueExplosion;
		float explosionAnimationTime = 0.3f;
		float delayForNextExplosion = 0.1f;
		Vector3 mins = m_Controller.renderer.bounds.min;
		Vector3 maxs = m_Controller.renderer.bounds.max;
		for(int i = 0; i < amountOfExplosions; i++){
			m_ExplosionDecreasingCount--;
			float alpha = m_ExplosionDecreasingCount / amountOfExplosions;
			m_AlphaColor.a = alpha;
			m_ControllerSprite.color = m_AlphaColor;
			float xOffset = (Random.Range(mins.x, maxs.x));
			float yOffset = (Random.Range(mins.y, maxs.y));
			Vector3 newPos = shipTransform.transform.position + new Vector3(xOffset, yOffset-2.0f, 0);			               
			GameObject newExplosion = Instantiate (explosion, newPos, shipTransform.transform.rotation) as GameObject;
			Destroy(newExplosion, explosionAnimationTime);
			yield return new WaitForSeconds(delayForNextExplosion);
		}
		m_Controller.DestroyObjectAndBehaviors();
		yield return null;
	}
}
