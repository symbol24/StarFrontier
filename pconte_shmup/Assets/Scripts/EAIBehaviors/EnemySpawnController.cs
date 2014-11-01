using UnityEngine;
using System.Collections;

public class EnemySpawnController : MonoBehaviour {
	private GameManager m_GameManager;

	//the enemy ships
	public EnemyController[] m_Enemies;
	private int m_EnemySelector = 0;
	private int m_EnemyCounter = 0;
	public int[] m_EnemnySpawnRange;
	public float m_LimiterY = 4.5f;
	public float m_LimiterX = 2.5f;
	public float m_MinSpawnRate = 1.0f;
	public float m_MaxSpawnRate = 2.0f;
	private float m_NextSpawn = 0.0F;



	// Use this for initialization
	void Start () {
		m_GameManager = GameObject.Find ("GameManagerObj").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(m_GameManager != null && m_GameManager.m_CurrentState == GameManager.gameState.playing){
			//spawn enemies
			if (Time.time > m_NextSpawn){
				m_NextSpawn = Time.time + Random.Range(m_MinSpawnRate, m_MaxSpawnRate);
				float tempX = Random.Range(-m_LimiterX, m_LimiterX);
				EnemyController eaiClone;
				eaiClone = Instantiate(m_Enemies[m_EnemySelector], new Vector2(tempX, transform.position.y + m_LimiterY), transform.rotation) as EnemyController;
				m_EnemyCounter++;
				
				//addind a tank into the mix from time to time
				if(m_EnemySelector != 1 && m_EnemyCounter >= Random.Range(m_EnemnySpawnRange[0], m_EnemnySpawnRange[1])){
					m_EnemySelector = 1;
				}else{
					m_EnemySelector = 0;
				}
			}
		}
	}
}
