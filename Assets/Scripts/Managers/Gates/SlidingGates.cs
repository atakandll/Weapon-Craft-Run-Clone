using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Managers.Game;
using UnityEngine;

namespace Managers.Gates
{
    public class SlidingGates : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public int FirstLoadInitYear, SecondLoadInitYear, ThirdLoadInitYear;
        public Transform BucketTransform;
        public List<GameObject> BulletsInBucket;
        public List<GameObject> ColoredImages;
        public List<GameObject> BlackAndWhiteImages;
        public List<GameObject> BulletsinFirstLoad, BulletinSecondLoad, BulletsinThirdLoad;

        #endregion

        #region Serialized Variables

        [SerializeField] private List<GameObject> bulletsInLoad;
        [SerializeField] private List<Transform> bulletInLoadTransform;
        [SerializeField] private int loadValue;
        [SerializeField] private BoxCollider firstBoxCollider, secondBoxCollider, thirdBoxCollider;
        [SerializeField] private Transform bulletTransform;
        [SerializeField] private List<Transform> lastBulletsSpawnTransforms;
        [SerializeField] private GameObject bullet;
        [SerializeField] private int bulletCounter = 0;
        [SerializeField] private float bulletGap;
        [SerializeField] private float moveDur = 0.1f;
        [SerializeField] Material greenMat;
        [SerializeField] GameObject FirstGate;
        [SerializeField] GameObject SecondGate;

        #endregion

        #region Private Variables

        private int currentCounter;

        #endregion

        #endregion

        private void Start()
        {
            firstBoxCollider.enabled = false;
            secondBoxCollider.enabled = false;
            thirdBoxCollider.enabled = false;
        }

        public void EqualizeLists()
        {
            if (BulletsInBucket != null)
            {
                var gameObjects = BulletsInBucket.Distinct().ToList();
            }

            for (int i = bulletCounter; i < BulletsInBucket.Count; i++)
            {
                if (BulletsInBucket.Count <= GameManager.Instance.NumOfBulletsInLoad)
                {
                    currentCounter++;

                    if (currentCounter >= 3)
                    {
                        LoadGate(bulletCounter);

                        ++bulletCounter;
                        currentCounter = 0;
                    }
                }
            }
        }
       
        public void LoadGate(int i)
        {
            if(i >= lastBulletsSpawnTransforms.Count)
            {
                Debug.Log("break load gate");
                return;
            }

            GameObject spawnedBullet = Instantiate(bullet,BucketTransform);
            spawnedBullet.transform.DOMove(lastBulletsSpawnTransforms[i].position,moveDur,false);
            bulletsInLoad.Add(spawnedBullet);

            if(bulletsInLoad.Count >= loadValue)
            {
                FirstGate.GetComponent<MeshRenderer>().material = greenMat;
        
                UnlockGate(firstBoxCollider,BlackAndWhiteImages[0],ColoredImages[0]);
                BulletsinFirstLoad.Add(bulletsInLoad[0]);
                BulletsinFirstLoad.Add(bulletsInLoad[1]); 
                BulletsinFirstLoad.Add(bulletsInLoad[2]); 
                BulletsinFirstLoad.Add(bulletsInLoad[3]); 
                BulletsinFirstLoad.Add(bulletsInLoad[4]); 

            }
            if(bulletsInLoad.Count >= loadValue * 2)
            {
                SecondGate.GetComponent<MeshRenderer>().material = greenMat;
                UnlockGate(secondBoxCollider,BlackAndWhiteImages[1],ColoredImages[1]);
                BulletinSecondLoad.Add(bulletsInLoad[5]);
                BulletinSecondLoad.Add(bulletsInLoad[6]); 
                BulletinSecondLoad.Add(bulletsInLoad[7]); 
                BulletinSecondLoad.Add(bulletsInLoad[8]); 
                BulletinSecondLoad.Add(bulletsInLoad[9]); 

            }
            if(bulletsInLoad.Count >= loadValue * 3)
            {
                UnlockGate(thirdBoxCollider,BlackAndWhiteImages[2],ColoredImages[2]);
                BulletsinThirdLoad.Add(bulletsInLoad[10]);
                BulletsinThirdLoad.Add(bulletsInLoad[11]); 
                BulletsinThirdLoad.Add(bulletsInLoad[12]); 
                BulletsinThirdLoad.Add(bulletsInLoad[13]); 
                BulletsinThirdLoad.Add(bulletsInLoad[14]);
            }
        }
        private void UnlockGate(BoxCollider collider,GameObject blackandWhiteImage,GameObject coloredImage)
        {
            collider.enabled = true;
            blackandWhiteImage.SetActive(false);
            coloredImage.SetActive(true);
        }
        public void PlayLoadingAnim(List<GameObject> newBulletsinLoad)
        {
            for (int i = 0; i < newBulletsinLoad.Count; i++)
            {
                float moveTransform = newBulletsinLoad[i].transform.position.y + 1f;
                newBulletsinLoad[i].transform.DOMoveY(moveTransform,0.2f);
                newBulletsinLoad[i].transform.DOScale(0,0.2f);
            }
            for (int i = 0; i < bulletsInLoad.Count; i++)
            {
                bulletsInLoad[i].GetComponent<Rigidbody>().useGravity = true;
                bulletsInLoad[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
        public void LockAllGates()
        {
            firstBoxCollider.enabled = false;
            secondBoxCollider.enabled = false;
            thirdBoxCollider.enabled = false;
        }
        
        

       
            

        
        

    }
}