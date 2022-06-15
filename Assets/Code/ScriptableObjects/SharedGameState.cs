using System.Collections;
using UniRx;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SharedGameState")]
    public class SharedGameState : ScriptableObject
    {
        private readonly ISubject<Unit> _chunkDestroyed = new Subject<Unit>();
        private readonly ISubject<Unit> _playerDied = new Subject<Unit>();
        private readonly ISubject<Unit> _enteredCannon = new Subject<Unit>();
        private readonly ISubject<float> _playerDistanceFromBox = new Subject<float>();
        public ISubject<Unit> ChunkDestroyed => _chunkDestroyed;
        public ISubject<Unit> PlayerDied => _playerDied;
        public bool JustDied { get; set; }
        public ISubject<float> PlayerDistanceFromBox => _playerDistanceFromBox;

        public ISubject<Unit> EnteredCannon => _enteredCannon;
    }
}