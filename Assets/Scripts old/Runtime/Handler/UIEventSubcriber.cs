using System;
using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Handler
{
    public class UIEventSubcriber : MonoBehaviour
    {
         #region Self Variables

        #region Serialized Variables

        [SerializeField] private UIEventSubscriptionTypes type;
        [SerializeField] private Button button;

        #endregion

        #region Private Variables

         private UIManager _manager;

        #endregion

        #endregion

        private void Awake()
        {
            FindReferences();
        }

        private void FindReferences()
        {
            _manager = FindObjectOfType<UIManager>();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            switch (type)
            {
                case UIEventSubscriptionTypes.OnPlay:
                {
                    button.onClick.AddListener(_manager.OnPlay);
                    break;
                }
                case UIEventSubscriptionTypes.OnNextLevel:
                {
                    button.onClick.AddListener(_manager.OnNextLevel);
                    break;
                }
                case UIEventSubscriptionTypes.OnRestartLevel:
                {
                    button.onClick.AddListener(_manager.OnRestartLevel);
                    break;
                }
                case UIEventSubscriptionTypes.OnIncreaseFireRate:
                {
                    button.onClick.AddListener(_manager.OnFireRateUpdate);
                    break;
                }
                case UIEventSubscriptionTypes.OnIncreaseInitYear:
                {
                    button.onClick.AddListener(_manager.OnInitYearUpdate);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void UnsubscribeEvents()
        {
            switch (type)
            {
                case UIEventSubscriptionTypes.OnPlay:
                {
                    button.onClick.RemoveListener(_manager.OnPlay);
                    break;
                }
                case UIEventSubscriptionTypes.OnNextLevel:
                {
                    button.onClick.RemoveListener(_manager.OnNextLevel);
                    break;
                }
                case UIEventSubscriptionTypes.OnRestartLevel:
                {
                    button.onClick.RemoveListener(_manager.OnRestartLevel);
                    break;
                }
                case UIEventSubscriptionTypes.OnIncreaseFireRate:
                {
                    button.onClick.RemoveListener(_manager.OnFireRateUpdate);
                    break;

                }
                case UIEventSubscriptionTypes.OnIncreaseInitYear:
                {
                    button.onClick.RemoveListener(_manager.OnInitYearUpdate);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}