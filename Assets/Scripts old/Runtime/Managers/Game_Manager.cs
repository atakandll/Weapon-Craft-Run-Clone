using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class Game_Manager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public GameStates States;

        #endregion

        #endregion

        protected void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameStates += OnChangeGameStates;
        }

        private void OnChangeGameStates(GameStates newStates)
        {
            States = newStates;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameStates -= OnChangeGameStates;
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}