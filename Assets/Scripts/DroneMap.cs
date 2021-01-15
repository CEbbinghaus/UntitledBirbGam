using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DroneMap
{
	[MenuItem("Drone/Make Map")]
	static void GenerateDroneMap()
	{
		List<DroneWaypoint> waypoints = Object.FindObjectsOfType<DroneWaypoint>().ToList();

		waypoints.ForEach(w => w.visibleWaypoints.Clear());

		foreach (DroneWaypoint sourceWaypoint in waypoints)
		{
			foreach (DroneWaypoint targetWaypoint in waypoints)
			{
				// If the source and target are the same, or they already appear on each other's lists, continue
				if (sourceWaypoint == targetWaypoint ||
					sourceWaypoint.visibleWaypoints.Contains(targetWaypoint) ||
					targetWaypoint.visibleWaypoints.Contains(sourceWaypoint))
					continue;

				if (Physics.Linecast(sourceWaypoint.transform.position, targetWaypoint.transform.position, out RaycastHit hit))
				{
					if (hit.transform.CompareTag("Bidge") || hit.transform.CompareTag("Frisbee") || hit.transform.CompareTag("Player"))
					{
						sourceWaypoint.visibleWaypoints.Add(targetWaypoint);
						targetWaypoint.visibleWaypoints.Add(sourceWaypoint);
						Debug.DrawLine(sourceWaypoint.transform.position, targetWaypoint.transform.position, Color.green, 5f);
					}
				}
				else
				{
					sourceWaypoint.visibleWaypoints.Add(targetWaypoint);
					targetWaypoint.visibleWaypoints.Add(sourceWaypoint);
					Debug.DrawLine(sourceWaypoint.transform.position, targetWaypoint.transform.position, Color.green, 5f);
				}
			}
		}
	}

	[MenuItem("Drone/Draw Map")]
	static void DrawDroneMap()
	{
		DroneWaypoint[] waypoints = Object.FindObjectsOfType<DroneWaypoint>();

		foreach (DroneWaypoint waypoint in waypoints)
		{
			foreach (DroneWaypoint waypoint1 in waypoint.visibleWaypoints)
			{
				Debug.DrawLine(waypoint.transform.position, waypoint1.transform.position, Color.white, 15f);
			}
		}
	}

	[MenuItem("Drone/Clear Map")]
	static void ClearDroneMap()
	{
		List<DroneWaypoint> waypoints = Object.FindObjectsOfType<DroneWaypoint>().ToList();

		waypoints.ForEach(w => w.visibleWaypoints.Clear());
	}
}
