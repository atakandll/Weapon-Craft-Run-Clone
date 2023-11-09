using Runtime.Extensions;
using Runtime.Keys;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class InputSignals : MonoSingleton<InputSignals>
    {
        public UnityAction onFirstTimeTouchTaken = delegate { };
        public UnityAction onInputTaken = delegate { };
        public UnityAction<HorizontalInputParams> onInputDragged = delegate { };
        public UnityAction onInputReleased = delegate { };
        public UnityAction<bool> onChangeInputState = delegate { };
    }
}