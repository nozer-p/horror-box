using HotForgeStudio.HorrorBox.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace HotForgeStudio.HorrorBox
{
    public class DataManager : IService, IDataManager
    {
        public event Action DataLoadedEvent;

        private readonly string StaticDataPrivateKey = "ASHHA12525zb7PWSRMpHMtWXAYUCanQYxxzb7PWSfas8215125asgddh6012GAsgasg";

        public CachedUserData CachedUserLocalData { get; set; }

        public readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            Converters =
                {
                    new StringEnumConverter(),
                }
        };

        private Dictionary<Enumerators.GameDataType, string> _gameDataPathes;

        public void Init()
        {
            _gameDataPathes = new Dictionary<Enumerators.GameDataType, string>()
            {
                 { Enumerators.GameDataType.UserData, $"{Application.persistentDataPath}/userData.dat" },
            };

            LoadAllData();
        }

        public void Update()
        {
        }

        public void Dispose()
        {
            SaveAllData();
        }

        public void SaveAllData()
        {
            foreach (Enumerators.GameDataType key in _gameDataPathes.Keys)
            {
                SaveData(key);
            }
        }

        public void LoadAllData()
        {
            foreach (Enumerators.GameDataType key in _gameDataPathes.Keys)
            {
                LoadData(key);
            }

            DataLoadedEvent?.Invoke();
        }

        public void SaveData(Enumerators.GameDataType gameDataType)
        {
            string data = string.Empty;
            string dataPath = _gameDataPathes[gameDataType];

            switch (gameDataType)
            {
                case Enumerators.GameDataType.UserData:
                    data = Serialize(CachedUserLocalData);
                    break;

                default:break;
            }

            if (data.Length > 0)
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                PlayerPrefs.SetString(gameDataType.ToString(), data);
                PlayerPrefs.Save();
#else
                if (!File.Exists(dataPath)) File.Create(dataPath).Close();

                File.WriteAllText(dataPath, data);
#endif
            }
        }

        public void LoadData(Enumerators.GameDataType gameDataType)
        {
            string dataPath = _gameDataPathes[gameDataType];

            switch (gameDataType)
            {
                case Enumerators.GameDataType.UserData:
                    CachedUserLocalData = DeserializeFromPath<CachedUserData>(dataPath, gameDataType);
                    if (CachedUserLocalData == null)
                    {
                        CachedUserLocalData = new CachedUserData();
                        CachedUserLocalData.musicVolume = 1f;
                        CachedUserLocalData.soundVolume = 1f;
                        CachedUserLocalData.bestTimeSurvived = 0;

                        SaveData(Enumerators.GameDataType.UserData);
                    }

                    GameClient.Get<ISoundManager>().SoundVolume = CachedUserLocalData.soundVolume;
                    GameClient.Get<ISoundManager>().MusicVolume = CachedUserLocalData.musicVolume;
                    break;
              
                default: break;
            }
        }

        public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, JsonSerializerSettings);
        }

        public T DeserializeFromPath<T>(string path, Enumerators.GameDataType type) where T : class
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (!PlayerPrefs.HasKey(type.ToString()))
                return null;

            return JsonConvert.DeserializeObject<T>(Decrypt(PlayerPrefs.GetString(type.ToString())), JsonSerializerSettings);
#else
            if (!File.Exists(path))
                return null;

            return JsonConvert.DeserializeObject<T>(Decrypt(File.ReadAllText(path)), JsonSerializerSettings);
#endif
        }

        public string Serialize(object @object, Formatting formatting = Formatting.Indented)
        {
            return Encrypt(JsonConvert.SerializeObject(@object, formatting));
        }

        private string Decrypt(string data)
        {
            return Constants.DataEncrypted ? Utilites.Decrypt(data, StaticDataPrivateKey) : data;
        }

        private string Encrypt(string data)
        {
            return Constants.DataEncrypted ? Utilites.Encrypt(data, StaticDataPrivateKey) : data;
        }
    }
}