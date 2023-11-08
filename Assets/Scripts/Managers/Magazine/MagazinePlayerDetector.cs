using UnityEngine;

namespace Managers.Magazine
{
    public class MagazinePlayerDetector : MonoBehaviour
    {
        
        [SerializeField] GameObject relatedMagazine;

        private void Start() 
        {
            relatedMagazine = transform.parent.gameObject;
        }
    
        private void OnTriggerEnter(Collider other) 
        {
            if(other.CompareTag("Player"))    
            {
                if(relatedMagazine.GetComponent<MagazineManager>().canTravel)
                {
                    relatedMagazine.GetComponent<MagazineManager>().MagazineTravel();
                }
            }
        }
    }
}