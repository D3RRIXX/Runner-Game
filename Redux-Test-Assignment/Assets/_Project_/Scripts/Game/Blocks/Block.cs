using UnityEngine;

namespace Game.Blocks
{
	public class Block : MonoBehaviour
	{
		[SerializeField] private Transform _respawnPoint;

		public Vector3 RespawnPoint => _respawnPoint.position;
	}
}