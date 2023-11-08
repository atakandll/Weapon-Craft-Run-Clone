using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        #region Self Variables

        #region Public Variables

        public bool GameHasEnded;
        public bool GameHasStarted;

        public float playerDamage;
        
        public float EnemyFireRate;
        public float EnemyFireRange;

        public int NumOfBulletsInLoad;

        public GameObject MainCam, StartingCam;

        public GameObject EndSniper;
        public Vector3 RotationSpeed = new Vector3(100, 0, 0);

        public float MagazineTravelDuration;

        public List<GameObject> Levels;
        public int NumOfPresetLevels;
        public Transform LevelSpawnTransform;

        
        #endregion

        #region Private Variables

        private Vector3 camStartingPos;

        #endregion
        
        #endregion

        #region Singleton

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

        private void Update()
        {
            EndSniper.transform.Rotate(RotationSpeed * Time.deltaTime);
            
        }

        public void LevelChooser()
        {
            
        }
    }
}