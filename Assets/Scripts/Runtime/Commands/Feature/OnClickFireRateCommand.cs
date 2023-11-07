using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Feature
{
    public class OnClickFireRateCommand : MonoBehaviour
    {
        private readonly FeatureManager _featureManager;
        private int _newPriceTag;
        private byte _fireRateLevel;

        public OnClickFireRateCommand(FeatureManager featureManager, ref int newPriceTag, ref byte fireRateLevel)
        {
            _featureManager = featureManager;
            _newPriceTag = newPriceTag;
            _fireRateLevel = fireRateLevel;
            
        }

        internal void Execute()
        {
            _newPriceTag = (int)(CoreGameSignals.Instance.onGetFireRateLevel() -
                                 ((Mathf.Pow(2, Mathf.Clamp(_fireRateLevel, 0, 10)) * 100)));
            _fireRateLevel += 1;
            ScoreSignals.Instance.onSendMoney?.Invoke((int) _newPriceTag);
            UISignals.Instance.onSetMoneyValue?.Invoke((int) _newPriceTag);
            _featureManager.SaveFeatureData();
        }
    }
}