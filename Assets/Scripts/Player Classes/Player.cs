using System;
using UnityEngine;

[RequireComponent(typeof(PlayerVitals))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerStatsHUD))]
[RequireComponent(typeof(PlayerTargetting))]

public class Player : MonoBehaviour
{
    public PlayerVitals PlayerVitals { get; private set; }
    public PlayerController PlayerController { get; private set; }
    public PlayerStatsHUD PlayerStatsHud { get; private set; }
    public PlayerTargetting PlayerTargetting { get; private set; }

    //Dependecies injected by GameManager.
    public PlayerStats PlayerStats { get; set; }
    public InputSettings InputSettings { get; set; }
    public MovementData MovementData { get; set; }

    //It is public so that all player components can subscribe to event.
    public EventHandler<PlayerVitals.DamagedEventArgs> DamagedHandler;
    public EventHandler<PlayerVitals.UsedChiEventArgs> UsedHandler;
    public EventHandler<PlayerVitals.LoadedChiEventArgs> LoadedHandler;

    void Awake()
    {
        PlayerVitals = GetComponent<PlayerVitals>();
        PlayerController = GetComponent<PlayerController>();
        PlayerStatsHud = GetComponent<PlayerStatsHUD>();
        PlayerTargetting = GetComponent<PlayerTargetting>();

        DamagedHandler += (sender, args) => OnDamaged(args.Amount);
        UsedHandler = (sender, args) => OnChiUsed(args.Amount);
        LoadedHandler = (sender, args) => OnChiLoaded(args.Amount);

        PlayerVitals.Damaged += DamagedHandler;
        PlayerVitals.LoadedEnergy += LoadedHandler;
        PlayerVitals.UsedEnergy += UsedHandler;
    }

    private void OnDamaged(int amount)
    {
        PlayerStats.DamageTaken += amount;
    }

    private void OnChiUsed(int amount)
    {
        PlayerStats.ChiSpent += amount;
    }

    private void OnChiLoaded(int amount)
    {
        PlayerStats.ChiRenewed += amount;
    }
}
