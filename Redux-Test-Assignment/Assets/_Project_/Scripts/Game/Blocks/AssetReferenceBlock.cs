using UnityEngine.AddressableAssets;

namespace Game.Blocks
{
	public class AssetReferenceBlock : AssetReferenceT<Block>
	{
		public AssetReferenceBlock(string guid) : base(guid) { }
	}
}