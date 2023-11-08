using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class UI_Manager : MonoBehaviour
    {
          private void OnEnable()
        {
            SubscribeEvents();
            OpenStartPanel();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnLevelInitialize;
            CoreGameSignals.Instance.onLevelFailed += OnLevelFailed;
            CoreGameSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
            CoreGameSignals.Instance.onReset += OnReset;
        }
        private void OpenStartPanel()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 0);
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Level, 1);
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Shop, 2);
        }
        private void OnLevelInitialize(byte levelValue)
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 0);
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Level, 1);
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Shop, 2);
            UISignals.Instance.onSetNewLevelValue?.Invoke(levelValue);
        }
        public void OnPlay()
        {
            CoreGameSignals.Instance.onPlay?.Invoke();
            CoreUISignals.Instance.onClosePanel?.Invoke(0);
            CoreUISignals.Instance.onClosePanel?.Invoke(2);
            //CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStates.Follow);
        }
        private void OnOpenWinPanel()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Win, 2);
        }

        private void OnOpenFailPanel()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Fail, 2);
        }

        public void OnNextLevel()
        {
            CoreGameSignals.Instance.onNextLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
        }

        public void OnRestartLevel()
        {
            CoreGameSignals.Instance.onRestartLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
        }

        private void OnLevelFailed()
        {
            OnOpenFailPanel();
        }

        private void OnLevelSuccessful()
        {
            OnOpenWinPanel();
        }

        public void OnFireRateUpdate()
        {
            UISignals.Instance.onClickFireRate?.Invoke();
            UISignals.Instance.onSetFireRateText?.Invoke();
        }

        public void OnInitYearUpdate()
        {
            UISignals.Instance.onClickInitYear?.Invoke();
            UISignals.Instance.onSetInitYearText?.Invoke();
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnLevelInitialize;
            CoreGameSignals.Instance.onLevelFailed -= OnLevelFailed;
            CoreGameSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
            CoreGameSignals.Instance.onReset -= OnReset;
        }


        private void OnReset()
        {
            //CoreUISignals.Instance.onCloseAllPanels?.Invoke();
        }
    }
}