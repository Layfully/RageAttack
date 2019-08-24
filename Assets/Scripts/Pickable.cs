using System.Collections;
using UnityEngine;

public class Pickable : Destructor
{
    [SerializeField] private bool _rotate;

    public Player MyPlayer { private get; set; }
    public bool Thrown { private get; set; }
    private Coroutine RotateCoroutine { get; set; }

    public bool Rotate
    {
        get
        {
            return _rotate;
        }
        set
        {
            if (value)
            {
                RotateCoroutine = StartCoroutine(transform.RotateConstantly(Vector3.down, 100));
            }
            else
            {
                if (RotateCoroutine != null)
                {
                    StopCoroutine(RotateCoroutine);
                }
                RotateCoroutine = null;
            }
            _rotate = value;
        }
    }

    void Start()
    {
        Thrown = false;
        RotateCoroutine = StartCoroutine(transform.RotateConstantly(Vector3.down, 100));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (Thrown)
        {
            if (col.gameObject.CompareTag("terrain"))
            {
                Terrain terrain = col.gameObject.GetComponent<Terrain>();
                DrawTransparencyOnTerrain(terrain, col);

                StartCoroutine(ShakeAndGenerateColider(terrain));
            }
            else if (col.gameObject.CompareTag("Player"))
            {
                MyPlayer.PlayerTargetting.EnemyPlayerStats.TakeDamage(Settings.Instance.DamageSettings.ThrowDamage);
            }

            Thrown = false;
            Rotate = true;
        }
    }

    private IEnumerator ShakeAndGenerateColider(Terrain terrain)
    {
        yield return StartCoroutine(Camera.main.Shake());

        if (!terrain.IsGenerating)
        {
            StartCoroutine(terrain.GenerateCollider());
        }
    }
}
