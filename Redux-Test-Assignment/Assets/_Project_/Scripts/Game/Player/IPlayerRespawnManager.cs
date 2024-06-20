using System;
using Infrastructure.ServiceLocator;

namespace Game.Player
{
	public interface IPlayerRespawnManager : IService, IDisposable
	{
		void Initialize();
		void RespawnPlayer(PlayerHealth player, bool withFullHealth = false);
	}
}