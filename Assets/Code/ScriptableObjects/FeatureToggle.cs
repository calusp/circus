using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Feature Toggle", menuName = "Configuration/Feature Toggles", order = 0)]
    public class FeatureToggle : ScriptableObject
    {
        [SerializeField] private bool _isCameraMovingPlayer = false;
        public bool IsCameraMovingPlayer => _isCameraMovingPlayer;
    }
}