using System;
using UnityEngine;
using UnityEngine.UI;

namespace HotForgeStudio.HorrorBox
{
    public interface IUIManager
    {
        GameObject Canvas { get; }
        IUIElement CurrentPage { get; }

        void SetPage<T>(bool hideAll = false) where T : IUIElement;
        void DrawPopup<T>(object message = null, bool setMainPriority = false) where T : IUIPopup;
        void HidePopup<T>() where T : IUIPopup;
        T GetPopup<T>() where T : IUIPopup;
        T GetPage<T>() where T : IUIElement;

        void HideAllPages();
        void HideAllPopups();
    }
}