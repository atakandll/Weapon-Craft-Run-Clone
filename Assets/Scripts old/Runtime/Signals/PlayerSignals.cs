using Runtime.Enums;
using Runtime.Extensions;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class PlayerSignals : MonoSingleton<PlayerSignals>
    {
        public UnityAction<PlayerAnimationStates> onChangePlayerAnimationState = delegate { };
        public UnityAction<bool> onPlayConditionChanged = delegate { };
        public UnityAction<bool> onMoveConditionChanged = delegate { };
        public UnityAction<int> onSetTotalScore = delegate { };
    }
}