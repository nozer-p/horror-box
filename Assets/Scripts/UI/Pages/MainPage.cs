using UnityEngine;
using UnityEngine.UI;
using HotForgeStudio.HorrorBox.Common;

namespace HotForgeStudio.HorrorBox
{
    public class MainPage : IUIElement
    {
        private GameObject _selfPage;

        private IUIManager _uiManager;
        private IAppStateManager _appStateManager;

        private Button _playButton;
        private Button _settingsButton;

        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            _appStateManager = GameClient.Get<IAppStateManager>();

            _selfPage = MonoBehaviour.Instantiate(GameClient.Get<ILoadObjectsManager>()
                .GetObjectByPath<GameObject>("Prefabs/UI/Pages/MainPage"),
                _uiManager.Canvas.transform, false);

            _playButton = _selfPage.transform.Find("Button_Play").GetComponent<Button>();
            _settingsButton = _selfPage.transform.Find("Button_Settings").GetComponent<Button>();

            _playButton.onClick.AddListener(PlayButtonOnClickHandler);
            _settingsButton.onClick.AddListener(SettingsButtonOnClickHandler);

            _selfPage.SetActive(false);
		}

        public void Dispose()
        {
            _playButton.onClick.RemoveAllListeners();
            _settingsButton.onClick.RemoveAllListeners();
        }

        public void Hide()
        {
            _selfPage.SetActive(false);
        }

        public void Show()
        {
            _selfPage.SetActive(true);
        }

		public void Update()
        {
        }

        private void PlayButtonOnClickHandler()
        {
            _appStateManager.ChangeAppState(Enumerators.AppState.Game);
        }

        private void SettingsButtonOnClickHandler()
        {
            _uiManager.DrawPopup<SettingsPopup>();
        }            
    }
}