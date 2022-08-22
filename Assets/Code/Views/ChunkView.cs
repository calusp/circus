using Code.Presenters;
using Code.ScriptableObjects;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Code.Views
{
    public class ChunkView : MonoBehaviour
    {
        [SerializeField] private BaseActionableView _actionableView;
        [SerializeField] private BaseHazardView _hazardViewView;
        [SerializeField] private GameConfiguration gameConfiguration;
        [SerializeField] private bool _hasActionable;
        [SerializeField] private bool _hasHazzard;
        [SerializeField] private float width = 20;
        [SerializeField] private DisplayableData distanceData;
        [SerializeField] private SharedGameState sharedGameState;
        [SerializeField] private bool _isInitial;
        [SerializeField] private List<GameObject> structures = new List<GameObject>();
        public int Id { get; set; }
        public bool HasActionable => _hasActionable;
        public bool HasHazard => _hasHazzard;
        public float Witdh => width;
        public Actionable Actionable => _actionableView;

        public bool IsInitial => _isInitial;

        private Transform playerTransform;

        private List<GameObject> instancedStructures = new List<GameObject>();
        // Start is called before the first frame update
        void Awake()
        {
            playerTransform = GameObject.Find("Player(Clone)").transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (sharedGameState.JustDied) return;
        }

        public float GetRightBound()
        {
            return transform.position.x + width/2;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void SetPosition(float positionX)
        {
            transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
        }

        private void OnEnable()
        {
            instancedStructures = new List<GameObject>();
            foreach (var structure in structures)
            {

                GameObject item = Instantiate(structure, transform);
                var side = item.GetComponent<StructureView>().GetSide;
                item.transform.localPosition = side == StructureView.Side.Left ? new Vector2(-2.5f, -1.85f) : new Vector2(2.5f, -1.85f);
                instancedStructures.Add(item) ;
            }
        }

        private void OnDisable()
        {
            foreach (var structure in instancedStructures)
            {
                Destroy(structure);
            }
        }
    }
}