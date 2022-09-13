using Code.Presenters;
using Code.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEditor;
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

     

#if UNITY_EDITOR
        public void AddStructure(GameObject gameObject)
        {
            StructureView structureView = gameObject.GetComponent<StructureView>();
            if (structureView == null)
            {
                Debug.LogError("There is no structure here");
                return;
            }
            if (structures.Any(structure => structure.GetComponent<StructureView>().ChunkSide == structureView.ChunkSide))
            {
                Debug.LogError("Already Added structure for this side.");
                return;
            }
            structures.Add(gameObject);
        }

        public void RemoveStructures()
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject,false);
            }
        }

    
        public void AddStructuresToList()
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                structures.Add(PrefabUtility.GetCorrespondingObjectFromOriginalSource(transform.GetChild(i).gameObject));
            }  
        }
#endif


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
            return transform.position.x + width / 2;
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
                var side = item.GetComponent<StructureView>().ChunkSide;
                item.transform.localPosition = side == StructureView.Side.Left ? new Vector2(-2.5f, -1.85f) : new Vector2(2.5f, -1.85f);
                instancedStructures.Add(item);
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