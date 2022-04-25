using System.Collections;
using UnityEngine;

namespace Assets.Code.Views.TargetSystem
{
    public class KnifeGenerator : MonoBehaviour
    {
        [SerializeField] KnifeView knifePrefab;
        [SerializeField] TargetView targetView;
        [SerializeField] Transform startPosition;
        [SerializeField] Transform endPosition;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            targetView.Swap();
            var knife = Instantiate<KnifeView>(knifePrefab, startPosition);
            knife.SetDestination(endPosition);
        }
    }
}