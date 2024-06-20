using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Game.Levels
{
	[CreateAssetMenu(fileName = "Level List", menuName = SOPaths.BASE + "Level List", order = 0)]
	public class LevelList : ScriptableObject
	{
		[SerializeField] private LevelConfig[] _levels;

		public IReadOnlyList<LevelConfig> Levels => _levels;
	}
}