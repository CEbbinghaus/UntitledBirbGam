public class ControllerButtonOptions : ControllerButton
{
	public override void Press()
	{
		base.Press();
		ControllerInputMenu.instance.activeSubMenu = UIPan.instance.activeOptions;
		UIPan.instance.ChangeState(UIPan.instance.activeOptions, UIPanState.MovingOnscreen);
		ControllerInputMenu.instance.menuState = MenuState.SubMenu;
	}
}