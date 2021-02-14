using UnityEditor;
using UnityEditor.EventSystems;

[CustomEditor(typeof(OptionSlider))]
public class OptionSliderEditor : EventTriggerEditor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		base.OnInspectorGUI();
	}
}
