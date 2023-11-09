using System;
using Runtime.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class ShopPanelController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TextMeshProUGUI fireRateLvlText;
        [SerializeField] private Button fireRateButton;
        [SerializeField] private TextMeshProUGUI fireRateValue;
        [SerializeField] private TextMeshProUGUI InitYearLvlText;
        [SerializeField] private Button initYearButton;
        [SerializeField] private TextMeshProUGUI initYearValue;
        

        #endregion

        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
           UISignals.Instance.onSetFireRateText += OnSetFireRateText;
           UISignals.Instance.onSetInitYearText += OnSetInitYearText;
        }

        private void OnSetInitYearText()
        {
            InitYearLvlText.text = "Init Year lvl\n" + CoreGameSignals.Instance.onGetInitYearLevel();
            initYearValue.text = (Mathf.Pow(2, Mathf.Clamp(CoreGameSignals.Instance.onGetInitYearLevel(), 0, 10)) * 100)
                .ToString();
        }

        private void OnSetFireRateText()
        {
            fireRateLvlText.text = "Fire Rate lvl\n" + CoreGameSignals.Instance.onGetFireRateLevel();
            fireRateValue.text = (Mathf.Pow(2, Mathf.Clamp(CoreGameSignals.Instance.onGetFireRateLevel(), 0, 10)) * 100)
                .ToString();
        }

        private void UnSubscribeEvents()
        {
            UISignals.Instance.onSetFireRateText -= OnSetFireRateText;
            UISignals.Instance.onSetInitYearText -= OnSetInitYearText;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        private void Start()
        {
            SyncShopUi();
        }

        private void SyncShopUi()
        {
            OnSetFireRateText();
            OnSetInitYearText();
            ChangesFireRateInteractable();
            ChangesInitYearInteractable();
        }

        private void ChangesFireRateInteractable()
        {
            if (int.Parse(UISignals.Instance.onGetMoneyValue?.Invoke().ToString()!) < int.Parse(fireRateValue.text) ||
                CoreGameSignals.Instance.onGetFireRateLevel() >= 30)
            {
                fireRateButton.interactable = false;
            }

            else
            {
                fireRateButton.interactable = true;
            }
        }

        private void ChangesInitYearInteractable()
        {
            if (int.Parse(UISignals.Instance.onGetMoneyValue?.Invoke().ToString()!) < int.Parse(initYearValue.text) ||
                CoreGameSignals.Instance.onGetInitYearLevel() >= 15)
            {
                initYearButton.interactable = false;
            }
            else
            {
                initYearButton.interactable = true;
            }
        }
    }
}