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
        // Start is called before the first frame update
        void Awake()
        {
            playerTransform = GameObject.Find("Player(Clone)").transform;
            prevDistance = (transform.position.x - width / 2) + (playerTransform.position.x + 5);
        }

        // Update is called once per frame
        void Update()
        {
            var position = transform.position;
            transform.position = Vector3.MoveTowards(
                position, 
                new Vector3(position.x - width, position.y, position.z),
                gameConfiguration.CameraSpeed * Time.deltaTime
                );

            var distance = (transform.position.x - width / 2) + (playerTransform.position.x + 5);
            if (distance > 0)
                distanceData.Content += Mathf.Abs(prevDistance) - Mathf.Abs( distance) ;
            prevDistance = distance;
        }

        public bool HasActionable => _hasActionable;

        public bool HasHazard => _hasHazzard;
    }
}