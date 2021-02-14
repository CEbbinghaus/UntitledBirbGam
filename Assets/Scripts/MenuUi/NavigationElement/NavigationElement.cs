using UnityEngine.EventSystems;

public abstract class NavigationElement : EventTrigger
{
	public abstract void SelectElement();
	public abstract void DeselectElement();
}
