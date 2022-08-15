using Code.ScriptableObjects;
using Code.Views;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Assets.Code.Views
{
    public class ChunkGenerator : MonoBehaviour
    {
        [SerializeField] private GameConfiguration gameConfiguration;
        [SerializeField] private DisplayableData distanceData;
        [SerializeField] private SharedGameState sharedGameState;
        [SerializeField] private Waypoint target;
        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private GamePlayView gamePlayView;

        private List<Transform> containers = new List<Transform>();
        private Transform playerTransform;
        private List<ChunkView> _chunks;
        private float currentSpeed;
        private List<int> indexes;
        private void OnEnable()
        {
            //sharedGameState.PlayerDistanceFromBox.Subscribe(FixChunks);
            distanceData.Content = 0;
           containers = Enumerable
               .Range(0, levelGenerator.Chunks.Count + 1)
               .Select(index => new GameObject($"Container {index}").transform)
               .ToList();
        }

        // Update is called once per frame
        void Update()
        {
            if (sharedGameState.JustDied) return;

            currentSpeed = gameConfiguration.ChunkSpeed * Time.deltaTime;
            distanceData.Content += currentSpeed;
        }

        private void FixedUpdate()
        {
            foreach (var container in containers.Where(x => x.GetChild(0).gameObject.activeSelf))
            {
                MoveContainer(container);
            }
        }

        private void MoveContainer(Transform container)
        {
            
            container.position = new Vector3(container.position.x - currentSpeed, container.position.y, container.position.z);
            var child = container.GetChild(0);
            var chunkView = child.GetComponent<ChunkView>();
            if (container.position.x <= target.transform.position.x - currentSpeed)
            {
                if (chunkView.IsInitial) return;
                var newIndex = GetRandomChunkIndex();
                child.gameObject.SetActive(false);
                indexes.Add(newIndex);
                indexes.RemoveAt(0);
                Transform containerTransform = containers[newIndex].transform;
                containerTransform.position = new Vector3(containers[indexes[1]].transform.position.x + levelGenerator.Width, containerTransform.position.y, containerTransform.position.z);
                containers[newIndex].GetChild(0).gameObject.SetActive(true);
            }
        }

        public void CreateChunksOnContainers()
        {
            LoadChunks();
            SelectChunks();
            ShowChunks();
            /*
            var initialChunk = Instantiate(levelGenerator.StartChunk, Vector3.zero, Quaternion.identity, containers[0]);
            containers[0].position =  new Vector2(-levelGenerator.StartChunk.Witdh / 2, levelGenerator.StartChunk.transform.position.y);
            for (int i = 1; i < containers.Count; i++)
            {
                CreateChunkAtContainer(containers[i], containers[i-1].GetComponentInChildren<ChunkView>());
            }
            */
        }

        private void CreateChunkAtContainer(Transform container, ChunkView prevChunk)
        {
            var chunk = Instantiate(levelGenerator.GetChunk(), container);
            container.position = new Vector3(prevChunk.GetRightBound() + chunk.Witdh / 2, container.position.y, container.position.z);
        }

        public void CreateChunkAtLast()
        {
            //CreateChunkAtContainer(containers.First(containers => containers.childCount == 0));
            CreateChunkAt(_chunks.Count - 1);
        }

        public void ClearChunks()
        {
            foreach (var container in containers)
            {
                if (container != null)
                    Destroy(container.gameObject);
            }
            _chunks = new List<ChunkView>();
            containers = new List<Transform>();
        }
        private void CreateChunkAt(int index)
        {
            var chunk = Instantiate(levelGenerator.GetChunk(),transform);
            chunk.transform.position = new Vector3(_chunks[index].GetRightBound() + chunk.Witdh/2, transform.position.y, transform.position.z);
            _chunks.Add(chunk);
        }


        private void LoadChunks()
        {
            var index = 0;
            var initialChunk = Instantiate(levelGenerator.StartChunk, Vector3.zero, Quaternion.identity, containers[index]);
            containers[index].position = new Vector2(-levelGenerator.StartChunk.Witdh / 2, levelGenerator.StartChunk.transform.position.y);
            _chunks = new List<ChunkView> { initialChunk };
            foreach (var chunk in levelGenerator.Chunks)
            {
                index++;
                var chunkView = Instantiate<ChunkView>(chunk, containers[index]);
                chunkView.gameObject.SetActive(false);
                _chunks.Add(chunk);
            }
        }

        private void SelectChunks()
        {
            indexes = new List<int>();
            for (int i = 0; i < levelGenerator.AmountOfChunks; i++)
            {
                int random = GetRandomChunkIndex();
                indexes.Add(random);
            }
        }

        private int GetRandomChunkIndex()
        {
            var random = -1;
            do
                random = Random.Range(1, levelGenerator.Chunks.Count+1);
            while (indexes.Contains(random));
            return random;
        }

        private void ShowChunks()
        {
            for (int i = 0; i < indexes.Count; i++)
            {
                var index = indexes[i];
                var initialPosition = containers[0].transform.position.x ;
                Transform containerTransform = containers[index].transform;
                containerTransform.position =  new Vector3(initialPosition + levelGenerator.Width* (i+1), containerTransform.position.y, containerTransform.position.z);
                containers[index].GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}