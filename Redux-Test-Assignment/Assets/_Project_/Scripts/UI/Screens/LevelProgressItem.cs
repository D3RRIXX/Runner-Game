using Game.Blocks;
using TMPro;
using UnityEngine;

namespace UI.Screens
{
	public class LevelProgressItem : MonoBehaviour
	{
		[SerializeField] private TMP_Text _blockLabel;
		[SerializeField] private TMP_Text _passedLabel;

		public void Construct(BlockType blockType, int passed)
		{
			_blockLabel.text = blockType.ToString();
			_blockLabel.text = passed.ToString();
		}
	}
}