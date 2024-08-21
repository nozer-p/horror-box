namespace HotForgeStudio.HorrorBox
{
    public class GameClient : ServiceLocatorBase
    {
        private static object _sync = new object();

        private static GameClient _Instance;
        public static GameClient Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_sync)
                    {
                        _Instance = new GameClient();
                    }
                }
                return _Instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameClient"/> class.
        /// </summary>
        internal GameClient() : base()
        {
            AddService<IDataManager>(new DataManager());
            AddService<ISoundManager>(new SoundManager());
            AddService<ILoadObjectsManager>(new LoadObjectsManager());
            AddService<IAppStateManager>(new AppStateManager());
            AddService<IGameplayManager>(new GameplayManager());
            AddService<IUIManager>(new UIManager());
            AddService<IInputManager>(new InputManager());
        }

        public static T Get<T>()
        {
            return Instance.GetService<T>();
        }
    }
}