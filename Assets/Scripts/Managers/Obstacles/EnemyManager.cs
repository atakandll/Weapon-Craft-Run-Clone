using System.Collections;
using Managers.Bullet;
using Managers.Game;
using TMPro;
using UnityEngine;

namespace Managers.Obstacles
{
    public class EnemyManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public bool CanShoot;
        

        #endregion

        #region Serialized Variables
        
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firingPosition;
        
        [SerializeField] private float health;
        
        [SerializeField] GameObject muzzleFlashVFX;
        
        [SerializeField] private GameObject money;
        [SerializeField] private Transform moneyTransform;

        [SerializeField] private TMP_Text healthText;


        #endregion

        #region Private Variables

        private float currentFireRate;
        
        [SerializeField] private Animator animator;

        #endregion

        #endregion

       

        private void Start() 
        {
            animator = GetComponent<Animator>();
            currentFireRate = GameManager.Instance.EnemyFireRate;
            UpdateHealthText();
            SetAnimationsFloat(0);
        }

        private void Update() 
        {
            if(!CanShoot) return;

            if(animator.GetFloat("Speed") <= 0) 
            {
                SetAnimationsFloat(1);
                FireBullet();
            }
            currentFireRate -= Time.deltaTime;
        
            if(currentFireRate <= 0)
            {
                FireBullet();
            }
        }

        private void FireBullet()
        {
            muzzleFlashVFX.SetActive(true);
            GameObject firedBullet = Instantiate(bulletPrefab, firingPosition.position, Quaternion.identity);

            firedBullet.GetComponent<BulletManager>().SetRelatedWeapon(gameObject);
            firedBullet.GetComponent<BulletManager>().firedPoint = firingPosition;
            firedBullet.GetComponent<BulletManager>().enemyBullet = true;
            currentFireRate = GameManager.Instance.EnemyFireRate;
            StartCoroutine(MuzzleFlashoff());
        }

        IEnumerator MuzzleFlashoff()
        {
            yield return new WaitForSeconds(0.3f); // Wait for 2 seconds
            muzzleFlashVFX.SetActive(false);
        
        }
        private void SetAnimationsFloat(float newAnimSpeed)
        {
            animator.SetFloat("Speed",newAnimSpeed);
        }

        public void TakeDamage(float damage)
        {
            print("TakeDamage");
            health -= damage;
            UpdateHealthText();
            if(health <= 0)
            {

                Instantiate(money,moneyTransform.position,Quaternion.identity);

                Destroy(gameObject);
            
            }
       
        }

        private void UpdateHealthText()
        {
            healthText.text = health.ToString();
        }
    }
}