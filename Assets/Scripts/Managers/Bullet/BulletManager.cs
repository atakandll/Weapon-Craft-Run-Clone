using System;
using DG.Tweening;
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
               //
            }
            else if (enemyBullet)
            {
                //
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
        public void SetRelatedWeapon(GameObject newWeapon)
        {
            relatedWeapon = newWeapon;
        }
    }
}