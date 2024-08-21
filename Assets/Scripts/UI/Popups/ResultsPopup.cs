using UnityEngine;
using UnityEngine.UI;
using HotForgeStudio.HorrorBox.Common;
using TMPro;

namespace HotForgeStudio.HorrorBox
{
    public class ResultsPopup : IUIPopup
    {
        private GameObject _selfPopup;

        private IUIManager _uiManager;
        private IAppStateManager _appStateManager;
        private IDataManager _dataManager;

        private MatchController _matchController;

        private TextMeshProUGUI _currentTimeText;
        private TextMeshProUGUI _bestTimeText;

        private Button _playAgainButton;
        private Button _menuButton;

        public GameObject Self => _selfPopup;

        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            _appStateManager = GameClient.Get<IAppStateManager>();
            _dataManager = GameClient.Get<IDataManager>();

            _matchController = GameClient.Get<IGameplayManager>().GetController<MatchController>();

            _selfPopup = MonoBehaviour.Instantiate(GameClient.Get<ILoadObjectsManager>()
                .GetObjectByPath<GameObject>("Prefabs/UI/Popups/ResultsPopup"),
                _uiManager.Canvas.transform, false);

            _currentTimeText = _selfPopup.transform.Find("Panel_Content/Text_CurrentTime").GetComponent<TextMeshProUGUI>();
            _bestTimeText = _selfPopup.transform.Find("Panel_Content/Text_BestTime").GetComponent<TextMeshProUGUI>();

            _playAgainButton = _selfPopup.transform.Find("Panel_Content/Container_Buttons/Button_PlayAgain").GetComponent<Button>();
            _menuButton = _selfPopup.transform.Find("Panel_Content/Container_Buttons/Button_Menu").GetComponent<Button>();

            _playAgainButton.onClick.AddListener(PlayAgainButtonOnClickHandler);
            _menuButton.onClick.AddListener(MenuButtonOnClickHandler);

            _selfPopup.SetActive(false);
        }

        public void Dispose()
        {
            _playAgainButton.onClick.RemoveAllListeners();
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
            SetTextData(_currentTimeText, Constants.CurrentTime);
            SetTextData(_bestTimeText, Constants.BestTime);

            _selfPopup.SetActive(true);
        }

        public void Update()
        {
        }

        public void SetMainPriority()
        {
        }

        private void SetTextData(TextMeshProUGUI textMeshPro, string text)
        {
            int gameplaySeconds = _matchController.GameplaySeconds;
            int minutes = (gameplaySeconds % 3600) / 60;
            int seconds = gameplaySeconds % 60;
            string time = minutes != 0 ? $"{minutes}m {seconds}s" : $"{seconds}s";
            textMeshPro.text = $"{text} {time}";
        }

        private void PlayAgainButtonOnClickHandler()
        {
            Hide();
            _appStateManager.ChangeAppState(Enumerators.AppState.Game);
        }

        private void MenuButtonOnClickHandler()
        {
            Hide();
            _appStateManager.ChangeAppState(Enumerators.AppState.Main);
        }
    }
}