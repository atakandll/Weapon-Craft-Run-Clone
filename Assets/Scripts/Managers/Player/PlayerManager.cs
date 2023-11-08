using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
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
        public int WeaponIndex;
        

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

        private Vector3 GetLimitedLocalPosition(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, -negativeMaxPositionX, maxPositionX);
            return position;
        }

        private Vector3 GetNewLocalPosition(float displacementX)
        {
            var lastPosition = transform.localPosition;
            var newPositionX = lastPosition.x + displacementX * moveSensivity;
            var newPosition = new Vector3(newPositionX, lastPosition.y, lastPosition.z + forwardMoveSpeed * Time.deltaTime);
            return newPosition;
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
    }
}