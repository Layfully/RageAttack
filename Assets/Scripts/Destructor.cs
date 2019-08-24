using UnityEngine;

public abstract class Destructor : MonoBehaviour {

    [SerializeField] private DestroySettings _mySettings;

    protected void DrawTransparencyOnTerrain(Terrain terrain, Collision2D col)
    {
        Texture2D tex = terrain.TerrainRenderer.sprite.texture;
        if (tex != null)
        {
            float textureLength = tex.width / 100.0f;
            float percentTextureWidth = tex.width / 100.0f;

            float targetX = ((col.contacts[0].point.x - terrain.TerrainOffset.x) / textureLength * 100 + 50) * percentTextureWidth;

            float textureHeight = tex.height / 100.0f;
            float percentTextureHeight = tex.height / 100.0f;

            float targetY = ((col.contacts[0].point.y - terrain.TerrainOffset.y) / textureHeight * 100 + 50) * percentTextureHeight;

            terrain.DrawCircle(tex, Mathf.RoundToInt(targetX), Mathf.RoundToInt(targetY), _mySettings.HoleSize, Color.clear);
        }
    }
}
