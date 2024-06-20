using Game.Levels;
using Game.StateMachine;
using Infrastructure.ServiceLocator;

namespace Game
{
	public class Application
	{
		public Application(LevelGenerationConfig levelGenerationConfig)
		{
			StateMachine = new GameStateMachine(AllServices.Container, levelGenerationConfig);
		}
		public IGameStateMachine StateMachine { get; }
	}
}