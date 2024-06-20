using Game.Levels;
using Game.Player;
using Infrastructure.ServiceLocator;
using TMPro;
using UnityEngine;

namespace UI.Screens
{
	public class HudView : MonoBehaviour
	{
		[SerializeField] private TMP_Text _livesLabel;
		
		private PlayerHealth _player;

		private void Awake()
		{
			_player = AllServices.Container.GetSingle<LevelState>().Player;
		}
	}
}