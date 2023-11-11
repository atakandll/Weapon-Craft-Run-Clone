using DG.Tweening;
using Managers.Game;
using TMPro;
using UnityEngine;

namespace Managers.Obstacles
{
    public class ChestManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private float health = 10;
        [SerializeField] private GameObject money;
        [SerializeField] private Transform moneySpawnPosition;
        [SerializeField] private int moneysValue;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Vector3 hitEffectScale;
        [SerializeField] private float hitEffectDur;
        
        #endregion

        #region Private Variables

        private Vector3 originalScale;

        #endregion

        #endregion

        private void Start() 
        {
            originalScale = healthText.transform.localScale;
            UpdateHealthText();        
        }
        public void TakeDamage(float dmg)
        {
            health -= dmg;
            UpdateHealthText();
            ObstacleHitEffect();
            
            if(health == 0)
            {
                GameObject spawnedMoney = Instantiate(money,moneySpawnPosition.position,Quaternion.identity);
                spawnedMoney.GetComponent<Money>().value = moneysValue;
                Destroy(gameObject);
            }
        }

        public void UpdateHealthText()
        {
            healthText.text = Mathf.RoundToInt(health).ToString();
        }

        private void ObstacleHitEffect()
        {
            healthText.transform.DOScale(hitEffectScale,hitEffectDur).OnComplete(ObstacleHitEffectReset);
        }

        private void ObstacleHitEffectReset()
        {
            healthText.transform.DOScale(originalScale,hitEffectDur);
        }

    }
}