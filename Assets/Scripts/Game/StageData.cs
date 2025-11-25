using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shooting Stage Data")]

public class StageData : ScriptableObject
{
    public List<WaveData> waves;
}
