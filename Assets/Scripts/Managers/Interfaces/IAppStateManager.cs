using HotForgeStudio.HorrorBox.Common;
using System;

namespace HotForgeStudio.HorrorBox
{
    public interface IAppStateManager
    {
        event Action AppStateChangedEvent;

        Enumerators.AppState AppState { get; }
        
        void ChangeAppState(Common.Enumerators.AppState stateTo);
    }
}