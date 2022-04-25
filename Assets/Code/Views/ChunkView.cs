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

        private Transform playerTransform;
        private float prevDistance;
        private float _increments;
        private Vector3 target;
        // Start is called before the first frame update
        void Awake()
        {
            Vector3 position = transform.position;
            target = new Vector3(position.x - width, position.y, position.z);
            playerTransform = GameObject.Find("Player(Clone)").transform;
            MoveChunk();
            prevDistance = position.x - width / 2 + (playerTransform.position.x + 5) + 36.5f;
        }

        // Update is called once per frame
        void Update()
        {
            if (sharedGameState.JustDied) return;
            MoveChunk();
            SaveDistanceMoved();
        }

        private void SaveDistanceMoved()
        {
            var distance = (transform.position.x - width / 2) + (playerTransform.position.x + 5);
            if (distance > 0)
                distanceData.Content += Mathf.Clamp(Mathf.Abs(prevDistance) - Mathf.Abs(distance), 0, 1);
            prevDistance = distance;
        }

        private void MoveChunk()
        {
            var speed = gameConfiguration.CameraSpeed + gameConfiguration.CalculateIncrement(distanceData.Content);
            var position = transform.position;

            transform.position = Vector3.MoveTowards(
                position,
                target,
                speed * Time.deltaTime
                );

            if (transform.position.x <= target.x)
            {
                Destroy(gameObject);
                if (!_isInitial)
                {
                    sharedGameState.ChunkDestroyed.OnNext(Unit.Default);
                }
            }
        }

        public float GetRightBound()
        {
            return transform.position.x + width;
        }
    }
}