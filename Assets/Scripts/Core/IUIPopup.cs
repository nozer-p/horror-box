﻿using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public interface IUIPopup
    {
        GameObject Self { get; }

        void Init();
        void Show();
        void Show(object data);
        void Hide();
        void Update();
        void Dispose();
        void SetMainPriority();
    }
}