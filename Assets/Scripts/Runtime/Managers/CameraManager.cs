using System;
using Cinemachine;
using Runtime.Enums;
using Runtime.Signals;
using Unity.Mathematics;
using UnityEngine;

namespace Runtime.Managers
{
    public class CameraManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;
        [SerializeField] private Animator animator;

        #endregion

        #region Private Variables

        private float3 _initialPosition;

        #endregion

        #endregion

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _initialPosition = transform.position;
        }
         private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onReset += OnReset;
            CameraSignals.Instance.onSetCinemachineTarget += OnSetCinemachineTarget;
            CameraSignals.Instance.onChangeCameraState += OnChangeCameraState;
        }

        private void OnSetCinemachineTarget()
        {
            //var playerManager = FindObjectOfType<PlayerManager>();
            //stateDrivenCamera.Follow = playerManager;
        }


        private void OnChangeCameraState(CameraStates state)
        {
            animator.SetTrigger(state.ToString());
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onReset -= OnReset;
            CameraSignals.Instance.onSetCinemachineTarget -= OnSetCinemachineTarget;
            CameraSignals.Instance.onChangeCameraState -= OnChangeCameraState;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        private void OnReset()
        {
            CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStates.Initial);
            stateDrivenCamera.Follow = null;
            stateDrivenCamera.LookAt = null;
            transform.position = _initialPosition;
        }
    }
}