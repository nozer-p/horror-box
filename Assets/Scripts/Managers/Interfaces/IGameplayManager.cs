using System;
using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public interface IGameplayManager
    {
        event Action GameplayStartedEvent;
        event Action GameplayEndedEvent;

        bool IsGameplayStarted { get; }
        bool IsGameplayPaused { get; }
        GameplayData GameplayData { get; }
        GameObject GameplayObject { get; }

        T GetController<T>() where T : IController;

        void StartGameplay();
        void StopGameplay();
        void SetPauseStatusOfGameplay(bool status);
    }
}