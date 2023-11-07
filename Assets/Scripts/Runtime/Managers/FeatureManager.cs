using Runtime.Commands.Feature;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class FeatureManager : MonoBehaviour
    {
         #region Self Variables

        #region Public Variables

        public FeatureManager()
        {
            _onClickFireRateCommand = new OnClickFireRateCommand(this , ref _newPriceTag, ref _fireRateLevel);
            _onClickInitYearCommand = new OnClickInitYearCommand(this, ref _newPriceTag, ref _initYearLevel);
        }

        #endregion

        #region Private Variables

         private byte _fireRateLevel = 1;
         private byte _initYearLevel = 1;
         private int _newPriceTag;

        private readonly  OnClickFireRateCommand _onClickFireRateCommand;
        private readonly OnClickInitYearCommand _onClickInitYearCommand;

        #endregion

        #endregion

        private void Awake()
        {
            _fireRateLevel = LoadIncomeData();
            _initYearLevel = LoadStackData();
        }

        private void OnEnable()
        {
            SubsribeEvents();
        }

        private void SubsribeEvents()
        {
            UISignals.Instance.onClickFireRate += _onClickFireRateCommand.Execute;
            UISignals.Instance.onClickInitYear += _onClickInitYearCommand.Execute;
            CoreGameSignals.Instance.onGetFireRateLevel += OnGetIncomeLevel;
            CoreGameSignals.Instance.onGetInitYearLevel += OnGetStackLevel;
        }

        private byte OnGetStackLevel() => _initYearLevel;

        private byte OnGetIncomeLevel() => _fireRateLevel;
        

        private void UnSubsribeEvents()
        {
            UISignals.Instance.onClickFireRate -= _onClickFireRateCommand.Execute;
            UISignals.Instance.onClickInitYear -= _onClickInitYearCommand.Execute;
            CoreGameSignals.Instance.onGetFireRateLevel -= OnGetIncomeLevel;
            CoreGameSignals.Instance.onGetInitYearLevel -= OnGetStackLevel;
        }
        private void OnDisable()
        {
            UnSubsribeEvents();
        }

        private byte LoadIncomeData()
        {
            if (!ES3.FileExists()) return 1;
            return (byte)(ES3.KeyExists("IncomeLevel") ? ES3.Load<int>("IncomeLevel") : 1);

        }
        private byte LoadStackData()
        {
            if (!ES3.FileExists()) return 1;
            return (byte)(ES3.KeyExists("StackLevel") ? ES3.Load<int>("StackLevel") : 1);
        }

        internal void SaveFeatureData()
        {
            SaveSignals.Instance.onSaveGameData?.Invoke();
        }
    }
}