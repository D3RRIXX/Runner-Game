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

		private LevelProgress _levelProgress;

		private void Awake()
		{
			_levelProgress = AllServices.Container.GetSingle<LevelProgress>();
		}

		private void OnEnable()
		{
			var lookup = _levelProgress.Level.Blocks.Take(_levelProgress.BlocksPassed)
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