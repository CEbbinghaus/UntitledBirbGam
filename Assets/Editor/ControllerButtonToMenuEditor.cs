using UnityEditor;
using UnityEditor.EventSystems;


[CustomEditor(typeof(ControllerButtonToMenu))]
public class ControllerButtonToMenuEditor : EventTriggerEditor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		base.OnInspectorGUI();
	}
}