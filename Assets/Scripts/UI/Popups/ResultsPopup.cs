using UnityEngine;
using UnityEngine.UI;
using HotForgeStudio.HorrorBox.Common;
using TMPro;
using DG.Tweening;

namespace HotForgeStudio.HorrorBox
{
    public class ResultsPopup : IUIPopup
    {
        private IUIManager _uiManager;
        private IAppStateManager _appStateManager;
        private IDataManager _dataManager;
        private ISoundManager _soundManager;

        private MatchController _matchController;

        private GameObject _selfPopup;
        private GameObject _contentObject;

        public GameObject Self => _selfPopup;

        private TextMeshProUGUI _currentTimeText;
        private TextMeshProUGUI _bestTimeText;

        private Button _playAgainButton;
        private Button _menuButton;

        private CanvasGroup _canvasGroup;

        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            _appStateManager = GameClient.Get<IAppStateManager>();
            _dataManager = GameClient.Get<IDataManager>();
            _soundManager = GameClient.Get<ISoundManager>();

            _matchController = GameClient.Get<IGameplayManager>().GetController<MatchController>();

            _selfPopup = MonoBehaviour.Instantiate(GameClient.Get<ILoadObjectsManager>()
                .GetObjectByPath<GameObject>("Prefabs/UI/Popups/ResultsPopup"),
                _uiManager.Canvas.transform, false);

            _contentObject = _selfPopup.transform.Find("Panel_Content").gameObject;

            _currentTimeText = _contentObject.transform.Find("Text_CurrentTime").GetComponent<TextMeshProUGUI>();
            _bestTimeText = _contentObject.transform.Find("Text_BestTime").GetComponent<TextMeshProUGUI>();

            _playAgainButton = _contentObject.transform.Find("Container_Buttons/Button_PlayAgain").GetComponent<Button>();
            _menuButton = _contentObject.transform.Find("Container_Buttons/Button_Menu").GetComponent<Button>();

            _canvasGroup = _selfPopup.GetComponent<CanvasGroup>();

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
            _canvasGroup.DOKill();
            _canvasGroup.alpha = 1f;
            _selfPopup.SetActive(false);
        }

        public void Show(object data)
        {
            Show();
        }

        public void Show()
        {
            SetTextData(_currentTimeText, Constants.CurrentTime, _matchController.GameplaySeconds);
            SetTextData(_bestTimeText, Constants.BestTime, _dataManager.CachedUserLocalData.bestTimeSurvived);

            _canvasGroup.DOKill();
            _canvasGroup.alpha = 0.001f;
            _contentObject.SetActive(false);
            _selfPopup.SetActive(true);
            _canvasGroup.DOFade(1f, 3f).SetEase(Ease.InOutSine)
                .OnComplete(() => _contentObject.SetActive(true));
        }

        public void Update()
        {
        }

        public void SetMainPriority()
        {
        }

        private void SetTextData(TextMeshProUGUI textMeshPro, string text, int gameplaySeconds)
        {
            int minutes = (gameplaySeconds % 3600) / 60;
            int seconds = gameplaySeconds % 60;
            string time = minutes != 0 ? $"{minutes}m {seconds}s" : $"{seconds}s";
            textMeshPro.text = $"{text} {time}";
        }

        private void PlayAgainButtonOnClickHandler()
        {
            _soundManager.PlaySound(Enumerators.SoundType.Knife);
            Hide();
            _appStateManager.ChangeAppState(Enumerators.AppState.Game);
        }

        private void MenuButtonOnClickHandler()
        {
            _soundManager.PlaySound(Enumerators.SoundType.Knife);
            Hide();
            _appStateManager.ChangeAppState(Enumerators.AppState.Main);
        }
    }
}