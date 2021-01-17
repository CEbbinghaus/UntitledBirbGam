using UnityEditor;
using UnityEditor.EventSystems;


[CustomEditor(typeof(ControllerButtonOptions))]
public class ControllerButtonOptionsEditor : EventTriggerEditor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		base.OnInspectorGUI();
	}
}