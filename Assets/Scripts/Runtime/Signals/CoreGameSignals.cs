using System;
using Runtime.Extensions;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {

        public UnityAction<byte> onLevelInitialize = delegate(byte arg0) {  };
        public UnityAction onClearActiveLevel = delegate() {  };
        public UnityAction onLevelSuccessful = delegate { };
        public UnityAction onLevelFailed = delegate { };
        public UnityAction onNextLevel = delegate() {  };
        public UnityAction onRestartLevel = delegate() {  };
        public UnityAction onPlay = delegate { };
        public UnityAction onReset = delegate { };
        public Func<byte> onGetLevelID = delegate { return 0; };

    }
}