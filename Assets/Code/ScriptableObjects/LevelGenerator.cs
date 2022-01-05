using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Level Generator", menuName = "Circus/Levels", order = 0)]
    public class LevelGenerator : ScriptableObject
    {
        [SerializeField] private List<GameObject> chunks;
        [SerializeField] private float chunkWidth;
        [SerializeField] private int amountOfChunks;
        public float Width => chunkWidth;
        public int AmountOfChunks => amountOfChunks;
        public float TotalWidth => amountOfChunks * chunkWidth;

        public GameObject GetChunk() => 
            chunks[Random.Range(0, chunks.Count)];
    }
}