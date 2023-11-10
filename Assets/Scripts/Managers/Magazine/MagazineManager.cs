using System;
using System.Collections.Generic;
using DG.Tweening;
using Managers.Game;
using Managers.Gates;
using UnityEngine;

namespace Managers.Magazine
{
    public class MagazineManager : MonoBehaviour
    {
        #region Self Variables

        #region  Public Variables

        public bool canTravel;
        public Transform travelPos;



        #endregion

        #region Serialized Variables
        
        [SerializeField] private int capacity = 6;
        [SerializeField] private List<GameObject> bulletsInClip;
        [SerializeField] private List<Transform> vfxPlayTransforms;
        [SerializeField] List<Vector3> rotationValues;
        [SerializeField] ParticleSystem loadVFX;
        [SerializeField] private float rotationDuration;
        [SerializeField] private bool isRotating;
        [SerializeField] private float moveSpeedNextGate;
        [SerializeField] private GameObject playerDetector, playerCollider;
        [SerializeField]  private float slidingGateDOMoveDur;
        [SerializeField] private Vector3 slidingGatePosRot = new Vector3(270, 0, 0);
        [SerializeField] GameObject clip;
        
        #endregion

        #region Private Variables

        private int numOfBulletsInClip;
        private bool isMovingLeftPlatform;
        
        #endregion

        #endregion

         public void MagazineShot()
    {
        bulletsInClip[numOfBulletsInClip].SetActive(true);
        loadVFX.transform.position = vfxPlayTransforms[numOfBulletsInClip].transform.position;
        loadVFX.Play();
        transform.DORotate(rotationValues[numOfBulletsInClip],rotationDuration,RotateMode.Fast);
       
        numOfBulletsInClip +=1;

        if(numOfBulletsInClip >= capacity)
        {
            MoveTowardsLeftPlatform();
        }

    }
    private void Update() 
    { 
        if( isMovingLeftPlatform && tag == "MovingToLeftPlatformMagazine")
        {
            playerDetector.SetActive(false);
            playerCollider.SetActive(false);
            transform.position = new Vector3(transform.position.x- moveSpeedNextGate * Time.deltaTime, 
                transform.position.y, transform.position.z);
        }
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("LeftPlatform") && tag == "MovingToLeftPlatformMagazine")
        {
            MoveTowardsNextGate(other);
        }
        if (other.CompareTag("SlidingGate") && tag == "MovingToNextGateMagazine")
        {
            SlidingGateInteraction(other);
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.CompareTag("LeftPlatform") && tag == "MovingToNextGateMagazine")
        {
            MoveTowardsNextGate(other);
        }
    }

    private void MoveTowardsNextGate(Collider other)
    {
        
        isMovingLeftPlatform = false;

        tag = "MovingToNextGateMagazine";
        transform.position = new Vector3(other.transform.position.x, 
                transform.position.y, transform.position.z + moveSpeedNextGate * Time.deltaTime);

    }

    public void MoveTowardsLeftPlatform()
    {
        tag = "MovingToLeftPlatformMagazine";
        isMovingLeftPlatform = true;
    }

    private void SlidingGateInteraction(Collider other)
    {
        tag = "Magazine";
        isMovingLeftPlatform = false;
        Vector3 positionToMove = other.GetComponent<SlidingGates>().BucketTransform.position;
        transform.DOMove(positionToMove, slidingGateDOMoveDur);
        transform.DORotate(slidingGatePosRot,slidingGateDOMoveDur).OnComplete(UnloadMagazine);
        for (int i = 0; i < bulletsInClip.Count; i++)
        {
            if(!bulletsInClip[i].activeSelf) break;
            other.GetComponent<SlidingGates>().BulletsInBucket.Add(bulletsInClip[i]);
        }
        
        other.GetComponent<SlidingGates>().EqualizeLists();
    }

    private void UnloadMagazine()
    {
        for (int i = 0; i < bulletsInClip.Count; i++)
        {
            if(!bulletsInClip[i].activeSelf) break;
            bulletsInClip[i].transform.parent = null;
            bulletsInClip[i].GetComponent<Rigidbody>().useGravity = true;
            bulletsInClip[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        clip.SetActive(false);
    }

    public void MagazineTravel()
    {
        transform.DOMove(travelPos.position,GameManager.Instance.MagazineTravelDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
        
    }
}