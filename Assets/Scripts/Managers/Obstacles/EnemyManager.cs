using System.Collections;
using Managers.Bullet;
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
        
        private Animator _animator;

        #endregion

        #endregion

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            currentFireRate = GameManager.Instance.EnemyFireRate;
            UpdateHealthText();
            SetAnimationsFloat(0);

        }

        private void Update()
        {
            if (!CanShoot) return;

            if (_animator.GetFloat("Speed") <= 0)
            {
                SetAnimationsFloat(1);
                FireBullet();

            }

            currentFireRate -= Time.deltaTime;

            if (currentFireRate <= 0)
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
            
            StartCoroutine(MuzzleFlashOff());
        }
        IEnumerator MuzzleFlashOff()
        {
            yield return new WaitForSeconds(0.3f); 
            muzzleFlashVFX.SetActive(false);
        
        }

        private void SetAnimationsFloat(float newAnimSpeed)
        {
            _animator.SetFloat("Speed",newAnimSpeed);
        }

        public void TakeDamage(float damage)
        {
            health -= damage;
            UpdateHealthText();

            if (health <= 0)
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