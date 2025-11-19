using UnityEngine;

public class SpikeFloor : MonoBehaviour, IDamageSource
{
    [SerializeField] private int contactDamage = 2;
    public int Damage => contactDamage;
}
