using HotForgeStudio.HorrorBox.Common;
using System;

namespace HotForgeStudio.HorrorBox
{
    public interface IDataManager
    {
        event Action DataLoadedEvent;

        CachedUserData CachedUserLocalData { get; set; }

        void SaveAllData();

        void SaveData(Enumerators.GameDataType gameDataType);

        T Deserialize<T>(string data);
    }
}
