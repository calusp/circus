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

        private void OnEnable()
        {
            //sharedGameState.PlayerDistanceFromBox.Subscribe(FixChunks);
            distanceData.Content = 0;
           containers = Enumerable
               .Range(0, levelGenerator.AmountOfChunks + 1)
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
            foreach (var container in containers)
            {
                MoveContainer(container);
            }
        }

        private void MoveContainer(Transform container)
        {
            
            container.position = new Vector3(container.position.x - currentSpeed, container.position.y, container.position.z);
            

            var child = container.GetChild(0);
            var chunkView = child.GetComponent<ChunkView>();
            if (container.position.x <= target.transform.position.x)
            {
                if (chunkView.IsInitial) return;
                Destroy(chunkView.gameObject);
                int indexOfCurrent = containers.IndexOf(container);
                var indexOfPrev = indexOfCurrent == 1 ? containers.Count - 1 : indexOfCurrent -1;
                CreateChunkAtContainer(container, containers[indexOfPrev].GetComponentInChildren<ChunkView>());
            }
        }

        public void CreateChunksOnContainers()
        {
            var initialChunk = Instantiate(levelGenerator.StartChunk, Vector3.zero, Quaternion.identity, containers[0]);
            containers[0].position =  new Vector2(-levelGenerator.StartChunk.Witdh / 2, levelGenerator.StartChunk.transform.position.y);
            for (int i = 1; i < containers.Count; i++)
            {
                CreateChunkAtContainer(containers[i], containers[i-1].GetComponentInChildren<ChunkView>());
            }
        }

        private void CreateChunkAtContainer(Transform container, ChunkView prevChunk)
        {
            var chunk = Instantiate(levelGenerator.GetChunk(), container);
            container.position = new Vector3(prevChunk.GetRightBound() + chunk.Witdh / 2, container.position.y, container.position.z);
            if (chunk.HasActionable)
                gamePlayView.AttachToActionable(chunk.Actionable);
            if (chunk.HasHazard)
                gamePlayView.AttachToHazard(chunk.Hazard);
        }

        public void CreateChunks()
        {
            var initialChunk = Instantiate(levelGenerator.StartChunk, new Vector2(-levelGenerator.StartChunk.Witdh / 2, levelGenerator.StartChunk.transform.position.y), Quaternion.identity, transform);
            _chunks = new List<ChunkView> { initialChunk };
            for (int i = 0; i < levelGenerator.AmountOfChunks; i++)
            {
                CreateChunkAt(i);
            }
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
            if (chunk.HasActionable)
                gamePlayView.AttachToActionable(chunk.Actionable);
            if (chunk.HasHazard)
                gamePlayView.AttachToHazard(chunk.Hazard);
            _chunks.Add(chunk);
        }
    }
}