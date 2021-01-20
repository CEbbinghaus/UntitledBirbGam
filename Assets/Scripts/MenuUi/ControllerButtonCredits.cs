public class ControllerButtonCredits : ControllerButton
{
	public override void Press()
	{
		base.Press();
		ControllerInputMenu.instance.activeSubMenu = UIPan.instance.activeCredits;
		UIPan.instance.ChangeState(UIPan.instance.activeCredits, UIPanState.MovingOnscreen);
		ControllerInputMenu.instance.menuState = MenuState.SubMenu;
	}
}
