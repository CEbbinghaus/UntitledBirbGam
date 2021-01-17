using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum BidgeState
{
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
		m_Agent = GetComponent<NavMeshAgent>();
		m_NavPath = new NavMeshPath();
		cachedYHeight = transform.position.y;
		//TODO: Fix when Audio Manager has been reworked
		// src = AudioManager.GetSource(AudioManager.Event.Chase);
		// bgsrc = AudioManager.instance.sources[0];
		// bgVolume = AudioManager.instance.BackgroundVolume;
	}

	public void StopChasing()
	{
		SetDeaggroTimer(5);
		state = BidgeState.Wandering;
		setRandomTarget();
	}

	// Update is called once per frame
	void Update()
	{
		if (target == null)return;

		BackgroundMusic.SetChasing(state == BidgeState.Chasing);

		switch (state)
		{
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

	private void FixedUpdate()
	{
		if (state == BidgeState.Chasing)
		{
			if (fadeTimer < 1)
				fadeTimer += Time.fixedDeltaTime / fadeDuration;
		}
		else if (state == BidgeState.Wandering)
		{
			if (fadeTimer > 0)
				fadeTimer -= Time.fixedDeltaTime / fadeDuration;
		}

		//TODO: Fix once audio manager is working
		// src.volume = Mathf.Lerp(ChaseVolume, 0, fadeTimer);
		// bgsrc.volume = Mathf.Lerp(bgVolume, 0, 1 - fadeTimer);
	}

	private bool inViewCone()
	{
		var diff = target.position - transform.position;
		float abs = Vector3.Dot(transform.forward, diff.normalized);
		if (diff.magnitude <= ViewDistance / 10 || abs > 1 - ViewCone && diff.magnitude <= ViewDistance)
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position, diff, out hit, ViewDistance))
			{
				return hit.transform.gameObject.CompareTag("Player");
			}
			else
			{
				Debug.LogError("Math is Fucked Fix your shit man");
			}
		}
		return false;
	}

	void setRandomTarget()
	{
		m_Agent.destination = m_WanderPoints[Random.Range(0, m_WanderPoints.Length)].position;
	}

	private void Chase()
	{
		var diff = target.position - transform.position;

		m_Agent.destination = target.position;
		//print(abs);

		//If the Player is Outside the View Distance
		if (m_DeaggroTimer > 0 || !inViewCone())
		{
			state = BidgeState.Wandering;

			if (Vector3.Distance(transform.position, m_Agent.destination) <= 2.0f)
				setRandomTarget();
		}
	}

	private void Wander()
	{
		if (m_DeaggroTimer <= 0.0f && inViewCone())
		{
			AudioManager.PlaySound("BidgeScream", transform.position);
			state = BidgeState.Chasing;
			m_Agent.destination = target.position;
			return;
		}

		//Go to Random Wander point if at Current Wander Point
		if (Vector3.Distance(transform.position, m_Agent.destination) <= 2.0f)
			setRandomTarget();
	}

	public void SetDeaggroTimer(float deaggroTimer)
	{
		m_DeaggroTimer = deaggroTimer;
	}
}
