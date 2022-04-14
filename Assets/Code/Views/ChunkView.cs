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
        public int Id { get; set; }

        public Actionable Actionable => _actionableView;
        public Hazard Hazard => _hazardViewView;

        // Start is called before the first frame update
        void Start()
        {
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
        }

        public bool HasActionable => _hasActionable;

        public bool HasHazard => _hasHazzard;
    }
}