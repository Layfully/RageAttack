using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public int DamageTaken;
    public int ChiSpent;
    public int ChiRenewed;
    public int ProjectileCounter;
    public int JumpCounter;
    public int TeleportationCounter;

    public string[] GenerateStatsStrings()
    {
        return new[] { DamageTaken.ToString(), ChiSpent.ToString(), ChiRenewed.ToString(), ProjectileCounter.ToString(), JumpCounter.ToString(), TeleportationCounter.ToString() };
    }

    public void Reset()
    {
        DamageTaken = 0;
        ChiSpent = 0;
        ChiRenewed = 0;
        ProjectileCounter = 0;
        JumpCounter = 0;
        TeleportationCounter = 0;
    }
}
