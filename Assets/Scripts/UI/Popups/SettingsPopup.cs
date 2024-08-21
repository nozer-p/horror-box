using UnityEngine;
using UnityEngine.UI;

namespace HotForgeStudio.HorrorBox
{
    public class SettingsPopup : IUIPopup
    {
        private GameObject _selfPopup;

        private IUIManager _uiManager;
        private IGameplayManager _gameplayManager;
        private IDataManager _dataManager;
        private ISoundManager _soundManager;

        private Button _closeButton;

        private SoundsSetting _soundsSetting;
        private MusicSetting _musicSetting;

        public GameObject Self => _selfPopup;

        public void Init()
        {
            _uiManager = GameClient.Get<IUIManager>();
            _gameplayManager = GameClient.Get<IGameplayManager>();
            _dataManager = GameClient.Get<IDataManager>();
            _soundManager = GameClient.Get<ISoundManager>();

            _selfPopup = MonoBehaviour.Instantiate(GameClient.Get<ILoadObjectsManager>()
                .GetObjectByPath<GameObject>("Prefabs/UI/Popups/SettingsPopup"),
                _uiManager.Canvas.transform, false);

            _closeButton = _selfPopup.transform.Find("Panel_Content/Button_Close").GetComponent<Button>();

            _closeButton.onClick.AddListener(CloseButtonOnClickHandler);

            _soundsSetting = new SoundsSetting(_selfPopup.transform.Find("Panel_Content/Panel_ItemsContainer/SoundsSetting").gameObject);
            _musicSetting = new MusicSetting(_selfPopup.transform.Find("Panel_Content/Panel_ItemsContainer/MusicSetting").gameObject);

            _selfPopup.SetActive(false);
        }

        public void Dispose()
        {
            _closeButton.onClick.RemoveAllListeners();
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

        private void CloseButtonOnClickHandler()
        {
            _gameplayManager.SetPauseStatusOfGameplay(false);

            _dataManager.CachedUserLocalData.soundVolume = _soundManager.SoundVolume;
            _dataManager.CachedUserLocalData.musicVolume = _soundManager.MusicVolume;
            _dataManager.SaveAllData();

            Hide();
        }
    }
}