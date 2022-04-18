using Code.Views;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Level Generator", menuName = "Circus/Levels", order = 0)]
    public class LevelGenerator : ScriptableObject
    {
        [SerializeField] private List<ChunkView> chunks;
        [SerializeField] private ChunkView startChunk;
        [SerializeField] private float startChunkWith;
        [SerializeField] private float chunkWidth;
        [SerializeField] private int amountOfChunks;
        public float Width => chunkWidth;
        public int AmountOfChunks => amountOfChunks;
        public float TotalWidth => (amountOfChunks -2) * chunkWidth;

        public float StartChunkWidth => startChunkWith;

        public ChunkView StartChunk => startChunk;
        public ChunkView GetChunk() => 
            chunks[Random.Range(0, chunks.Count)];
    }
}