using DG.Tweening;
using Managers.Bullet;
using Managers.Player;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers.Gates
{
    public class GateManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public bool isGateActive;
        public BoxCollider boxCollider;

        

        #endregion

        #region Serialized Variables

        [SerializeField] private float positiveValues = 4.0f;
        [SerializeField] private float negativeValues = -4.0f;
        [SerializeField] private float gateValue;
        
        [SerializeField] Material redPrimaryMaterial;
        [SerializeField] Material greenPrimaryMaterial;
        
        [SerializeReference] Material greenSecondaryMat,redSecondaryMat;
        
        [Header("Visual")] 
        [SerializeField] TMP_Text gateOperatorText;
        [SerializeField] TMP_Text gateValueText;
        [Header("Hit Effect")]
        [SerializeField] Vector3 originalScale;
        [SerializeField] Vector3 hitEffectScale;
        [SerializeField] float hitEffectDur;
        [SerializeField] TMP_Text damageText;
        

        #endregion

        #region Private Variables
        
        private MeshRenderer meshRenderer;
        private bool fireRateGate, fireRangeGate, yearGate;
        private float damage;
        private BoxCollider[] gatesBoxColliders;

        #endregion


        #endregion

        private void Awake()
        {
            GetReferences();
        }

        private void GetReferences()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            isGateActive = true;
            tag = "Gate";
            ChooseOperation();
            UpdateGateText();
            DamageSelectionAndTextUpdate();
            originalScale = transform.localScale;
            if(transform.parent.tag == "GateManager")
            {
                if(transform.parent.GetComponentInChildren<BoxCollider>())
                {
                    gatesBoxColliders = transform.parent.GetComponentsInChildren<BoxCollider>();
                }
            }
            
        }

        private void DamageSelectionAndTextUpdate()
        {
            if (!yearGate)
            {
                int random = Random.Range(1, 3);
                damage = random;
                damageText.text = damage.ToString();
            }
            else if (yearGate)
            {
                int random = Random.Range(1, 2);
                damage = random;
                damageText.text = damage.ToString();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Bullet") && isGateActive)
            {
                if(!other.GetComponent<BulletManager>().enemyBullet)
                {
                    if(!other.GetComponent<BulletManager>().hasHit)
                    {
                        TakeDamage();
                        other.GetComponent<BulletManager>().hasHit = true;
                    }
                }
            
                Destroy(other.gameObject);
            }
            else if(other.CompareTag("Player") && isGateActive)
            {
                if(transform.parent.tag == "GateManager")
                {
                    foreach (var collider in gatesBoxColliders)
                    {   
                        collider.enabled = false;
                    }
                }
                if(fireRateGate)
                {
                    PlayerManager.Instance.IncrementCurrentFireRate(gateValue);
                }
                else if(fireRangeGate)
                {
                    PlayerManager.Instance.IncrementInGameFireRange(gateValue);
                }
                else if(yearGate)
                {
                    PlayerManager.Instance.IncrementInGameInitYear(Mathf.RoundToInt(gateValue));
                }
                gameObject.SetActive(false);
            }
        }

        private void UpdateTheColorOfGate(Material newPrimaryColor, Material newSecondaryColor)
        {
            var materials = meshRenderer.materials;
            materials[0].color = newPrimaryColor.color;
            materials[1].color = newSecondaryColor.color;

        }

        private void ChooseOperation()
        {
            int chooseRandom = Random.Range(0, 3);
            float valueRandom = Random.Range(negativeValues, positiveValues);
            float halfValueRand = RoundToClosestHalf(valueRandom);
            gateValue = halfValueRand;
            
            if(chooseRandom == 0)
            {
                fireRangeGate = true;
                gateOperatorText.text = "Fire Range";
            }
            else if(chooseRandom == 1)
            {
                fireRateGate = true;
                gateOperatorText.text = "Fire Rate";
            }
            else if(chooseRandom == 2)
            {
                yearGate = true;
                gateOperatorText.text = "Init Year";
                valueRandom = Random.Range(-5,4);
                gateValue = valueRandom;
            }

            if(gateValue >= 0)
            {
                if(meshRenderer.materials[0].color != greenPrimaryMaterial.color)
                {
                    UpdateTheColorOfGate(greenPrimaryMaterial,greenSecondaryMat);
                }
            }
            else if (gateValue < 0)
            {
                if(meshRenderer.materials[0].color != redPrimaryMaterial.color)
                {
                    UpdateTheColorOfGate(redPrimaryMaterial,redSecondaryMat);
                }
            }
        }

        private void TakeDamage()
        {
            gateValue += damage;

            if (yearGate)
            {
                gateValue = Mathf.Clamp(gateValue, -100, 50);
            }
            GateHitEffect();
            UpdateGateText();

            if (gateValue >= 0)
            {
                UpdateTheColorOfGate(greenPrimaryMaterial,greenSecondaryMat);
                
            }
            else if (gateValue < 0)
            {
                UpdateTheColorOfGate(redPrimaryMaterial,redSecondaryMat);
            }
        }
        private void UpdateGateText()
        {
            gateValueText.text = gateValue.ToString();
        }
        public float RoundToClosestHalf(float number)
        {
            float roundedValue = Mathf.Round(number * 2) / 2;
            return roundedValue;
        }
        private void GateHitEffect()
        {
            transform.DOScale(hitEffectScale,hitEffectDur).OnComplete(GateHitEffectReset);
        }

        private void GateHitEffectReset()
        {
            transform.DOScale(originalScale,hitEffectDur);
        }
        
    }
}