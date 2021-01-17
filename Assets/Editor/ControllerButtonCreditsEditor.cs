using UnityEditor;
using UnityEditor.EventSystems;


[CustomEditor(typeof(ControllerButtonCredits))]
public class ControllerButtonCreditsEditor : EventTriggerEditor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		base.OnInspectorGUI();
	}
}