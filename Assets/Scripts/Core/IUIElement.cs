using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public interface IUIElement
    {
        void Init();
        void Show();
        void Hide();
        void Update();
        void Dispose();
    }
}