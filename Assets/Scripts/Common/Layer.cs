using UnityEngine;

public static class Layers
{
    public static readonly int Player = LayerMask.NameToLayer("Player");
    public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
    public static readonly int PlayerBullet = LayerMask.NameToLayer("PlayerBullet");
    public static readonly int EnemyBullet = LayerMask.NameToLayer("EnemyBullet");
    public static readonly int Hazard = LayerMask.NameToLayer("Hazard");
}
