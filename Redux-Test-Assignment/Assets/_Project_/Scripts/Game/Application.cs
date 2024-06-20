using Game.Levels;
using Game.StateMachine;
using Infrastructure.ServiceLocator;

namespace Game
{
	public class Application
	{
		public Application(LevelList levelList)
		{
			StateMachine = new GameStateMachine(AllServices.Container, levelList);
		}
		public IGameStateMachine StateMachine { get; }
	}
}