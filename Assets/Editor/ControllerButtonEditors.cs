using UnityEditor;
using UnityEditor.EventSystems;


[CustomEditor(typeof(ControllerButtonResume))]
public class ControllerButtonResumeEditor : EventTriggerEditor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		base.OnInspectorGUI();
	}
}

[CustomEditor(typeof(ControllerButtonLoadScene))]
public class ControllerButtonLoadSceneEditor : EventTriggerEditor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		base.OnInspectorGUI();
	}
}

[CustomEditor(typeof(ControllerButtonMenuOpen))]
public class ControllerButtonMenuOpenEditor : EventTriggerEditor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		base.OnInspectorGUI();
	}
}