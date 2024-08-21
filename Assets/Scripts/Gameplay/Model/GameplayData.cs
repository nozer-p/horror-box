using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplayData", menuName = "HotForgeStudio/HorrorBox/GameplayData", order = 3)]
public class GameplayData : ScriptableObject
{
    [Header("Player")]
    public Vector3 playerSpawnPosition = new Vector3(0f, 0.8f, 0f);
    public float playerSpeed = 4.8f;

    [Header("Enemy")]
    [SerializeField]
    public EnemyInfo enemyInfo;

    [Header("Camera")]
    public float cameraDistance = 7f;
    public float cameraMovementSpeed = 4f;

    [Serializable]
    public class EnemyInfo
    {
        public Vector2 enemySpawnPosition = new Vector2(35f, 35f);

        public float enemySpawnMinDistanceFromPlayer = 7f;
        public float enemySpawnMaxDistanceFromPlayer = 20f;

        public float cubeSpawnTime = 8f;
        public float cubeSpawnTimeDecrease = 0.2f;

        public float cubeDefaultSpeed = 3f;
        public float cubeSpeedIncrease = 0.3f;
        public float cubeIncreaseSpeedTime = 5f;

        public int maxSpawnedBombs = 10;
    }
}