using DG.Tweening;
using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Rigidbody managerRigidbody;

        #endregion

        #region Private Variables

        private readonly string _obstacle = "Obstacle";
        private readonly string _gate = "Gate";
        private readonly string _money = "Money";
        private readonly string _chest = "Chest";
        private readonly string _miniGame = "MiniGame";

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_obstacle))
            {
                managerRigidbody.transform.DOMoveZ(managerRigidbody.transform.position.z - 10f, 1f)
                    .SetEase(Ease.OutBack);
                return;
            }

            if (other.CompareTag(_gate))
            {
                CoreGameSignals.Instance.onGateTouched?.Invoke(other.gameObject);
                return;
            }

            if (other.CompareTag(_money))
            {
                //adding money
                return;
            }

            if (other.CompareTag(_chest))
            {
                //break chest
                //collect money
            }

            if (other.CompareTag(_miniGame))
            {
                CoreGameSignals.Instance.onMiniGameEntered?.Invoke();
                DOVirtual.DelayedCall(1.5f,
                    () => CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStates.MiniGame));
                DOVirtual.DelayedCall(.5f,
                    () => CameraSignals.Instance.onSetCinemachineTarget?.Invoke(CameraTargetState.Player));
                return;
            }
        }
    }
}