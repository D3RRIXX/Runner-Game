using Game.Levels;

namespace Game.StateMachine.States
{
	public class LevelCompleteState : IState
	{
		private readonly ILevelService _levelService;

		public LevelCompleteState(ILevelService levelService)
		{
			_levelService = levelService;
		}

		public void OnEnter()
		{
			_levelService.SetCurrentLevelCompleted();
		}
		
		public void OnExit() { }
	}
}