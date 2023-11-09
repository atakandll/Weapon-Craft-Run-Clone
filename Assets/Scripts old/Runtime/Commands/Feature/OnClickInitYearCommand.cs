using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Feature
{
    public class OnClickInitYearCommand : MonoBehaviour
    {
        
        private Feature_Manager _featureManager;
        private byte _initYearLevel;
        private int _newPriceTag;

        public OnClickInitYearCommand(Feature_Manager featureManager, ref int newPriceTag, ref byte  initYearLevel)
        {
            _featureManager = featureManager;
            _newPriceTag = newPriceTag;
            _initYearLevel =  initYearLevel;
        }

        internal void Execute()
        {
            _newPriceTag = (int)(CoreGameSignals.Instance.onGetInitYearLevel() -
                                 ((Mathf.Pow(2, Mathf.Clamp( _initYearLevel, 0, 10)) * 100)));
            _initYearLevel += 1;
            ScoreSignals.Instance.onSendMoney?.Invoke((int)_newPriceTag);
            UISignals.Instance.onSetMoneyValue?.Invoke((int)_newPriceTag);
            _featureManager.SaveFeatureData();
        }
    }
}