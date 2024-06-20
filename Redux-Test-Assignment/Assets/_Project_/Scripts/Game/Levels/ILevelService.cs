using Infrastructure.ServiceLocator;

namespace Game.Levels
{
	public interface ILevelService : IService
	{
		LevelConfig GetNextLevel();
	}
}