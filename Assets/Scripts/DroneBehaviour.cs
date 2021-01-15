using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DroneBehaviour : MonoBehaviour
{
	static DroneWaypoint[] waypoints;

	DroneWaypoint[] history = new DroneWaypoint[10];

	[SerializeField]
	Transform[] rotors;

	DroneWaypoint lastTarget;
	DroneWaypoint target;

	[SerializeField]
	float RotationSpeed = 20;

	[SerializeField]
	float rotorRotationSpeed = 20;

	[SerializeField]
	float minDistance = .2f;

	[SerializeField]
	float Speed = 10f;

	[SerializeField]
	float angle = -10f;

	float localAngle = 0.0f;

	Quaternion lastDir = Quaternion.identity;

	float timer = 0.0f;

	[SerializeField]
	Vector2 RamdomWaitTime;

	void Awake()
	{
		waypoints = FindObjectsOfType<DroneWaypoint>();

		if (waypoints.Length <= 0)
			Debug.LogError("Could not find Drone Waypoints");

		DroneWaypoint closest = waypoints[0];
		float least = Vector3.Distance(transform.position, closest.transform.position);
		foreach (DroneWaypoint point in waypoints)
		{
			float distance = Vector3.Distance(transform.position, point.transform.position);
			if (distance < least)
			{
				closest = point;
				least = distance;
			}
		}

		target = closest;
	}

	void MoveDown(DroneWaypoint point)
	{
		Array.Copy(history, 0, history, 1, history.Length - 1);
		history[0] = point;
	}

	void Update()
	{
		localAngle = Mathf.Lerp(localAngle, target == null ? 0 : angle, Time.deltaTime);

		Quaternion targetRotation = Quaternion.identity;

		if (target != null)
		{
			Vector3 diff = target.transform.position - transform.position;
			diff.y = 0;
			diff.Normalize();

			lastDir = targetRotation = Quaternion.LookRotation(diff, Vector3.up);

			transform.position += diff * Speed * Time.deltaTime;

			//Drone has arrived at the Waypoint
			if (Vector3.Distance(transform.position, target.transform.position) < minDistance)
			{

				MoveDown(target);
				lastTarget = target;
				target = null;
				timer = UnityEngine.Random.Range(RamdomWaitTime.x, RamdomWaitTime.y);

			}

		}
		else
		{
			targetRotation = lastDir;

			if (timer <= 0)
			{
				var elements = lastTarget.visibleWaypoints.Where((DroneWaypoint waypoint) => Array.FindIndex(history, (DroneWaypoint e) => e == waypoint) == -1).ToArray();
				if (elements.Length <= 0)
				{
					elements = lastTarget.visibleWaypoints.ToArray();
				}

				target = elements.RandomElement();
			}
			else
			{
				timer -= Time.deltaTime;
			}
		}
		targetRotation *= Quaternion.Euler(localAngle, 0, 0);

		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);

		for (int i = 0; i < rotors.Length; ++i)
		{
			rotors[i].rotation *= Quaternion.Euler(0, rotorRotationSpeed, 0);
		}
	}

}
