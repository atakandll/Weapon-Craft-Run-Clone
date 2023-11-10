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
        public static UIManager Instance { get; private set; }

        [Header("Starting Hud")] [SerializeField]
        GameObject startingHud;

        bool canHideStartingUI;
        [SerializeField] Button startButton, fireRateButton, inityearButton;

        [SerializeField] Image fingerImage;
        [SerializeField] float leftRightMovement;
        [SerializeField] float animTime;

        [Tooltip("SettingButton")] [SerializeField]
        Image slidingUI;

        [SerializeField] float moveValue;


        [Header("Game Hud")] public GameObject gameHud;

        [SerializeField] TMP_Text currentLevelText;

        [Header("GameHud Attributes")] [SerializeField]
        TMP_Text initYearNumber;

        [SerializeField] TMP_Text playerMoneyText,  reducerText;
        [SerializeField] float reducerMoveValue, reducerMoveDur;
        [SerializeField] Vector2 reducerTextResetPos;

        [Header("End Hud")] public GameObject endHud;
        [SerializeField] GameObject initYearImage;

        [Header("Upgrades")] [SerializeField] TMP_Text fireRateLevelText;
        [SerializeField] TMP_Text fireRangeLevelText, incomeLevelText, initYearLevelText;
        [SerializeField] TMP_Text fireRateCostText, fireRangeCostText, incomeCostText, initYearCostText;

        [Header("Slider")] [SerializeField] Slider weaponSlider;
        [SerializeField] Image fillImage;
        [SerializeField] List<GameObject> blackandWhiteImages;
        [SerializeField] List<GameObject> coloredImages;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

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
            if (canHideStartingUI)
            {
                HideStartingUI();
            }
        }

        public void UpdateWeaponBar()
        {
            for (int i = 0; i < blackandWhiteImages.Count; i++)
            {
                blackandWhiteImages[i].SetActive(false);
            }

            for (int i = 0; i < coloredImages.Count; i++)
            {
                coloredImages[i].SetActive(false);
            }

            coloredImages[PlayerManager.Instance.WeaponIndex].SetActive(true);
            blackandWhiteImages[PlayerManager.Instance.WeaponIndex].SetActive(true);

            float fillValue = (float)PlayerManager.Instance.InitYear -
                              (float)PlayerManager.Instance.WeaponChoosingInitYearsLimit[
                                  PlayerManager.Instance.WeaponIndex];

            fillImage.fillAmount = (fillValue + 50) / (float)50;
        }

        public void OnSettingsButtonPressed()
        {

            if (slidingUI.color.a > 0)
            {
                slidingUI.rectTransform.DOAnchorPos(new Vector2(slidingUI.rectTransform.anchoredPosition.x,
                    slidingUI.rectTransform.anchoredPosition.y + moveValue), 0.4f);

                slidingUI.DOFade(0, 0.3f);
            }
            else if (slidingUI.color.a <= 0)
            {
                slidingUI.rectTransform.DOAnchorPos(new Vector2(slidingUI.rectTransform.anchoredPosition.x,
                    slidingUI.rectTransform.anchoredPosition.y - moveValue), 0.4f);
                slidingUI.DOFade(1, 0.3f);
            }
        }

        // STARTING HUD
        public void OnPlayButtonPressed()
        {
            startButton.interactable = false;
            GameManager.Instance.GameHasStarted = true;
            GameManager.Instance.CameraStateChange();
            canHideStartingUI = true;
        }

        private void HideStartingUI()
        {
            startButton.transform.SetParent(startingHud.transform, false);
            fireRateButton.transform.SetParent(startingHud.transform, false);
            inityearButton.transform.SetParent(startingHud.transform, false);

            startingHud.GetComponent<CanvasGroup>().alpha -= Time.deltaTime;
            if (startingHud.GetComponent<CanvasGroup>().alpha <= 0)
            {
                canHideStartingUI = false;
                startingHud.SetActive(false);
                fillImage.gameObject.SetActive(false);
            }
        }

        private void MoveFinger()
        {
            fingerImage.rectTransform.DOAnchorPos
                (new Vector3(fingerImage.rectTransform.anchoredPosition.x + leftRightMovement,
                    fingerImage.rectTransform.anchoredPosition.y), animTime)
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



            fireRateLevelText.text = "Level " + (PlayerManager.Instance.FireRateValueIndex + 1).ToString();
            initYearLevelText.text = "Level " + (PlayerManager.Instance.InitYearValueIndex + 1).ToString();

            fireRateCostText.text =
                "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRateValueIndex].ToString());
            initYearCostText.text =
                "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.InitYearValueIndex].ToString());
            UpdateInitYearText();
            UpdateMoneyText();
        }

        public void UpdateEndingHudTexts()
        {
            fireRangeLevelText.text = "Level " + (PlayerManager.Instance.FireRangeValueIndex + 1).ToString();
            incomeLevelText.text = "Level " + (PlayerManager.Instance.IncomeValueIndex + 1).ToString();

            fireRangeCostText.text =
                "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRangeValueIndex].ToString());
            incomeCostText.text = "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.IncomeValueIndex].ToString());

        }

        // Upgrades
        public void OnFireRateUpdatePressed()
        {

            if (PlayerManager.Instance.Money >= UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRateValueIndex])
            {
                PlayerManager.Instance.Money -= UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRateValueIndex];
                PlayerManager.Instance.FireRateValueIndex += 1;
                fireRateLevelText.text = "Level " + (PlayerManager.Instance.FireRateValueIndex + 1).ToString();
                fireRateCostText.text =
                    "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRateValueIndex].ToString());
                PlayerManager.Instance.SetUpgradedValues();
                UpdateMoneyText();
                PlayerManager.Instance.SavePlayerData();
            }
        }

        public void OnFireRangeUpdatePressed()
        {
            if (PlayerManager.Instance.Money >= UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRangeValueIndex])
            {
                PlayerManager.Instance.Money -= UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRangeValueIndex];
                PlayerManager.Instance.FireRangeValueIndex += 1;
                fireRangeLevelText.text = "Level " + (PlayerManager.Instance.FireRangeValueIndex + 1).ToString();
                fireRangeCostText.text =
                    "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.FireRangeValueIndex].ToString());
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
                initYearCostText.text =
                    "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.InitYearValueIndex].ToString());
                UpdateWeaponBar();
                UpdateMoneyText();
                PlayerManager.Instance.SetUpgradedValues();
                UpdateInitYearText();
                PlayerManager.Instance.SavePlayerData();

            }
        }

        public void OnIncomeUpdatePressed()
        {
            print("income");
            if (PlayerManager.Instance.Money >= UpgradeManager.Instance.Costs[PlayerManager.Instance.IncomeValueIndex])
            {
                PlayerManager.Instance.Money -= UpgradeManager.Instance.Costs[PlayerManager.Instance.IncomeValueIndex];
                PlayerManager.Instance.IncomeValueIndex += 1;
                incomeLevelText.text = "Level " + (PlayerManager.Instance.IncomeValueIndex + 1).ToString();
                incomeCostText.text =
                    "$" + (UpgradeManager.Instance.Costs[PlayerManager.Instance.IncomeValueIndex].ToString());
                UpdateMoneyText();
                PlayerManager.Instance.SetUpgradedValues();
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

            // spawn olark yapalım
            reducerText.gameObject.SetActive(true);
            reducerText.rectTransform.anchoredPosition = reducerTextResetPos;

            reducerText.rectTransform.DOAnchorPos(new Vector2(reducerText.rectTransform.anchoredPosition.x,
                    reducerText.rectTransform.anchoredPosition.y - reducerMoveValue), reducerMoveDur)
                .OnPlay(() => { reducerText.DOFade(0, reducerMoveDur); }).OnComplete(() =>
                {
                    reducerText.DOFade(255, reducerMoveDur);
                    reducerText.rectTransform.anchoredPosition = reducerTextResetPos;
                    reducerText.gameObject.SetActive(false);
                });
        }
    }
}