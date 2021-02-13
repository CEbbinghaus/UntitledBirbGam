using UnityEditor;
using UnityEditor.EventSystems;


[CustomEditor(typeof(ControllerButtonPauseOptions))]
public class ControllerButtonPauseOptionsEditor : EventTriggerEditor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		base.OnInspectorGUI();
	}
}