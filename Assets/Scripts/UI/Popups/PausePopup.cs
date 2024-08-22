using UnityEngine;
using UnityEngine.UI;
using HotForgeStudio.HorrorBox.Common;

namespace HotForgeStudio.HorrorBox
{
    public class PausePopup : IUIPopup
    {
        private GameObject _selfPopup;

        private IUIManager _uiManager;
        private IGameplayManager _gameplayManager;
        private IAppStateManager _appStateManager;
        private ISoundManager _soundManager;

        private Button _playButton;
        private Button _menuButton;

        public GameObject Self => _selfPopup;

        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            _gameplayManager = GameClient.Get<IGameplayManager>();
            _appStateManager = GameClient.Get<IAppStateManager>();
            _soundManager = GameClient.Get<ISoundManager>();

            _selfPopup = MonoBehaviour.Instantiate(GameClient.Get<ILoadObjectsManager>()
                .GetObjectByPath<GameObject>("Prefabs/UI/Popups/PausePopup"),
                _uiManager.Canvas.transform, false);

            _playButton = _selfPopup.transform.Find("Panel_Content/Container_Buttons/Button_Play").GetComponent<Button>();
            _menuButton = _selfPopup.transform.Find("Panel_Content/Container_Buttons/Button_Menu").GetComponent<Button>();

            _playButton.onClick.AddListener(PlayButtonOnClickHandler);
            _menuButton.onClick.AddListener(MenuButtonOnClickHandler);

            _selfPopup.SetActive(false);
        }

        public void Dispose()
        {
            _playButton.onClick.RemoveAllListeners();
            _menuButton.onClick.RemoveAllListeners();
        }

        public void Hide()
        {
            _selfPopup.SetActive(false);
        }

        public void Show(object data)
        {
            Show();
        }

        public void Show()
        {
            _selfPopup.SetActive(true);
        }

        public void Update()
        {
        }

        public void SetMainPriority()
        {
        }

        private void PlayButtonOnClickHandler()
        {
            _soundManager.PlaySound(Enumerators.SoundType.Knife);
            Hide();
            _gameplayManager.SetPauseStatusOfGameplay(false);
        }

        private void MenuButtonOnClickHandler()
        {
            _soundManager.PlaySound(Enumerators.SoundType.Knife);
            Hide();
            _appStateManager.ChangeAppState(Enumerators.AppState.Main);
        }
    }
}