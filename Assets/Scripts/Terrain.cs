using System.Collections;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField] private Sprite _startMapImage;
    [SerializeField] private PhysicsMaterial2D _physicsMaterial;
    [SerializeField] private float _colliderGenerationTime;

    private Sprite CurrentMapImage { get; set; }
    public bool IsGenerating { get; private set; }
    public Vector2 TerrainOffset { get { return gameObject.transform.position; } }
    public SpriteRenderer TerrainRenderer { get; private set; }

    private void Awake()
    {
        TerrainRenderer = GetComponent<SpriteRenderer>();
        IsGenerating = false;
        SetupTerrainTexture();
    }
    /// <summary>
    /// 
    /// 
    /// <param name="texture"></param>
    /// <param name="cx"> Współrzędna x środka koła</param>
    /// <param name="cy"> Współrzędna y środka koła</param>
    /// <param name="radius"></param>
    /// <param name="color"></param>
    /// </summary>
    public void DrawCircle(Texture2D texture, int cx, int cy, int radius, Color color)
    {
        int width = texture.width;

        Color32[] pixelColors = texture.GetPixels32();

        for (int x = 0; x <= radius; x++)
        {
            int end = (int)Mathf.Ceil(Mathf.Sqrt(radius * radius - x * x));

            for (int y = 0; y <= end; y++)
            {
                int px = Mathf.Clamp(cx + x, 0, width);         //clamp values so that it doesn't draw on the other side of texture
                int nx = Mathf.Clamp(cx - x, 0, width);

                int py = cy + y;
                int ny = cy - y;

                if (py * width + px < pixelColors.Length)
                    pixelColors[py * width + px] = color;
                if (py * width + nx < pixelColors.Length)
                    pixelColors[py * width + nx] = color;
                if (ny * width + px < pixelColors.Length && ny * width + px > 0)
                    pixelColors[ny * width + px] = color;
                if (ny * width + nx < pixelColors.Length && ny * width + nx > 0)
                    pixelColors[ny * width + nx] = color;
            }
        }
        texture.SetPixels32(pixelColors);
        texture.Apply();
    }

    private void SetupTerrainTexture()
    {
        Texture2D tex = new Texture2D(_startMapImage.texture.width, _startMapImage.texture.height, _startMapImage.texture.format, false);
        tex.SetPixels(_startMapImage.texture.GetPixels());
        tex.Apply();
        CurrentMapImage = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
        TerrainRenderer.sprite = CurrentMapImage;

    }
    
    public IEnumerator GenerateCollider(GameObject bulletGameObject)
    {
        IsGenerating = true;

        yield return new WaitForSeconds(_colliderGenerationTime);

        Destroy(GetComponent<PolygonCollider2D>());

        PolygonCollider2D myCollider = gameObject.AddComponent<PolygonCollider2D>();
        myCollider.sharedMaterial = _physicsMaterial;

        IsGenerating = false;
        Destroy(bulletGameObject);
    }

    public IEnumerator GenerateCollider()
    {
        IsGenerating = true;

        yield return new WaitForSeconds(_colliderGenerationTime);

        Destroy(GetComponent<PolygonCollider2D>());

        PolygonCollider2D myCollider = gameObject.AddComponent<PolygonCollider2D>();
        myCollider.sharedMaterial = _physicsMaterial;

        IsGenerating = false;
    }
}