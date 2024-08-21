namespace HotForgeStudio.HorrorBox
{
    public interface IAppStateManager
    {
        Common.Enumerators.AppState AppState { get; }
        void ChangeAppState(Common.Enumerators.AppState stateTo);
    }
}