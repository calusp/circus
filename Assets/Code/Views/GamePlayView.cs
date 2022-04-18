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
        [SerializeField] private CameraView cameraView;
        [SerializeField] private DisplayableData displayableData;
        [SerializeField] private SharedGameState sharedGameState;
        [SerializeField] private GameObject endGameCanvas;
        [SerializeField] private GameObject hudCanvas;

        private GameObject initialChunk;
        private List<ChunkView> _chunks;
        private PlayerView playerGo;
        private GameConfiguration _configuration;

        public List<Actionable> Actionables { get; private set; }

        public void Initialize(GameConfiguration configuration)
        {
            displayableData.Content = 0;
            _configuration = configuration;
            Actionables = new List<Actionable>();
            SetUp();
            playerGo = Instantiate(playerView, transform);
            GamePlayStart(playerInput, playerGo, cameraView);
        }

        public void Finish()
        {
            //gameObject.SetActive(false);
            ClearChunks();
            //Destroy(playerGo.gameObject);
            GamePlayFinish();
            endGameCanvas.SetActive(true);
            hudCanvas.SetActive(false);
        }

        private void ClearChunks()
        {
            _chunks.ForEach(Destroy);
            Destroy(initialChunk);
        }

        private void Update()
        {
            float playerSpeed = _configuration.PlayerSpeed;
            MovePlayer(playerSpeed * Time.deltaTime);
            
                
            if (HasPlayerCollidedHorizontally(playerGo.transform.position.x)) 
                Finish();
        } 

        private bool HasPlayerCollidedHorizontally(float position) => 
            HasCollidedWithEndOfCamera(position, position+0.5f, cameraView.NextPositionOnAxisX - 17.5f * 0.5f);

        private bool HasCollidedWithEndOfCamera(float colliderMinX, float colliderMaxX, float cameraMinX) =>
            colliderMinX < cameraMinX &&
            colliderMaxX >= cameraMinX;

        private void SetUp()
        {
            gameObject.SetActive(true);
            sharedGameState.ChunkDestroyed.Subscribe(_ => CreateChunkAt(_chunks.Count-1));
        }


        public void StartGamePlay()
        {
            var initialChunk = Instantiate(levelGenerator.StartChunk, new Vector2(-levelGenerator.StartChunk.Witdh/2, levelGenerator.StartChunk.transform.position.y), Quaternion.identity, transform);
            _chunks = new List<ChunkView> { initialChunk};
            for (int i = 0; i < levelGenerator.AmountOfChunks; i++)
            {
                CreateChunkAt(i);
            }
        }

        private void CreateChunkAt(int index)
        {
            var chunk = Instantiate(levelGenerator.GetChunk(), transform);
            chunk.transform.position = new Vector3(_chunks[index].GetRightBound(), transform.position.y, transform.position.z);
            if (chunk.HasActionable)
                AttachToActionable(chunk.Actionable);
            if (chunk.HasHazard)
                AttachToHazard(chunk.Hazard);
            _chunks.Add(chunk);
        }
    }
}