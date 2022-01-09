using Code.Presenters;
using UnityEngine;

namespace Code.Views
{
    public class ChunkView : MonoBehaviour
    {
        [SerializeField] private BaseActionableView _actionableView;
        [SerializeField] private BaseHazardView _hazardViewView;
        [SerializeField] private bool _hasActionable;
        [SerializeField] private bool _hasHazzard;
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
        
        }

        public bool HasActionable => _hasActionable;

        public bool HasHazard => _hasHazzard;
    }
}