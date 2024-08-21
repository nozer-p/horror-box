namespace HotForgeStudio.HorrorBox
{
    public interface IServiceLocator
    {
        T GetService<T>();
        void Update();
    }
}