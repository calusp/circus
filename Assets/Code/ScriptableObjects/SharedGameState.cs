using System.Collections;
using UniRx;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SharedGameState")]
    public class SharedGameState : ScriptableObject
    {
       private readonly ISubject<Unit> _chunkDestroyed = new Subject<Unit>();

        public ISubject<Unit> ChunkDestroyed => _chunkDestroyed;

        public bool JustDied { get; set; }
    }
}