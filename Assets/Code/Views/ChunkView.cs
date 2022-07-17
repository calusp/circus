using Code.Presenters;
using Code.ScriptableObjects;
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
        public int Id { get; set; }
        public bool HasActionable => _hasActionable;
        public bool HasHazard => _hasHazzard;
        public float Witdh => width;
        public Actionable Actionable => _actionableView;
        public Hazard Hazard => _hazardViewView;

        public bool IsInitial => _isInitial;

        private Transform playerTransform;
        // Start is called before the first frame update
        void Awake()
        {
            Vector3 position = transform.position;
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
    }
}