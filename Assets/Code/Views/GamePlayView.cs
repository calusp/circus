using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Code.Presenters;
using Code.ScriptableObjects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Code.Views
{
    public class GamePlayView : MonoBehaviour
    {
        public Action<PlayerInput, PlayerView, CameraView> GamePlayStart { get; set; }
        public Action GamePlayFinish { get; set; }
        public Action<float> MovePlayer { get; set; }
        public Action<float> MoveCamera { get; set; }
        public Action<Actionable>AttachToActionable { get; set; }
        public Action<Hazard>AttachToHazard { get; set; }
        
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerView playerView;
        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private float chunkStartPositionHorizontal = -6.5f;
        [SerializeField] private float chunkStartPositionVertical = 0f;
        [SerializeField] private GameObject chunkContainer;
        [SerializeField] private CameraView cameraView;
        [Range(0.0f, 10f), SerializeField] private float moveHorizontalValue;
        private List<GameObject> _chunks;
        private int _previousChunk;
        private PlayerView playerGo;
        public List<Actionable> Actionables { get; private set; }

        public void Initialize()
        {
            Actionables = new List<Actionable>();
            SetUp();
            playerGo = Instantiate(playerView, transform);
            GamePlayStart(playerInput, playerGo, cameraView);
        }

        public void Finish()
        {
            gameObject.SetActive(false);
            ClearChunks();
            Destroy(playerGo.gameObject);
            GamePlayFinish();
        }

        private void ClearChunks()
        {
            _chunks.ForEach(Destroy);
        }

        private void Update()
        {
            var moveAmount = moveHorizontalValue * Time.deltaTime;
            MovePlayer(moveAmount);
            MoveCamera(moveAmount * 1.1f);
            if (HasPlayerCollidedHorizontally(playerGo.transform.position.x)) 
                Finish();
        } 

        private bool HasPlayerCollidedHorizontally(float position) => 
            HasCollidedWithEndOfCamera(position, position+0.5f, cameraView.NextPositionOnAxisX - 17.5f * 0.5f);

        private void FixedUpdate()
        {
            if (HasCameraGotANewChunk()) return;
            UpdateGameplayForChunk(chunk: _chunks[_previousChunk]);
            _previousChunk = CurrentCameraChunk().GetComponent<ChunkContainerView>().Id;
        }

        private bool HasCameraGotANewChunk() =>
            _previousChunk == CurrentCameraChunk().GetComponent<ChunkContainerView>().Id;


        private GameObject CurrentCameraChunk() =>
            _chunks.Any(HasCollided) ? _chunks.FirstOrDefault(HasCollided) : _chunks.First();

        private bool HasCollided(GameObject chunk)
        {
            var chunkPosition = chunk.transform.position;
            var chunkMinX = chunkPosition.x - levelGenerator.Width * 0.5f;
            var chunkMaxX = chunkPosition.x + levelGenerator.Width * 0.5f;
            var cameraMinX = cameraView.NextPositionOnAxisX - 17.5f * 0.5f;
            return HasCollidedWithEndOfCamera(chunkMinX, chunkMaxX, cameraMinX);
        }

        private bool HasCollidedWithEndOfCamera(float colliderMinX, float colliderMaxX, float cameraMinX) =>
            colliderMinX <
            cameraMinX &&
            colliderMaxX >=
            cameraMinX;

        private void SetUp()
        {
            gameObject.SetActive(true);
            _previousChunk = 0;
        }

        public void CreateChunks()
        {
            var initialChunk =  CreateFirstChunk();

            _chunks = Enumerable.Range(0, levelGenerator.AmountOfChunks)
                .Select(_ => InitializeChunkContainersWithFirstChunks()).ToList();
            for (var i = 0; i < _chunks.Count; i++)
            {
                var chunk = _chunks[i];
                var offset = (levelGenerator.Width * 0.5f - levelGenerator.StartChunkWidth * 0.5f);
                var initialposition = offset + initialChunk.transform.position.x + levelGenerator.StartChunkWidth;
                chunk.transform.position =
                    new Vector3(initialposition + (i * levelGenerator.Width) , chunkStartPositionVertical);
                chunk.GetComponent<ChunkContainerView>().Id = i;
            }
        }

        private GameObject CreateFirstChunk()
        {
            var container = Instantiate(chunkContainer, transform);
            var initialChunk = Instantiate(levelGenerator.StartChunk, container.transform);
            container.GetComponent<ChunkContainerView>().Chunk = initialChunk.GetComponent<ChunkView>();
            container.transform.position =
                    new Vector3(chunkStartPositionHorizontal, chunkStartPositionVertical);
            return container;
        }

        private GameObject InitializeChunkContainersWithFirstChunks()
        {
            var go = Instantiate(chunkContainer, transform);
            AddChunkToContainer(go);
            return go;
        }

        private void AddChunkToContainer(GameObject container)
        {
            var go = Instantiate(levelGenerator.GetChunk(), container.transform);
            container.GetComponent<ChunkContainerView>().Chunk = go.GetComponent<ChunkView>();
            if (container.GetComponent<ChunkContainerView>().Chunk.HasActionable)
                AttachToActionable(container.GetComponent<ChunkContainerView>().Chunk.Actionable);
            if (container.GetComponent<ChunkContainerView>().Chunk.HasHazard)
                AttachToHazard(container.GetComponent<ChunkContainerView>().Chunk.Hazard);
        }

        private void UpdateGameplayForChunk(GameObject chunk)
        {
            chunk.transform.position =
                new Vector3(chunk.transform.position.x + levelGenerator.TotalWidth,
                    chunkStartPositionVertical);
        
            Destroy(chunk.transform.GetChild(0).gameObject);
            AddChunkToContainer(chunk);
        }
    }
}