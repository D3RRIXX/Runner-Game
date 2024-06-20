using Game.Blocks;
using Game.Events;
using Game.StateMachine;
using Game.StateMachine.States;
using Infrastructure.EventBus;
using Infrastructure.ServiceLocator;
using UnityEngine;

namespace Game.Player
{
	public class PlayerRespawnManager : MonoBehaviour
	{
		private IEventService _eventService;
		private Block _lastPassedBlock;
		private IGameStateMachine _gameStateMachine;

		private void Awake()
		{
			_eventService = AllServices.Container.GetSingle<IEventService>();
			_gameStateMachine = AllServices.Container.GetSingle<IGameStateMachine>();

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
			PlayerHealth player = evt.Player;
			if (player.Lives.Value == 0)
			{
				_gameStateMachine.Enter<LevelFailedState>();
				return;
			}
			
			Transform point = _lastPassedBlock.RespawnPoint;
			player.transform.SetPositionAndRotation(point.position, point.rotation);
			player.SetInvincible().Forget();
		}
	}
}