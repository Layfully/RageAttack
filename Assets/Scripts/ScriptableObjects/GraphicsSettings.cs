using UnityEngine;

[CreateAssetMenu]
public class GraphicsSettings : ScriptableObject 
{
    public int ScreenMode;
    public int AntiAliasingMode;
    public int QualitySettingsLevel;
    [SerializeField]
    public Resolution ScreenResolution;
    public bool VignetteEnabled;
    public bool ColorGradingEnabled;

}
