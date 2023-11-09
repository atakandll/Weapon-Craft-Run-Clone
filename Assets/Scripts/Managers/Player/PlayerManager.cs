using System.Collections.Generic;
using DG.Tweening;
using Managers.Game;
using Managers.Gates;
using Managers.Magazine;
using Managers.Obstacles;
using Managers.UI;
using Managers.Upgrades;
using Managers.Weapon;
using UnityEngine;

namespace Managers.Player
{
    public class PlayerManager : MonoBehaviour
    {
        #region Singleton
        
        public static PlayerManager Instance;

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

        #region Self Variables

        #region Public Variables

        public bool KnockBacked;

        public int InitYear;
        public float Income = 1;
        public float FireRate, FireRange;

        public List<int> WeaponChoosingInitYearsLimit;

        public int FireRateValueIndex;
        public int FireRangeValueIndex, InitYearValueIndex, IncomeValueIndex;
        public int Money;
        public int CurrentLevelIndex;
        public float CurrentPlayerDamage;
        [HideInInspector] public int WeaponIndex;
        

        #endregion

        #region Serialized Variables
        
        [SerializeField] private float forwardMoveSpeed;
        [SerializeField] private float negativeLimitValue, positiveLimitValue,maxSwerveAmountPerFrame;
        
        [SerializeField] float knockbackValue = 10f ;
        [SerializeField] float knockbackDur = 0.4f;
        [SerializeField] float slowMovSpeed;
        [SerializeField] float fastMovSpeed;
        [SerializeField] float originalMoveSpeed;
        
        [SerializeField] GameObject currentWeapon;
        
        [SerializeField] List<GameObject> weapons;
        
        [SerializeField] float playerDamage;
        
        [SerializeField] Vector3 deathEndValue;
        [SerializeField] float deathDur;
        
        [SerializeField] private float maxDisplacement = 0.2f;
        [SerializeField] private float maxPositionX , negativeMaxPositionX;
        
        [SerializeField] float moveSensivity;
        

        #endregion

        #region Private Variables

        #region Components

        private CapsuleCollider _capsuleCollider;
        private Rigidbody _rigidbody;
        private BoxCollider _boxCollider;

        #endregion

        private float _lastXPos;
        private int _inGameInitYear;
        private float _inGameFireRate, _inGameFireRage;
        private Vector2 _anchorPosition;



        #endregion

        #endregion

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _boxCollider = GetComponent<BoxCollider>();
            
            currentWeapon = GameObject.FindGameObjectWithTag("Weapon");
            tag = "Player";

            originalMoveSpeed = forwardMoveSpeed;
            


        }

        private void Update()
        {
            if (!GameManager.Instance.GameHasStarted) return;
            if (GameManager.Instance.GameHasEnded) return;

            if (!GameManager.Instance.GameHasEnded)
            {
                if (!KnockBacked)
                {
                    var inputX = GetInput();
                    
                    var displacementX = GetDisplacement(inputX);
                    
                    displacementX = SmootOutDisplacement(displacementX);
                    
                    var newPosition = GetNewLocalPosition(displacementX);

                    newPosition = GetLimitedLocalPosition(newPosition);

                    transform.localPosition = newPosition;
                }
            }
        }
        private float GetInput()
        {
            var inputX = 0f;

            if (Input.GetMouseButtonDown(0))
            {
                _anchorPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                inputX = (Input.mousePosition.x - _anchorPosition.x);
                _anchorPosition = Input.mousePosition;
            }
            return inputX;
        }
        private float GetDisplacement(float inputX)
        {
            var displacementX = 0f;
            displacementX = inputX * Time.deltaTime;
            return displacementX;

        }
        private float SmootOutDisplacement(float displacementX)
        {
            return Mathf.Clamp(displacementX, -maxDisplacement, maxDisplacement);
        }
        private Vector3 GetNewLocalPosition(float displacementX)
        {
            var lastPosition = transform.localPosition;
            var newPositionX = lastPosition.x + displacementX * moveSensivity;
            var newPosition = new Vector3(newPositionX, lastPosition.y, lastPosition.z + forwardMoveSpeed * Time.deltaTime);
            return newPosition;
        }

