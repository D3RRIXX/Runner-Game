using Game.Blocks;
using Game.Events;
using Infrastructure.EventBus;
using Infrastructure.ServiceLocator;
using UnityEngine;

namespace Game.Player
{
	public class PlayerRespawnManager : MonoBehaviour
	{
		private IEventService _eventService;
		private Block _lastPassedBlock;

		private void Awake()
		{
			_eventService = AllServices.Container.GetSingle<IEventService>();

			_eventService.Subscribe<BlockPassedEvent>(OnBlockPassed);
			_eventService.Subscribe<PlayerDiedEvent>(OnPlayerDied);
		}

		private void OnDestroy()
		{
			_eventService.Unsubscribe<BlockPassedEvent>(OnBlockPassed);
			_eventService.Unsubscribe<PlayerDiedEvent>(OnPlayerDied);
		}

		private void OnBlockPassed(BlockPassedEvent evt)
		{
			Debug.Log($"Player passed {evt.Block.BlockType} block", evt.Block);
			_lastPassedBlock = evt.Block;
		}

		private void OnPlayerDied(PlayerDiedEvent evt)
		{
			Transform point = _lastPassedBlock.RespawnPoint;
			evt.Player.transform.SetPositionAndRotation(point.position, point.rotation);
		}
	}
}