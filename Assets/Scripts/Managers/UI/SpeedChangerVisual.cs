using System;
using UnityEngine;

namespace Managers.UI
{
    public class SpeedChangerVisual : MonoBehaviour
    {

        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Material material;


        #endregion

        #region Private Variables

        private float value;


        #endregion

        #endregion

        private void Awake()
        {
            material = GetComponent<Renderer>().material;

        }

      

        private void Update()
        {
            value -= Time.deltaTime;
            material.mainTextureOffset = new Vector2(1, value);
        }
    }
}