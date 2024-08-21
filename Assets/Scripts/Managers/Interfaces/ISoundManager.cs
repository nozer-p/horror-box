using HotForgeStudio.HorrorBox.Common;

namespace HotForgeStudio.HorrorBox
{
    public interface ISoundManager
    {
        SoundData SoundData { get; }
        float SoundVolume { get; set; }
        float MusicVolume { get; set; }

        void PlaySound(Enumerators.SoundType soundType);

        void StopSound(Enumerators.SoundType soundType);
    }
}