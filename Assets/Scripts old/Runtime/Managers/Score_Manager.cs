using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class Score_Manager : MonoBehaviour
    {
        
        #region Self Variables

        #region Private Variables

         private int _money;
         private int _valueMultiplier;
         private int _scoreCache = 0;
         private int _gateScoreValue = 0;

        #endregion

        #endregion

        private void Awake()
        {
            _money = GetMoneyValue();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            ScoreSignals.Instance.onSendMoney += OnSendMoney;
            ScoreSignals.Instance.onGetMoney += () => _money;
            ScoreSignals.Instance.onSetScore += OnSetScore;
            ScoreSignals.Instance.onSetGateScore += OnSetAtmScore;
            CoreGameSignals.Instance.onMiniGameStart +=
                () => ScoreSignals.Instance.onSendFinalScore?.Invoke(_scoreCache);
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onLevelSuccessful += RefreshMoney;
            CoreGameSignals.Instance.onLevelFailed += RefreshMoney;
            UISignals.Instance.onClickInitYear += OnSetValueMultiplier;
        }

        private void OnSendMoney(int value)
        {
            _money = value;
        }

        private void OnSetScore(int setScore)
        {
            _scoreCache = (setScore * _valueMultiplier) + _gateScoreValue;
            PlayerSignals.Instance.onSetTotalScore?.Invoke(_scoreCache);
        }

        private void OnSetAtmScore(int atmValues)
        {
            _gateScoreValue += atmValues * _valueMultiplier;
            //AtmSignals.Instance.onSetAtmScoreText?.Invoke(_atmScoreValue);
        }

        private void OnSetValueMultiplier()
        {
            _valueMultiplier = CoreGameSignals.Instance.onGetFireRateLevel();
        }

        private void UnSubscribeEvents()
        {
            ScoreSignals.Instance.onSendMoney -= OnSendMoney;
            ScoreSignals.Instance.onGetMoney -= () => _money;
            ScoreSignals.Instance.onSetScore -= OnSetScore;
            ScoreSignals.Instance.onSetGateScore -= OnSetAtmScore;
            CoreGameSignals.Instance.onMiniGameStart -=
                () => ScoreSignals.Instance.onSendFinalScore?.Invoke(_scoreCache);
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onLevelSuccessful -= RefreshMoney;
            CoreGameSignals.Instance.onLevelFailed -= RefreshMoney;
            UISignals.Instance.onClickInitYear -= OnSetValueMultiplier;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Start()
        {
            OnSetValueMultiplier();
            RefreshMoney();
        }

        private int GetMoneyValue()
        {
            if (!ES3.FileExists()) return 0;
            return (int)(ES3.KeyExists("Money") ? ES3.Load<int>("Money") : 0);
        }

        private void RefreshMoney()
        {
            _money += (int)(_scoreCache * ScoreSignals.Instance.onGetMultiplier());
            UISignals.Instance.onSetMoneyValue?.Invoke(_money);
        }

        private void OnReset()
        {
            _scoreCache = 0;
            _gateScoreValue = 0;
        }
    }
}