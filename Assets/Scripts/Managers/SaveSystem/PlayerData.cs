using Managers.Player;
using UnityEngine;

namespace Managers.SaveSystem
{
    [System.Serializable]
    public class PlayerData
    {
        public int level;
        public int fireRangeValueIndex;
        public int fireRateValueIndex;
        public int initYearValueIndex, incomeValueIndex;
        public int money;


        public PlayerData(PlayerManager player)
        {
            level = player.CurrentLevelIndex;
            fireRateValueIndex = player.FireRateValueIndex;
            initYearValueIndex = player.InitYearValueIndex;
            incomeValueIndex = player.IncomeValueIndex;
            money = player.Money;
            fireRangeValueIndex = player.FireRangeValueIndex;
        }
    }
}