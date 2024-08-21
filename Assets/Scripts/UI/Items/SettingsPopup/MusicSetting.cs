using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public class MusicSetting : SliderItem
    {
        private ISoundManager _soundManager;

        public MusicSetting(GameObject gameObject) : base(gameObject)
        {
            _soundManager = GameClient.Get<ISoundManager>();

            SetStartSliderValue(_soundManager.MusicVolume);

            OnSliderValueChangedEvent += OnSliderValueChangedEventHandler;
        }

        private void OnSliderValueChangedEventHandler(float value)
        {
            _soundManager.MusicVolume = value;
        }
    }
}