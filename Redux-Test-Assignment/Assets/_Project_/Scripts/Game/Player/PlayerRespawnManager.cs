using Game.Blocks;
using Game.Events;
using Game.Levels;
using Game.StateMachine;
using Game.StateMachine.States;
using Infrastructure.EventBus;
using UnityEngine;

namespace Game.Player
{
	public class PlayerRespawnManager : IPlayerRespawnManager
	{
		private const float RESPAWN_INVINCIBILITY = 3f;

		private readonly IEventService _eventService;
		private readonly IGameStateMachine _gameStateMachine;
		private readonly LevelState _levelState;

		private Block _lastPassedBlock;

		public PlayerRespawnManager(IEventService eventService, IGameStateMachine gameStateMachine, LevelState levelState)
		{
			_eventService = eventService;
			_gameStateMachine = gameStateMachine;
			_levelState = levelState;
		}

		public void Initialize()
		{
			_eventService.Subscribe<BlockPassedEvent>(OnBlockPassed);
			_eventService.Subscribe<PlayerDiedEvent>(OnPlayerDied);
		}

		public void Dispose()
		{
			_eventService.Unsubscribe<BlockPassedEvent>(OnBlockPassed);
			_eventService.Unsubscribe<PlayerDiedEvent>(OnPlayerDied);
		}

		private void OnBlockPassed(BlockPassedEvent evt)
		{
			_lastPassedBlock = evt.Block;
		}

		public void RespawnPlayer(bool withFullHealth = false)
		{
			PlayerHealth player = _levelState.Player;
			Vector3 position;
			Quaternion rotation;

			if (withFullHealth)
			{
				player.RestoreLives(player.MaxLives);
				Transform point = _lastPassedBlock.NextBlockSpawnTransform;
				position = point.position;
				position.y = _lastPassedBlock.RespawnPoint.position.y;
				
				rotation = point.rotation;
			}
			else
			{
				Transform point = _lastPassedBlock.RespawnPoint;
				position = point.position;
				rotation = point.rotation;
			}

			player.transform.SetPositionAndRotation(position, rotation);
			player.SetInvincible(RESPAWN_INVINCIBILITY).Forget();
		}

		private void OnPlayerDied(PlayerDiedEvent evt)
		{
			PlayerHealth player = evt.Player;
			if (player.Lives.Value == 0)
			{
				_gameStateMachine.Enter<LevelFailedState>();
				return;
			}

			RespawnPlayer();
		}
	}
}