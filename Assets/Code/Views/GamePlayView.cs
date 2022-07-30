using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Assets.Code.Views;
using Code.Presenters;
using Code.ScriptableObjects;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Code.Views
{
    public class GamePlayView : MonoBehaviour
    {
        public Action<PlayerInput, PlayerView, CameraView, AudioCenter, InGameMenuView> GamePlayStart { get; set; }
        public Action GamePlayFinish { get; set; }
        public Action<float> MoveCamera { get; set; }
        public Action<Actionable>AttachToActionable { get; set; }
        public Action<Hazard>AttachToHazard { get; set; }
        
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerView playerView;
        [SerializeField] private CameraView cameraView;
        [SerializeField] private DisplayableData distance;
        [SerializeField] private DisplayableData tickets;
        [SerializeField] private SharedGameState sharedGameState;
        [SerializeField] private GameObject hudCanvas;
        [SerializeField] private ChunkGenerator chunkGenerator;
        [SerializeField] private AudioClip mainClip;
        [SerializeField] private AudioCenter audioCenter;
        [SerializeField] private InGameMenuView inGameMenuView;
        private PlayerView playerGo;

        public List<Actionable> Actionables { get; private set; }
        public void OnEnable()
        {
            audioCenter.ChangeBackgroundMusic(mainClip);
        }

        public void Initialize()
        {
            sharedGameState.JustDied = false;
            distance.Content = 0;
            tickets.Content = 0;
            Actionables = new List<Actionable>();
            SetUp();
            playerGo = Instantiate(playerView, transform);
            GamePlayStart(playerInput, playerGo, cameraView, audioCenter, inGameMenuView);
           
        }

        public void Finish()
        {
            ClearState();
            GamePlayFinish();
        }

        public void ClearState()
        {
            sharedGameState.PlayerDied.OnNext(Unit.Default);
            chunkGenerator.ClearChunks();
            Destroy(playerGo.gameObject);

            hudCanvas.SetActive(false);
            gameObject.SetActive(false);
        }


        private void Update()
        {
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
            hudCanvas.SetActive(true);
            gameObject.SetActive(true);
            sharedGameState.ChunkDestroyed.Subscribe(_ => chunkGenerator.CreateChunkAtLast());
        }


        public void StartGamePlay()
        {
            chunkGenerator.CreateChunksOnContainers();
        }
    }
}