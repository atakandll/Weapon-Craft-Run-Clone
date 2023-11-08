using System;
using Runtime.Extensions;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class UISignals : MonoSingleton<UISignals>
    {
        public UnityAction onSetFireRateText = delegate { };
        public UnityAction onSetInitYearText = delegate { };
        public UnityAction<byte> onSetNewLevelValue = delegate { };
        public UnityAction<int> onSetMoneyValue = delegate { };
        public Func<int> onGetMoneyValue = delegate { return 0; };
        
        public UnityAction onClickFireRate = delegate { };
        public UnityAction onClickInitYear = delegate { };
    }
}