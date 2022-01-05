using UnityEngine;

namespace Code.Views
{
    public class ChunkView : MonoBehaviour
    {
        [SerializeField] private BaseActionableView _actionableView;
        [SerializeField] private bool _hasActionable;
        public int Id { get; set; }

        public Actionable Actionable => _actionableView;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public bool HasActionable() => _hasActionable;
    }
}