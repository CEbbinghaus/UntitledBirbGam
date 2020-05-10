using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum BidgeState {
	Chasing,
	Wandering
}

public class BidgeBehaviour : MonoBehaviour
{


	/// <summary>
	/// The Nav Mesh Agent
	/// </summary>
	private NavMeshAgent m_Agent = null;

	/// <summary>
	/// A path on the nav mesh.
	/// </summary>
	private NavMeshPath m_NavPath = null;

	/// <summary>
	/// Points on the map for the AI to wander to when it can't reach the player.
	/// </summary>
	public Transform[] m_WanderPoints;

	//private RaycastHit m_VisionRaycastHit = null;

	public Transform target;

	float cachedYHeight;

	[Range(0, 1)]
	public float ChaseVolume;
	float bgVolume;

	AudioSource src;
	AudioSource bgsrc;

	public float fadeDuration = 10000;
	float fadeTimer = 0.0f; 

	[Range(0, 1)]
	public float ViewCone = .05f;
	public float ViewDistance = 10f;


	public float m_DeaggroTimer = 10.0f;


	BidgeState state;

    /// <summary>
	/// On startup.
	/// </summary>
    void Start()
    {
		src = AudioManager.GetSource(AudioManager.Event.Chase);
		bgVolume = AudioManager.instance.BackgroundVolume;
		bgsrc = AudioManager.instance.sources[0];
		m_Agent = GetComponent<NavMeshAgent>();
		m_NavPath = new NavMeshPath();
		cachedYHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
		if(target == null)return;

		switch(state){
			case BidgeState.Chasing:
				Chase();
			break;
			case BidgeState.Wandering:
				Wander();
			break;
		}

		if (m_DeaggroTimer > 0.0f)
		{
			m_DeaggroTimer -= Time.deltaTime;
		}
	}

	private void FixedUpdate() {
		if(state == BidgeState.Chasing){
			if(fadeTimer < 1)
				fadeTimer += Time.fixedDeltaTime / fadeDuration;
		}else if(state == BidgeState.Wandering){
			if(fadeTimer > 0)
				fadeTimer -= Time.fixedDeltaTime / fadeDuration;
		}

		src.volume = Mathf.Lerp(ChaseVolume, 0, fadeTimer);
		bgsrc.volume = Mathf.Lerp(bgVolume, 0, 1 - fadeTimer);
	}

	private void Chase(){
		var diff = target.position - transform.position;

		m_Agent.destination = target.position;

		float abs = Mathf.Abs(Vector3.Dot(transform.forward, diff.normalized));
		print(abs);

		//If the Player is Outside the View Distance
		if(m_DeaggroTimer > 0 || diff.magnitude > ViewDistance * 1.5){
			state = BidgeState.Wandering;
			m_Agent.destination = m_WanderPoints[Random.Range(0, m_WanderPoints.Length)].position;
		}
	}

	private void Wander()
	{
		//transform.position = new Vector3(transform.position.x, cachedYHeight, transform.position.z);

		var diff = target.position - transform.position;
		var dir = diff.normalized;

		//Check if the Player is within Bidge's View Cone
		float abs = Vector3.Dot(transform.forward, diff.normalized);
		print(abs);

		//Physics.Raycast(transform.position, (m_PlayerCharacterTransform.position - transform.position),out m_VisionRaycastHit, ViewDistance);
		
		// If Bidge can find a path to the player character and can see them, chase them.
		// if (m_VisionRaycastHit.collider.tag == "Player" && m_DeaggroTimer <= 0.0f)
		// {
			//Debug.Log("See the player!");

		// }

		//If the Player is within the Distance that the Bird can see
		if(abs > 1 - ViewCone && diff.magnitude <= ViewDistance){
			state = BidgeState.Chasing;
			m_Agent.destination = target.position;
			return;
		}

		//Go to Random Wander point if at Current Wander Point
		if(Vector3.Distance(transform.position, m_Agent.destination) <= 2.0f)
			m_Agent.destination = m_WanderPoints[Random.Range(0, m_WanderPoints.Length)].position;
	}

	public void SetDeaggroTimer(float deaggroTimer)
	{
		m_DeaggroTimer = deaggroTimer;
	}
}