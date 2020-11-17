using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class FindMissingScriptsRecursively : EditorWindow
{
	static int go_count = 0, components_count = 0, missing_count = 0;
	static HashSet<Component> IteratedComponents = new HashSet<Component>();

	[MenuItem("Window/FindMissingScriptsRecursively")]
	public static void ShowWindow()
	{
		FindInSelected();
		//EditorWindow.GetWindow(typeof(FindMissingScriptsRecursively));
	}

	public void OnGUI()
	{
		if (GUILayout.Button("Find Missing Scripts in selected GameObjects"))
		{
			FindInSelected();
		}
	}
	private static void FindInSelected()
	{
		IteratedComponents.Clear();
		GameObject[] go = Selection.gameObjects;
		go_count = 0;
		components_count = 0;
		missing_count = 0;
		foreach (GameObject g in go)
		{
			FindInGO(g);
		}
		Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
	}

	private static void FindInGO(GameObject g)
	{
		go_count++;
		Component[] components = g.GetComponents<Component>();
		for (int i = 0; i < components.Length; i++)
		{
			components_count++;
			if (components[i] != null && IteratedComponents.Contains(components[i]))continue;
			if (components[i] == null)
			{
				missing_count++;
				string s = g.name;
				Transform t = g.transform;
				while (t.parent != null)
				{
					s = t.parent.name + "/" + s;
					t = t.parent;
				}
				Debug.Log(s + " has an empty script attached in position: " + i, g);
			}
		}
		// Now recurse through each child GO (if there are any):
		foreach (Transform childT in g.transform)
		{
			//Debug.Log("Searching " + childT.name  + " " );
			FindInGO(childT.gameObject);
		}
	}
}

#endif
