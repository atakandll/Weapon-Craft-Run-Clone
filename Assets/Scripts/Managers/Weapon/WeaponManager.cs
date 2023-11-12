using System.Collections;
using DG.Tweening;
using Managers.Bullet;
using Managers.Game;
using Managers.Player;
using UnityEngine;

namespace Managers.Weapon
{
    public class WeaponManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public float damage;

        #endregion

        #region Serialized Variables
        
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform weaponType;
        [SerializeField] private float firedRotationDelay = 0.2f, resetRotationDelay = 0.8f;
        [SerializeField] private Vector3 fireEndRotationValue = new Vector3(315, 0, 0);
        [SerializeField] private Vector3 originalRotationValue;
        [SerializeField] private float fireRange;
        [SerializeField] private float fireRate;
        [SerializeField] private GameObject muzzleFlashVFX;

        #endregion

        #region Private Variables
        
        private float currentFireRate;
        private BulletManager bullet;
        
        #endregion

        #endregion
        

        private void Start() 
        {   
            tag = "Weapon";
        }

        private void ResetPos()
        {
            transform.DORotate(originalRotationValue,resetRotationDelay,RotateMode.Fast);
        }
        private void Update() 
        {
            if(!GameManager.Instance.GameHasStarted) return;
            if(GameManager.Instance.GameHasEnded) return;
        
            if(PlayerManager.Instance.KnockBacked)
            {
                UpdateFireRate();
                return;
            }
        
            currentFireRate -= Time.deltaTime;
        
            if(currentFireRate <= 0)
            {
                FireBullet();
                UpdateFireRate();
            }
        }
        public void FireBullet()
        {
            muzzleFlashVFX.SetActive(true);

            transform.DORotate(fireEndRotationValue,firedRotationDelay,RotateMode.Fast).
                OnComplete(ResetPos);
            

            GameObject firedBullet = Instantiate(bulletPrefab, weaponType.position ,Quaternion.identity);
        
            firedBullet.GetComponent<BulletManager>().firedPoint = weaponType;
            firedBullet.GetComponent<BulletManager>().SetRelatedWeapon(gameObject);
            StartCoroutine(MuzzleFlashoff());
        }
        IEnumerator MuzzleFlashoff()
        {
            yield return new WaitForSeconds(0.3f); 
            muzzleFlashVFX.SetActive(false);
        
        }
        public float GetWeaponsFireRange()
        {
            return PlayerManager.Instance.GetInGameFireRange() + fireRange;
        }
        private void UpdateFireRate()
        {
            currentFireRate = PlayerManager.Instance.GetInGateFireRate() + fireRate;
        }
        
    }
}