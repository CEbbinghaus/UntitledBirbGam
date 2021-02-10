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