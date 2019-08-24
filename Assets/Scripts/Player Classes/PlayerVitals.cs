using System;
using UnityEngine;

public class PlayerVitals : MonoBehaviour
{
    private int CurrentHealth { get; set; }
    public int MaximumHealth { get; private set; }

    public int CurrentChi { get; private set; }
    public int MaximumChi { get; private set; }

    public event EventHandler<DamagedEventArgs> Damaged;
    public event EventHandler<UsedChiEventArgs> UsedEnergy;
    public event EventHandler<LoadedChiEventArgs> LoadedEnergy;

    private LoadedChiEventArgs ChiLoadedEventArguments { get; set; }
    private UsedChiEventArgs ChiUsedEventArguments { get; set; }
    private DamagedEventArgs DamagedEventArguments { get; set; }

    private void Awake()
    {
        MaximumHealth = 100;
        MaximumChi = 100;
        CurrentChi = MaximumChi;
        CurrentHealth = MaximumHealth;
        ChiLoadedEventArguments = new LoadedChiEventArgs();
        ChiUsedEventArguments = new UsedChiEventArgs();
        DamagedEventArguments = new DamagedEventArgs();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Math.Max(CurrentHealth - amount, 0);

        if (Damaged != null)
        {
            DamagedEventArguments.Amount = amount;
            DamagedEventArguments.CurrentHealth = CurrentHealth;

            Damaged(this, DamagedEventArguments);
        }
    }

    public void UseChi(int amount)
    {
        CurrentChi = Math.Max(CurrentChi - amount, 0);

        if (UsedEnergy != null)
        {
            ChiUsedEventArguments.Amount = amount;
            ChiUsedEventArguments.CurrentChi = CurrentChi;
            UsedEnergy(this, ChiUsedEventArguments);
        }
    }

    public void LoadChi(int amount)
    {
        CurrentChi = Math.Min(CurrentChi + amount, MaximumChi);

        if (LoadedEnergy != null)
        {
            ChiLoadedEventArguments.Amount = amount;
            ChiLoadedEventArguments.CurrentChi = CurrentChi;
            LoadedEnergy(this, ChiLoadedEventArguments);
        }
    }

    public class DamagedEventArgs : EventArgs
    {
        public int Amount { get; set; }
        public int CurrentHealth { get; set; }

        public DamagedEventArgs(int amount, int currentHealth)
        {
            Amount = amount;
            CurrentHealth = currentHealth;
        }

        public DamagedEventArgs()
        {
            Amount = 0;
            CurrentHealth = 0;
        }
    }

    public class LoadedChiEventArgs : EventArgs
    {
        public int Amount { get; set; }
        public int CurrentChi { get; set; }

        public LoadedChiEventArgs(int amount, int currentChi)
        {
            Amount = amount;
            CurrentChi = currentChi;
        }

        public LoadedChiEventArgs()
        {
            Amount = 0;
            CurrentChi = 0;
        }
    }

    public class UsedChiEventArgs : EventArgs
    {
        public int Amount { get; set; }
        public int CurrentChi { get; set; }

        public UsedChiEventArgs(int amount, int currentChi)
        {
            Amount = amount;
            CurrentChi = currentChi;
        }

        public UsedChiEventArgs()
        {
            Amount = 0;
            CurrentChi = 0;
        }
    }
}
