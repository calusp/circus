using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Assets.Code.Views.TargetSystem
{
    public class KnifeGenerator : MonoBehaviour
    {
        [SerializeField] List<KnifeView> knifePrefab = new List<KnifeView>();
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
            var knife = Instantiate(knifePrefab[Random.Range(0,knifePrefab.Count)], startPosition) ;
            knife.SetDestination(endPosition);
        }
    }
}