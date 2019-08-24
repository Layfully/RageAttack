using System;
using UnityEngine.UI;
using UnityEngine;

public class PlayerStatsHUD : MonoBehaviour
{
    public Slider HealthBar { private get; set; }
    public Slider ChiBar { private get; set; }
    private Player MyPlayer { get; set; }

    private EventHandler<PlayerVitals.LoadedChiEventArgs> _loadedEnergyHandler;
    private EventHandler<PlayerVitals.UsedChiEventArgs> _usedEnergyHandler;
    private EventHandler<PlayerVitals.DamagedEventArgs> _damagedHandler;

    void OnEnable()
    {
        MyPlayer = GetComponent<Player>();

        _loadedEnergyHandler = (sender, args) => UpdateChiBar(args.CurrentChi);
        _usedEnergyHandler = (sender, args) => UpdateChiBar(args.CurrentChi);
        _damagedHandler = (sender, args) => UpdateHealthBar(args.CurrentHealth);

        MyPlayer.PlayerVitals.LoadedEnergy += _loadedEnergyHandler;
        MyPlayer.PlayerVitals.UsedEnergy += _usedEnergyHandler;
        MyPlayer.PlayerVitals.Damaged += _damagedHandler;
    }

    void OnDisable()
    {
        MyPlayer.PlayerVitals.LoadedEnergy -= _loadedEnergyHandler;
        MyPlayer.PlayerVitals.UsedEnergy -= _usedEnergyHandler;
        MyPlayer.PlayerVitals.Damaged -= _damagedHandler;
    }

    void Awake()
    {
        MyPlayer = GetComponent<Player>();
    }

    void Start()
    {
        HealthBar.maxValue = MyPlayer.PlayerVitals.MaximumHealth;
        ChiBar.maxValue = MyPlayer.PlayerVitals.MaximumChi;
        HealthBar.value = HealthBar.maxValue;
        ChiBar.value = ChiBar.maxValue;
    }

    private void UpdateHealthBar(int value)
    {
        HealthBar.value = value;
    }

    private void UpdateChiBar(int value)
    {
        ChiBar.value = value;
    }
}
