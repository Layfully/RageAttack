using UnityEngine;

[CreateAssetMenu]
public class MovementData : ScriptableObject
{
    public int BulletCost;
    public int SpecialBulletCost;
    public int SpecialMultipleBulletCost;
    public int SpecialAboveCost;
    public int ChiLoadAmount;
    public float JumpVelocity;
    public float FallMultipler;
    public float LowJumpMultipler;
    public float Speed;
    public float DoublePressSpeed;
    public float TeleportationDistance;
    public float TeleportCooldown;
    public float ShootCooldown;
    public float MaxJumpVelocity;
    public float MinJumpVelocity;
    public float LongJumpTreshold;
    public float JumpLoadTime;
    public string JumpButtonName;
    public string ShootProjectileButtonName;
    public string PunchButtonName;
    public string UltimateButtonName;
    public string LoadChiButtonName;
    public string HorizontalAxisName;
    public LayerMask GroundLayer;
    public Vector4 VelocityTreshold;
}