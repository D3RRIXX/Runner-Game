using UnityEngine;

namespace Game.Blocks
{
	public partial class Block : MonoBehaviour
	{
		[SerializeField] private Transform _respawnPoint;
		[SerializeField] private Transform _endPoint;

		public Vector3 RespawnPoint => _respawnPoint.position;
		public (Vector3 position, Quaternion rotation) NextBlockSpawnTransform => (_endPoint.position, _endPoint.rotation);
	}
}