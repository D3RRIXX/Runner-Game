using UnityEngine;

namespace UI
{
	public class UIScreen : MonoBehaviour
	{
		[SerializeField] private UIScreenType _screenType;

		public UIScreenType ScreenType => _screenType;
	}
}