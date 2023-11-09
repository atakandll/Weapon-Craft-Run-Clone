using TMPro;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private TextMeshPro yearText;

        #endregion

        #endregion
        
        public void SetTotalYear(int yearValue)
        {
            yearText.text = yearValue.ToString();
        }
        
    }
}