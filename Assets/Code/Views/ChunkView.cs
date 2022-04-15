using Code.Presenters;
using Code.ScriptableObjects;
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
        public int Id { get; set; }

        public Actionable Actionable => _actionableView;
        public Hazard Hazard => _hazardViewView;
        private Transform playerTransform;
        private float prevDistance;
        private float _increments;

        // Start is called before the first frame update
        void Awake()
        {
            playerTransform = GameObject.Find("Player(Clone)").transform;
            MoveChunk();
            prevDistance = (transform.position.x - width / 2) + (playerTransform.position.x + 5) +36.5f;
        }

        // Update is called once per frame
        void Update()
        {
            MoveChunk();
            SaveDistanceMoved();
        }

        private void SaveDistanceMoved()
        {
            var distance = (transform.position.x - width / 2) + (playerTransform.position.x + 5);
            if (distance > 0)
                distanceData.Content +=Mathf.Clamp(Mathf.Abs(prevDistance) - Mathf.Abs(distance),0,1);
            prevDistance = distance;
        }

        private void MoveChunk()
        {
            var speed = gameConfiguration.CameraSpeed + gameConfiguration.IncrementalRatio * _increments;
            if (distanceData.Content > gameConfiguration.DistanceCap * (_increments + 1))
                _increments++;
            var position = transform.position;
            transform.position = Vector3.MoveTowards(
                position,
                new Vector3(position.x - width, position.y, position.z),
                speed * Time.deltaTime
                );
        }

        public bool HasActionable => _hasActionable;

        public bool HasHazard => _hasHazzard;
    }
}