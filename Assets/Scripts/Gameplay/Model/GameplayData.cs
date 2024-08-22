using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplayData", menuName = "HotForgeStudio/HorrorBox/GameplayData", order = 3)]
public class GameplayData : ScriptableObject
{
    [Header("Player")]
    public Vector3 playerSpawnPosition = new Vector3(0f, 0.85f, 0f);
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
        public float enemySpawnPositionLimit = 35f;

        public float enemySpawnMinDistanceFromPlayer = 15f;
        public float enemySpawnMaxDistanceFromPlayer = 60f;

        public float cubeSpawnTime = 7f;
        public float cubeSpawnTimeDecrease = 0.2f;

        public float cubeDefaultSpeed = 2f;
        public float cubeSpeedIncrease = 0.2f;

        public float cubeChangeDataTime = 5f;

        public int maxSpawnedCubes = 30;
        public int maxSpawnedBombs = 40;
    }
}