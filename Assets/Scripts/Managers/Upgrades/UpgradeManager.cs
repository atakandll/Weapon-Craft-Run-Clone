using System.Collections.Generic;
using UnityEngine;

namespace Managers.Upgrades
{
    public class UpgradeManager : MonoBehaviour
    {
        
        #region Self Variables

        #region Public Variables
        
        public static UpgradeManager Instance;

        public List<float> IncomeValues;
        public List<int> InitYearValues;
        public List<float> FireRateValues;
        public List<float> FireRangeValues;
        public List<int> Costs;
        
        #endregion

        #region Serialized Variables

        [SerializeField] private float incomeStartValue, incomeIncreasingValue;
        [SerializeField] private int initYearStartValue, initYearIncreasingValue;
        [SerializeField] private float fireRateStartValue, fireRateIncreasingValue, secondFireRateIncreasingValue;
        [SerializeField] private int fireRateChangeLevelIndex;
        [SerializeField] private float fireRangeStartValue, fireRangeIncreasingValue;
        [SerializeField] private int costStartingValue, costIncrasingValue;
        
        #endregion

        #endregion
        #region Singleton

        

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            
            InitYearValues.Clear();
            Costs.Clear();
            FireRangeValues.Clear();
            FireRateValues.Clear();
            IncomeValues.Clear();
            
            SetIncomeValues();
            SetInitYearValues();
            SetFireRateValues();
            SetFireRangeValues();
            SetCostValues();
                
            
        }
        private void SetIncomeValues()
        {
            float firstValue = incomeStartValue;
            IncomeValues.Add(firstValue);

            for (int i = 1; i < 1000; i++)
            {
                float nextValue = IncomeValues[i - 1] + incomeIncreasingValue;
                nextValue = Mathf.Round(nextValue * 100f) / 100f;
                IncomeValues.Add(nextValue);
            }
        }
        private void SetInitYearValues()
        {
            int firstValue = initYearStartValue;
            InitYearValues.Add(firstValue);

            for (int i = 1; i < 1000; i++)
            {
                int valueNext = InitYearValues[i - 1] + initYearIncreasingValue;
                InitYearValues.Add(valueNext);

            }
        }
        private void SetFireRateValues()
        {
            float firstValue = fireRateStartValue;
            FireRateValues.Add(firstValue);

            for (int i = 1; i < fireRateChangeLevelIndex; i++)
            {
                float nextValue = FireRateValues[i - 1] - fireRateIncreasingValue;
                nextValue = Mathf.Round(nextValue * 100f) / 100f;
                FireRateValues.Add(nextValue);
            }

            for (int i = fireRateChangeLevelIndex; i < 1000; i++)
            {
                float nextValue = FireRateValues[i - 1] - secondFireRateIncreasingValue;
                nextValue = Mathf.Round(nextValue * 100f) / 100f;
                nextValue = Mathf.Clamp(nextValue, 0.04f, 5f);
                FireRateValues.Add(nextValue);
            }
        }
        private void SetFireRangeValues()
        {
            float firstValue = fireRangeStartValue;
            FireRangeValues.Add(firstValue);
           
            for (int i = 1; i < 1000; i++)
            {
                float valueNext = FireRangeValues[i - 1] + fireRangeIncreasingValue;
                FireRangeValues.Add(valueNext);
               
            }
        }
        private void SetCostValues()
        {
           int firstValue = costStartingValue;
           Costs.Add(firstValue);
           
           for (int i = 1; i < 1000; i++)
           {
               int valueNext = Costs[i - 1] + costIncrasingValue;
               Costs.Add(valueNext);
               
           }
        }

       

       

      

       

        #endregion
    }
}