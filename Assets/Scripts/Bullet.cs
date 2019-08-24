using System.Collections;
using UnityEngine;

public class Bullet : Destructor
{
    public Player MyPlayer { private get; set; }
    private Rigidbody2D MyRigidbody { get; set; }
    [SerializeField]
    private BulletSettings _myBulletSettings;
    public bool IsSpecial { get; set; }
    public bool Follow { get; set; }

    private void Start()
    {
        MyRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (Follow)
        {
            MyRigidbody.FollowTargetWithRotation(MyPlayer.PlayerTargetting.EnemyTransform, _myBulletSettings.DistanceToStop, _myBulletSettings.Speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("terrain"))
        {
            Terrain terrain = col.gameObject.GetComponent<Terrain>();
            if (terrain != null)
            {
                DrawTransparencyOnTerrain(terrain, col);

                GetComponent<Rigidbody2D>().simulated = false;
                GetComponent<SpriteRenderer>().enabled = false;

                StartCoroutine(ShakeAndGenerateColider(terrain));
            }
            else
            {
                Destroy(gameObject);
            }
        }

        else if (col.gameObject.CompareTag("Player"))
        {
            if (IsSpecial)
            {
                MyPlayer.PlayerTargetting.EnemyPlayerStats.TakeDamage(Settings.Instance.DamageSettings
                    .SpecialProjectileDamage);
            }
            else
            {
                MyPlayer.PlayerTargetting.EnemyPlayerStats.TakeDamage(Settings.Instance.DamageSettings
                    .ProjectileDamage);
            }
            Destroy(gameObject);
        }

    }

    private IEnumerator ShakeAndGenerateColider(Terrain terrain)
    {
        yield return StartCoroutine(Camera.main.Shake());

        if (!terrain.IsGenerating)
        {
            StartCoroutine(terrain.GenerateCollider(gameObject));
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnableGravity()
    {
        GetComponent<Rigidbody2D>().gravityScale = 1;
        transform.localRotation = Quaternion.Euler(new Vector3(0,0,270));
    }
}
