using System.Collections.Generic;
using System.Linq;
using Game.Blocks;
using Game.Levels;
using Infrastructure.ServiceLocator;
using UnityEngine;

namespace UI.Screens
{
	public class LevelProgressView : MonoBehaviour
	{
		[SerializeField] private LevelProgressItem _listItemPrefab;
		[SerializeField] private Transform _contentParent;

		private LevelState _levelState;

		private void Awake()
		{
			_levelState = AllServices.Container.GetSingle<LevelState>();
		}

		private void OnEnable()
		{
			var lookup = _levelState.Level.Blocks.Take(_levelState.BlocksPassed)
			                           .GroupBy(x => x)
			                           .ToDictionary(x => x.Key, x => x.Count());

			foreach (KeyValuePair<BlockType, int> pair in lookup)
			{
				var listItem = Instantiate(_listItemPrefab, _contentParent);
				listItem.Construct(pair.Key, pair.Value);
			}
		}
	}
}