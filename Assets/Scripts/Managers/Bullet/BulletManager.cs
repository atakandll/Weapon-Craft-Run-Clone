using System;
using DG.Tweening;
using Managers.Game;
using Managers.Magazine;
using Managers.Obstacles;
using Managers.Player;
using Managers.Weapon;
using UnityEngine;

namespace Managers.Bullet
{
    public class BulletManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public Transform firedPoint;
        public bool hasHit;
        public bool enemyBullet;

        #endregion

        #region Serialized Variables

        [SerializeField] private float moveSpeed;
        [SerializeField] private GameObject relatedWeapon;
        [SerializeField] private Vector3 rotationValue;
        [SerializeField] private GameObject hitEffect;
        



        #endregion

        #region Private Variables
        
        private WeaponManager _relatedWeaponComponent;
        private BoxCollider _boxCollider;
        private Vector3 _firedPointCurrent;
        private float fireDistance;



        #endregion

        #endregion

        private void Start()
        {
            _firedPointCurrent = firedPoint.position;
            
            if (!enemyBullet)
            {
               fireDistance = relatedWeapon.GetComponent<WeaponManager>().GetWeaponsFireRange();
            }
            else if (enemyBullet)
            {
                fireDistance = GameManager.Instance.EnemyFireRange;
            }

            transform.DORotate(rotationValue, 0f);
        }

        private void Update()
        {
            if (!enemyBullet)
            {
                if (!(Vector3.Distance(_firedPointCurrent, transform.position) > fireDistance))
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y,
                        transform.position.z + moveSpeed * Time.deltaTime);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else if (enemyBullet)
            {
                if(!(Vector3.Distance(_firedPointCurrent,transform.position) > fireDistance ))
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, 
                        transform.position.z - moveSpeed * Time.deltaTime);
                }
                else
                {
                    Destroy(gameObject); 
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasHit) return;

            if (other.CompareTag("Untagged"))
            {
                PlayHitFX();
                Destroy(gameObject);
            }

            if (other.CompareTag("Chest"))
            {
                hasHit = true;
                if (!enemyBullet)
                {
                    PlayHitFX();
                    other.GetComponent<ChestManager>().TakeDamage(GameManager.Instance.playerDamage);
                    Destroy(gameObject);
                }
            }
            else if (other.CompareTag("SlidingGate"))
            {
                PlayHitFX();
                Destroy(gameObject);
            }
            else if (other.CompareTag("Magazine"))
            {
                if (!enemyBullet)
                {
                    PlayHitFX();
                    other.GetComponent<MagazineManager>().MagazineShot();
                }

                Destroy(gameObject);
                
            }
            else if (other.CompareTag("Player"))
            {
                if (enemyBullet)
                {
                    
                    PlayerManager.Instance.IncrementInGameInitYear(-1);
                    Destroy(gameObject);
                }
            }
            else if (other.CompareTag("Enemy"))
            {
                if (!enemyBullet)
                {
                    PlayHitFX();
                    other.GetComponent<EnemyManager>().TakeDamage(GameManager.Instance.playerDamage);
                    Destroy(gameObject);
                }
            }
            else if (other.CompareTag("TouchedEnemy"))
            {
                if (!enemyBullet)
                {
                    PlayHitFX();
                    other.GetComponent<EnemyManager>().TakeDamage(GameManager.Instance.playerDamage);
                    Destroy(gameObject);
                }
            }
        }

        private void PlayHitFX()
        {
            if (hitEffect != null)
            {
                GameObject hitfx = Instantiate(hitEffect, transform.position, Quaternion.identity);
                hitfx.GetComponent<ParticleSystem>().Play();
            }
        }
        public void SetRelatedWeapon(GameObject newWeapon)
        {
            relatedWeapon = newWeapon;
        }
    }
}