        private Vector3 GetLimitedLocalPosition(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, -negativeMaxPositionX, maxPositionX);
            return position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("MovementSlower"))
            {
                SetMovementSpeed(slowMovSpeed);
            }
            else if (other.CompareTag("MovementFaster"))
            {
                SetMovementSpeed(fastMovSpeed);
            }
            else if (other.CompareTag("Obstacle"))
            {
                KnockbackPlayer();
                other.tag = "Untagged";
            }
            else if (other.CompareTag("Enemy"))
            {
                KnockbackPlayer();
                other.tag = "TouchedEnemy";
            }
            else if (other.CompareTag("EnemyPlayerDetector"))
            {
                other.GetComponent<EnemyManager>().CanShoot = true;
            }
            else if (other.CompareTag("Chest"))
            {
                GameManager.Instance.EndLevel();
            }
            else if (other.CompareTag("MagazinesPlayerCollider"))
            {
                other.transform.parent.GetComponent<MagazineManager>().MoveTowardsLeftPlatform();
            }
            else if (other.CompareTag("Money"))
            {
                IncrementMoney(other.GetComponent<Money>().value);
                Destroy(other.gameObject);
            }
            else if (other.CompareTag("FirstSlidingGateCollider"))
            {
                IncrementInGameInitYear(other.transform.parent.parent.GetComponent<SlidingGates>().FirstLoadInitYear);

                other.transform.parent.parent.GetComponent<SlidingGates>()
                    .PlayLoadingAnim(other.transform.parent.parent.GetComponent<SlidingGates>().BulletsinFirstLoad);

                other.transform.parent.parent.GetComponent<SlidingGates>().LockAllGates();
            }
            else if(other.CompareTag("SecondSlidingGateCol"))
            {
                IncrementInGameInitYear(other.transform.parent.parent.GetComponent<SlidingGates>().SecondLoadInitYear);

                other.transform.parent.parent.GetComponent<SlidingGates>().
                    PlayLoadingAnim(other.transform.parent.parent.GetComponent<SlidingGates>().BulletinSecondLoad);

                other.transform.parent.parent.GetComponent<SlidingGates>().LockAllGates();
            }
            else if(other.CompareTag("ThirdSlidingGateCol"))
            {
                IncrementInGameInitYear(other.transform.parent.parent.GetComponent<SlidingGates>().ThirdLoadInitYear);

                other.transform.parent.parent.GetComponent<SlidingGates>().
                    PlayLoadingAnim(other.transform.parent.parent.GetComponent<SlidingGates>().BulletsinThirdLoad);

                other.transform.parent.parent.GetComponent<SlidingGates>().LockAllGates();
            }
            else if (other.CompareTag("FinishLine"))
            {
                GameManager.Instance.CameraStateChange();
            }
           
        }
        private void OnTriggerExit(Collider other) 
        {
            if(other.CompareTag("MovementSlower"))
            {
                SetMovementSpeed(originalMoveSpeed);
            }
            else if(other.CompareTag("MovementFaster"))
            {
                SetMovementSpeed(originalMoveSpeed);
            }
        }
      

        private void WeaponSelector()
        {
            if (_inGameInitYear <= WeaponChoosingInitYearsLimit[0] && currentWeapon != weapons[0])
            {
                for (int i = 0; i < weapons.Count; i++)
                {
                    weapons[i].SetActive(false);
                }

                currentWeapon = weapons[0];
                WeaponIndex = 0;
                currentWeapon.SetActive(true);
            }

            if (_inGameInitYear > WeaponChoosingInitYearsLimit[0] && InitYear <= WeaponChoosingInitYearsLimit[1] &&
                currentWeapon != weapons[1])
            {
                for (int i = 0; i < weapons.Count; i++)
                {
                    weapons[i].SetActive(false);
                }

                currentWeapon = weapons[1];
                WeaponIndex = 1;
                currentWeapon.SetActive(true);

            }
            
            if (_inGameInitYear > WeaponChoosingInitYearsLimit[1] && InitYear <= WeaponChoosingInitYearsLimit[2] &&
                currentWeapon != weapons[2])
            {
                for (int i = 0; i < weapons.Count; i++)
                {
                    weapons[i].SetActive(false);
                }

                currentWeapon = weapons[2];
                WeaponIndex = 2;
                currentWeapon.SetActive(true);

            }
            if (_inGameInitYear > WeaponChoosingInitYearsLimit[2] && InitYear <= WeaponChoosingInitYearsLimit[3] &&
                currentWeapon != weapons[3])
            {
                for (int i = 0; i < weapons.Count; i++)
                {
                    weapons[i].SetActive(false);
                }

                currentWeapon = weapons[3];
                WeaponIndex = 3;
                currentWeapon.SetActive(true);

            }

            currentWeapon.transform.parent = transform;
            
            UpdatePlayersDamage();
            
            GameManager.Instance.UpdatePlayerDamage();

        }

        public void KnockbackPlayer()
        {
            KnockBacked = true;
            IncrementInGameInitYear(-1);

            transform.DOMove
                    (new Vector3(transform.position.x,transform.position.y, transform.position.z - knockbackValue),knockbackDur).
                OnComplete(ResetKnockback);
            
            UIManager.Instance.UpdateInitYearText();
        }

        private void ResetKnockback()
        {
            KnockBacked = false;
        }

        public void PlayerDeath()
        {
            DOTween.Clear(currentWeapon.gameObject);
            currentWeapon.transform.DORotate(deathEndValue,deathDur,RotateMode.Fast);
            _boxCollider.isTrigger = false;
            _rigidbody.useGravity = true;
        }

        public void UpdatePlayersDamage()
        {
            CurrentPlayerDamage = playerDamage + currentWeapon.GetComponent<WeaponManager>().damage;
        }

        private void SetMovementSpeed(float newMoveSpeed)
        {
            forwardMoveSpeed = newMoveSpeed;
        }

        public void SetUpgradedValues()
        {
            InitYear = UpgradeManager.Instance.InitYearValues[InitYearValueIndex];
            FireRange = UpgradeManager.Instance.FireRangeValues[FireRangeValueIndex];
            FireRate = UpgradeManager.Instance.FireRateValues[FireRateValueIndex];
            Income = UpgradeManager.Instance.IncomeValues[IncomeValueIndex];
            
            SetStartingValues();
        }
        private void SetStartingValues()
        {
            _inGameFireRage = FireRange;
            _inGameFireRate = FireRate;
            _inGameInitYear = InitYear;
            WeaponSelector();
        }
        public int GetInGameInitYear()
        {
            return _inGameInitYear;
        }
        public float GetInGameFireRange()
        {
            return _inGameFireRage;
        }
        public float GetInGateFireRate()
        {
            return _inGameFireRate;
        }
        public void IncrementInGameFireRange(float value)
        {
            _inGameFireRage += value;
        }
        public void IncrementCurrentFireRate(float value)
        {
            float effectiveValue = value / 100;
            _inGameFireRate += effectiveValue;
        }
        public void IncrementInGameInitYear(int value)
        {
            if(value == -1) 
            {
                
                UIManager.Instance.DisplayInitYearReduce();
            }
            
            _inGameInitYear += value;
            
            UIManager.Instance.UpdateInitYearText();
            UIManager.Instance.UpdateWeaponBar();

            WeaponSelector();
        }
        public void IncrementMoney(int value)
        {
            Money += Mathf.RoundToInt(value * Income);
            
            UIManager.Instance.UpdateMoneyText();
        }

       
    }
}