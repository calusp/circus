using System.Collections;
using UnityEngine;

namespace Assets.Code.Views
{
    public class TargetView : MonoBehaviour
    {
        [SerializeField] GameObject targetFrontal;
        [SerializeField] GameObject targetSideways;
        // Use this for initialization
        void Start()
        {
            targetFrontal.SetActive(true);
            targetSideways.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Swap()
        {
            targetFrontal.SetActive(false);
            targetSideways.SetActive(true);
        }
    }
}