using System;
using System.Collections.Generic;
using DG.Tweening;
using Managers.Game;
using Managers.Player;
using Managers.Upgrades;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.UI
{
    public class UIManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public static UIManager Instance;


        #endregion

        #region Serialized Variables

        [Header("Start Panel")] [SerializeField]
        private GameObject startingHud;
        [SerializeField] private Button startButton, fireRateButton, initYearButton;
        [SerializeField] private Image fingerImage;
        [SerializeField] private float leftRightMovement;
        [SerializeField] private float animTime;
        [SerializeField] private Image slidingUI;
        [SerializeField] private float moveValue;

        [Header("Game Panel")] [SerializeField]
        private GameObject gameHud;

        [SerializeField] private TextMeshProUGUI initYearNumber;
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private TextMeshProUGUI playerMoneyText, reducerText;
        [SerializeField] private float reducerMoveValue, reducerMoveDur;
        [SerializeField] private Vector2 reducerTextResetPos;

        [Header("End Game Panel")] [SerializeField]
        private GameObject endHud;
        [SerializeField] GameObject initYearImage;
        
        [Header("Upgrades")]
        [SerializeField] private TextMeshProUGUI fireRateLevelText;
        [SerializeField] private TextMeshProUGUI fireRangeLevelText, incomeLevelText, initYearLevelText;
        [SerializeField] private TextMeshProUGUI fireRateCostText, fireRangeCostText, incomeCostText, initYearCostText;


        [Header("Slider")] [SerializeField] private Slider weaponSlider;
        [SerializeField] private Image fillImage;
        [SerializeField] private List<GameObject> blackAndWhiteImages;
        [SerializeField] private List<GameObject> coloredImages;

        #endregion

        #region Private Variables

        private bool _canHideStartingUI;

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
        }

        #endregion

        private void Start()
        {
            MoveFinger();
            UpdateWeaponBar();
            reducerText.rectTransform.anchoredPosition = reducerTextResetPos;
            reducerText.gameObject.SetActive(false);
            UpdateStartingHudTexts();
        }

        private void Update()
        {
            if(_canHideStartingUI)
                HideStartingUI();
        }

        public void UpdateWeaponBar()
        {
            for (int i = 0; i < blackAndWhiteImages.Count; i++)
            {
                blackAndWhiteImages[i].SetActive(false);
            }

            for (int i = 0; i < coloredImages.Count; i++)
            {
                coloredImages[i].SetActive(false);
            }
            coloredImages[PlayerManager.Instance.WeaponIndex].SetActive(true);
            blackAndWhiteImages[PlayerManager.Instance.WeaponIndex].SetActive(true);

            float fillValue = PlayerManager.Instance.InitYear -
                              PlayerManager.Instance.WeaponChoosingInitYearsLimit[PlayerManager.Instance.WeaponIndex];

            fillImage.fillAmount = (fillValue + 50) / 50;
        }

        public void OnSettingsButtonPressed()
        {
            if(slidingUI.color.a > 0)
            {
                slidingUI.rectTransform.DOAnchorPos(new Vector2(slidingUI.rectTransform.anchoredPosition.x,
                    slidingUI.rectTransform.anchoredPosition.y + moveValue), 0.4f);
            
                slidingUI.DOFade(0,0.3f);
            }
            else if(slidingUI.color.a <= 0)
            {
                slidingUI.rectTransform.DOAnchorPos(new Vector2(slidingUI.rectTransform.anchoredPosition.x,
                    slidingUI.rectTransform.anchoredPosition.y - moveValue), 0.4f);
                slidingUI.DOFade(1,0.3f);
            }
        }

        public void OnPlayButtonPressed()
        {
            startButton.interactable = false;
            GameManager.Instance.GameHasStarted = true;
            GameManager.Instance.CameraStateChange();
            _canHideStartingUI = true;
        }

        private void HideStartingUI()
        {
            startButton.transform.SetParent(startingHud.transform, false);
            fireRateButton.transform.SetParent(startingHud.transform,false);
            initYearButton.transform.SetParent(startingHud.transform,false);

            startingHud.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;

            if (startingHud.GetComponent<CanvasGroup>().alpha <= 0)
            {
                _canHideStartingUI = false;
                startingHud.SetActive(false);
                fillImage.gameObject.SetActive(false);
            }
        }

        private void MoveFinger()
        {
            fingerImage.rectTransform.DOAnchorPos
                (new Vector3(fingerImage.rectTransform.anchoredPosition.x + leftRightMovement,
                    fingerImage.rectTransform.anchoredPosition.y),animTime)
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        public void UpdateInitYearText()
        {
            initYearNumber.text = PlayerManager.Instance.GetInGameInitYear().ToString();
        }

        public void UpdateMoneyText()
        {
            playerMoneyText.text = PlayerManager.Instance.Money.ToString();
        }

        public void UpdateStartingHudTexts()
        {
            currentLevelText.text = "Level " + (PlayerManager.Instance.CurrentLevelIndex + 1).ToString();
            initYearLevelText.text = "Level " + (PlayerManager.Instance.InitYearValueIndex + 1).ToString();
            
            fireRateCostText.text = "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRateValueIndex].ToString());
            initYearCostText.text = "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.InitYearValueIndex].ToString());
            UpdateInitYearText();
            UpdateMoneyText();
            
            
        }
        public void UpdateEndingHudTexts()
        {
            fireRangeLevelText.text = "Level " + (PlayerManager.Instance.FireRangeValueIndex + 1 ).ToString();
            incomeLevelText.text = "Level " + (PlayerManager.Instance.IncomeValueIndex + 1 ).ToString();

            fireRangeCostText.text = "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRangeValueIndex].ToString());
            incomeCostText.text = "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.IncomeValueIndex].ToString());

        }
        public void OnFireRateUpdatePressed()
        {
            if (PlayerManager.Instance.Money >=
                UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRateValueIndex])
            {
                PlayerManager.Instance.Money -=
                    UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRateValueIndex];
                
                PlayerManager.Instance.FireRateValueIndex += 1;
                fireRateLevelText.text = "Level " + (PlayerManager.Instance.FireRateValueIndex + 1).ToString();
                fireRateCostText.text = "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRateValueIndex].ToString());
                
                PlayerManager.Instance.SetUpgradedValues();
                UpdateMoneyText();
                PlayerManager.Instance.SavePlayerData();

                
            }
        }
        public void OnFireRangeUpdatePressed()
        {
            if (PlayerManager.Instance.Money >=
                UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRangeValueIndex])
            {
                PlayerManager.Instance.Money -=
                    UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRangeValueIndex];
                
                PlayerManager.Instance.FireRangeValueIndex += 1;
                fireRangeLevelText.text = "Level " + (PlayerManager.Instance.FireRangeValueIndex + 1).ToString();
                fireRangeCostText.text = "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRangeValueIndex].ToString());
                
                PlayerManager.Instance.SetUpgradedValues();
                UpdateMoneyText();
                PlayerManager.Instance.SavePlayerData();

                
            }
        }

        public void OnInitYearUpdatePressed()
        {
            if (PlayerManager.Instance.Money >=
                UpgradeManager.Instance.Costs[PlayerManager.Instance.InitYearValueIndex])
            {
                PlayerManager.Instance.Money -=
                    UpgradeManager.Instance.Costs[PlayerManager.Instance.InitYearValueIndex];
                PlayerManager.Instance.InitYearValueIndex += 1;
                initYearLevelText.text = "Level " + (PlayerManager.Instance.InitYearValueIndex + 1).ToString();
                initYearCostText.text = "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.InitYearValueIndex].ToString());
                
                UpdateWeaponBar();
                UpdateMoneyText();
                PlayerManager.Instance.SetUpgradedValues();
                UpdateInitYearText();
                PlayerManager.Instance.SavePlayerData();

            }
        }

        public void OnIncomeUpdatePressed()
        {
            if (PlayerManager.Instance.Money >=
                UpgradeManager.Instance.Costs[PlayerManager.Instance.IncomeValueIndex])
            {
                PlayerManager.Instance.Money -=
                    UpgradeManager.Instance.Costs[PlayerManager.Instance.IncomeValueIndex];
                PlayerManager.Instance.IncomeValueIndex += 1;
                incomeLevelText.text = "Level " + (PlayerManager.Instance.IncomeValueIndex + 1).ToString();
                incomeCostText.text = "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.IncomeValueIndex].ToString());
                
                PlayerManager.Instance.SetUpgradedValues();
                UpdateMoneyText();
                PlayerManager.Instance.SavePlayerData();

                
            }
            
        }
        public void OnContinueButtonPressed()
        {
            GameManager.Instance.LoadNextScene();
        }

        public void FinishHud()
        {
            endHud.SetActive(true);
            fillImage.gameObject.SetActive(false);
            initYearImage.SetActive(false);
            UpdateEndingHudTexts();
        }

        public void DisplayInitYearReduce()
        {
            reducerText.gameObject.SetActive(true);
            reducerText.rectTransform.anchoredPosition = reducerTextResetPos;
            
            reducerText.rectTransform.DOAnchorPos(new Vector2(reducerText.rectTransform.anchoredPosition.x,
                    reducerText.rectTransform.anchoredPosition.y - reducerMoveValue),reducerMoveDur).
                OnPlay(() => {reducerText.DOFade(0,reducerMoveDur);}).
                OnComplete(() => 
                {
                    reducerText.DOFade(255,reducerMoveDur);
                    reducerText.rectTransform.anchoredPosition = reducerTextResetPos;
                    reducerText.gameObject.SetActive(false);
                });
        }
       
            
    }
}