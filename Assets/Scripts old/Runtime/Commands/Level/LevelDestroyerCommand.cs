using Runtime.Interfaces;
using Object = UnityEngine.Object;



namespace Runtime.Commands.Level
{
    public class LevelDestroyerCommand : ICommand
    {
        private readonly Level_Manager _levelManager;

        public LevelDestroyerCommand(Level_Manager levelManager)
        {
            _levelManager = levelManager;
        }

        public void Execute()
        {
            Object.Destroy(_levelManager.levelHolder.transform.GetChild(0).gameObject);
        }
        
       
    }
}