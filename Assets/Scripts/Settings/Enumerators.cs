namespace HotForgeStudio.HorrorBox.Common
{
    public class Enumerators
    {
        public enum AppState
        {
            Unknown,

            AppStart,
            Main,
            Game,
            GameOver
        }

        public enum InputType
        {
            Unknown,

            Mouse,
            Keyboard,
            Swipe,
            Joystick
        }

        public enum AxisOptions 
        { 
            Both,
            Horizontal,
            Vertical
        }

        public enum SoundType
        {
            Unknown,

            MainMusic,
            Explosion
        }

        public enum GameDataType
        {
            Unknown,

            UserData
        }

        public enum Direction
        {
            Unknown,

            Left,
            Right,
            Up,
            Down
        }

        public enum EnemyType
        {
            Unknown,

            Cube,
            Bomb
        }
    }
}