
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shooting Stage Wave Data")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class SpawnEvent
    {
        public float time;
        public EnemyBase enemyPrefab;
        [Range(0f, 1f)]
        public float viewportY;
    }

    public List<SpawnEvent> events;

}
