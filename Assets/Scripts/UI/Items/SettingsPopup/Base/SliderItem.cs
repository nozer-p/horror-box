using System;
using UnityEngine;
using UnityEngine.UI;

namespace HotForgeStudio.HorrorBox
{
    public class SliderItem
    {
        protected event Action<float> OnSliderValueChangedEvent;

        protected GameObject _selfObject;

        protected Slider _slider;

        public SliderItem(GameObject gameObject)
        {
            _selfObject = gameObject;

            _slider = _selfObject.transform.Find("Slider").GetComponent<Slider>();

            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void SetStartSliderValue(float value)
        {
            _slider.SetValueWithoutNotify(value);
        }

        private void OnSliderValueChanged(float value)
        {
            OnSliderValueChangedEvent?.Invoke(value);
        }
    }
}