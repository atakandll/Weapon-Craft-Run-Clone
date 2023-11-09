using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Signals;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Commands.Level
{
    public class LevelLoaderCommand : ICommand
    {
        private readonly Level_Manager _levelManager;

        public LevelLoaderCommand(Level_Manager levelManager)
        {
            _levelManager = levelManager;
        }
        public void Execute(byte parameter)
        {
            var resourceRequest = Resources.LoadAsync<GameObject>($"Prefabs/LevelPrefabs/level {parameter}");
            resourceRequest.completed += operation =>
            {
                var newLevel = Object.Instantiate(resourceRequest.asset.GameObject(),
                    Vector3.zero, Quaternion.identity);
                if (newLevel != null) newLevel.transform.SetParent(_levelManager.levelHolder.transform);
                CameraSignals.Instance.onSetCinemachineTarget?.Invoke(CameraTargetState.Player);

            };
        }
    }
}