using Runtime.Interfaces;
using Object = UnityEngine.Object;



namespace Runtime.Commands.Level
{
    public class LevelDestroyerCommand : ICommand
    {
        private readonly LevelManager _levelManager;

        public LevelDestroyerCommand(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        public void Execute()
        {
            Object.Destroy(_levelManager.levelHolder.transform.GetChild(0).gameObject);
        }
        
       
    }
}