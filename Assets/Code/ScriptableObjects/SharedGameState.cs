using System.Collections;
using UniRx;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SharedGameState")]
    public class SharedGameState : ScriptableObject
    {
        private ISubject<Unit> _chunkDestroyed;
        private ISubject<Unit> _playerDied;
        private ISubject<Unit> _enteredCannon;
        private ISubject<float> _playerDistanceFromBox;
        private ISubject<Unit> _onTrampolineHit;

        public void Initialize()
        {
            _chunkDestroyed = new Subject<Unit>();
            _playerDied = new Subject<Unit>();
            _enteredCannon = new Subject<Unit>();
            _playerDistanceFromBox = new Subject<float>();
            _onTrampolineHit = new Subject<Unit>();

        }
        public ISubject<Unit> ChunkDestroyed => _chunkDestroyed;
        public ISubject<Unit> PlayerDied => _playerDied;
        public bool JustDied { get; set; }
        public ISubject<float> PlayerDistanceFromBox => _playerDistanceFromBox;

        public ISubject<Unit> EnteredCannon => _enteredCannon;

        public ISubject<Unit> OnTrampolineHit => _onTrampolineHit;
    }
}