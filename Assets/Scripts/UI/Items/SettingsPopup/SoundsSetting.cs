using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public class SoundsSetting : SliderItem
    {
        private ISoundManager _soundManager;

        public SoundsSetting(GameObject gameObject) : base(gameObject)
        {
            _soundManager = GameClient.Get<ISoundManager>();

            SetStartSliderValue(_soundManager.SoundVolume);

            OnSliderValueChangedEvent += OnSliderValueChangedEventHandler;
        }

        private void OnSliderValueChangedEventHandler(float value)
        {
            _soundManager.SoundVolume = value;
        }
    }
}