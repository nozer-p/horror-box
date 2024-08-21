using System;
using System.Collections.Generic;
using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public class GameplayManager : IService, IGameplayManager
    {
        public event Action GameplayStartedEvent;
        public event Action GameplayEndedEvent;

        private List<IController> _controllers;

        private IUIManager _uiManager;
        private ILoadObjectsManager _loadObjectsManager;
        private IInputManager _inputManager;

        public bool IsGameplayStarted { get; private set; }
        public bool IsGameplayPaused { get; private set; }

        public GameplayData GameplayData { get; private set; }

        public GameObject GameplayObject { get; private set; }

        private Player _player;

        public void Dispose()
        {
            foreach (var item in _controllers)
                item.Dispose();
        }

        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            _loadObjectsManager = GameClient.Get<ILoadObjectsManager>();
            _inputManager = GameClient.Get<IInputManager>();

            GameplayData = _loadObjectsManager.GetObjectByPath<GameplayData>("Data/GameplayData");

            GameplayObject = MonoBehaviour.Instantiate(_loadObjectsManager.
                GetObjectByPath<GameObject>("Prefabs/Gameplay/Gameplay"), MainApp.Instance.transform, false);
            GameplayObject.name = "[GameplayContainer]";

            _controllers = new List<IController>()
            {
                new CameraController(),
                new PlayerController(),
                new MatchController(),
                new EnemyController()
            };

            foreach (var item in _controllers)
                item.Init();
        }

        public void Update()
        {
            foreach (var item in _controllers)
                item.Update();
        }

        public T GetController<T>() where T : IController
        {
            foreach (var item in _controllers)
            {
                if (item is T)
                {
                    return (T)item;
                }
            }
            throw new Exception("Controller " + typeof(T).ToString() + " have not implemented");
        }

        public void StartGameplay()
		{
            if (IsGameplayStarted)
                return;

            _inputManager.CanHandleInput = true;

            IsGameplayPaused = false;
            IsGameplayStarted = true;

            GameplayStartedEvent?.Invoke();
        }

        public void StopGameplay()
		{
            if (!IsGameplayStarted)
                return;

            foreach (var item in _controllers)
                item.ResetAll();

            _inputManager.CanHandleInput = false;

            IsGameplayStarted = false;
            IsGameplayPaused = false;

            GameplayEndedEvent?.Invoke();
        }

        public void SetPauseStatusOfGameplay(bool status)
        {
            if (!IsGameplayStarted || IsGameplayPaused == status)
                return;

            IsGameplayPaused = status;

            _inputManager.CanHandleInput = !status;
        }
    }
}