using Runtime.Enums;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Animator animator;

        #endregion

        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerSignals.Instance.onChangePlayerAnimationState += OnChangePlayerAnimationState;
        }

        private void OnDisable()
        {
            UnSubcribeEvents();
        }

        private void UnSubcribeEvents()
        {
            PlayerSignals.Instance.onChangePlayerAnimationState -= OnChangePlayerAnimationState;
        }

        private void OnChangePlayerAnimationState(PlayerAnimationStates states)
        {
            animator.SetTrigger(states.ToString());
        }

        internal void OnReset()
        {
            PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Idle);
        }
    }
}