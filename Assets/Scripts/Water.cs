using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Water : MonoBehaviour
{
    private Coroutine DamageOverTime { get; set; }
    private List<Player> PlayersInWater { get; set; } 

    void Start()
    {
        PlayersInWater = new List<Player>();
    }

    IEnumerator DamagePlayers()
    {
        while (true)
        {
            yield return new WaitForSeconds(Settings.Instance.DamageSettings.WaterDamageTimer);

            foreach (Player player in PlayersInWater)
            {
                player.PlayerVitals.TakeDamage(Settings.Instance.DamageSettings.WaterDamage);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        
        if (!PlayersInWater.Contains(player) && player != null)
        {
            PlayersInWater.Add(player);

            if (DamageOverTime == null)
            {
                DamageOverTime = StartCoroutine(DamagePlayers());
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (PlayersInWater.Contains(player) && player != null)
        {
            PlayersInWater.Remove(player);

            if (PlayersInWater.Count == 0)
            {
                StopCoroutine(DamageOverTime);
                DamageOverTime = null;
            }
        }
    }
}
