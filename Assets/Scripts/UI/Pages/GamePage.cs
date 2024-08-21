using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HotForgeStudio.HorrorBox
{
    public class GamePage : IUIElement
    {
        private GameObject _selfPage;

        private IUIManager _uiManager;
        private IDataManager _dataManager;
        private IAppStateManager _appStateManager;
        private IInputManager _inputManager;
        private IGameplayManager _gameplayManager;

        private MatchController _matchController;

        private Transform _controllersParent;

        private Button _pauseButton;
        private Button _settingsButton;

        private TextMeshProUGUI _timeText;

        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            _dataManager = GameClient.Get<IDataManager>();
            _appStateManager = GameClient.Get<IAppStateManager>();
            _inputManager = GameClient.Get<IInputManager>();
            _gameplayManager = GameClient.Get<IGameplayManager>();

            _matchController = GameClient.Get<IGameplayManager>().GetController<MatchController>();

            _selfPage = MonoBehaviour.Instantiate(GameClient.Get<ILoadObjectsManager>()
                .GetObjectByPath<GameObject>("Prefabs/UI/Pages/GamePage"),
                _uiManager.Canvas.transform, false);

            _controllersParent = _selfPage.transform.Find("Controllers");

            _pauseButton = _selfPage.transform.Find("Button_Pause").GetComponent<Button>();
            _settingsButton = _selfPage.transform.Find("Button_Settings").GetComponent<Button>();

            _timeText = _selfPage.transform.Find("Image_Timer/Text_Time").GetComponent<TextMeshProUGUI>();

            _matchController.GameplaySecondsUpdatedEvent += GameplaySecondsUpdatedEventHandler;

            _pauseButton.onClick.AddListener(PauseButtonOnClickHandler);
            _settingsButton.onClick.AddListener(SettingsButtonOnClickHandler);

            _selfPage.SetActive(false);
		}

        public void Dispose()
        {
            _matchController.GameplaySecondsUpdatedEvent -= GameplaySecondsUpdatedEventHandler;

            _pauseButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
        }

        public void Hide()
        {
            _selfPage.SetActive(false);
        }

        public void Show()
        {
            CleanUp();
            _selfPage.SetActive(true);
        }

		public void Update()
        {
        }

        public Transform GetControllsParent()
        {
            return _controllersParent;
        }

        private void CleanUp()
        {
            _timeText.text = "0s";
        }
        
        private void GameplaySecondsUpdatedEventHandler()
        {
            int gameplaySeconds = _matchController.GameplaySeconds;

            int minutes = (gameplaySeconds % 3600) / 60;
            int seconds = gameplaySeconds % 60;

            _timeText.text = minutes != 0 ? $"{minutes}m {seconds}s" : $"{seconds}s";
        }

        private void PauseButtonOnClickHandler()
        {
            _gameplayManager.SetPauseStatusOfGameplay(true);
            _uiManager.DrawPopup<PausePopup>();
        }

        private void SettingsButtonOnClickHandler()
        {
            _gameplayManager.SetPauseStatusOfGameplay(true);
            _uiManager.DrawPopup<SettingsPopup>();
        }
    }
}