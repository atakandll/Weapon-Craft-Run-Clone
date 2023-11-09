using System.Collections.Generic;
using Cinemachine;
using Managers.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Managers.Game
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

        private void Start()
        {
            UpdatePlayerDamage();
            LevelChooser();
            EndSniper = GameObject.FindGameObjectWithTag("EndSniper");
            camStartingPos = MainCam.transform.localPosition;
        }

        private void Update()
        {
            EndSniper.transform.Rotate(RotationSpeed * Time.deltaTime);
            
        }

        public void LevelChooser()
        {
            if (PlayerManager.Instance.CurrentLevelIndex <= NumOfPresetLevels)
            {
                Instantiate(Levels[PlayerManager.Instance.CurrentLevelIndex], LevelSpawnTransform.position,
                    Quaternion.identity);
            }
            else
            {
                int levelRand = Random.Range(0, Levels.Count);
                Instantiate(Levels[levelRand], LevelSpawnTransform.position, Quaternion.identity);
            }
        }

        public void UpdatePlayerDamage()
        {
            playerDamage = PlayerManager.Instance.CurrentPlayerDamage;
        }

        public void EndLevel()
        {
            GameHasEnded = true;
            PlayerManager.Instance.PlayerDeath();
            
        }

        public void CameraStateChange()
        {
            if (StartingCam.activeSelf)
            {
                MainCam.GetComponent<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
                StartingCam.SetActive(false);
            }
            else
            {
                MainCam.GetComponent<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
                StartingCam.SetActive(true);
            }
        }

        public void LoadNextScene()
        {
            PlayerManager.Instance.CurrentLevelIndex++;
            SceneManager.LoadScene(0);
        }
    }
}