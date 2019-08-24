using UnityEngine;
using UnityEngine.PostProcessing;

public class Settings : MonoBehaviour
{
    public static Settings Instance;

    public DamageSettings DamageSettings;
    public SoundSettings SoundSettings;
    public GraphicsSettings GraphicsSettings;
    public InputSettings InputSettings;
    public ScreenShakeSettings ScreenShakeSettings;

    public PostProcessingProfile[] PostProcessingProfile = new PostProcessingProfile[4];

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeMusicVolume()
    {
        AudioListener.volume = SoundSettings.MusicVolume;
    }

    private void ChangeScreenResolution()
    {
        Screen.SetResolution(GraphicsSettings.ScreenResolution.width, GraphicsSettings.ScreenResolution.height, GraphicsSettings.ScreenMode == 1, GraphicsSettings.ScreenResolution.refreshRate);
    }

    private void ChangeAntiAliasingMode()
    {
        foreach (var postProcessingProfile in PostProcessingProfile)
        {
            var antialiasingSettings = postProcessingProfile.antialiasing.settings;
            antialiasingSettings.method = (AntialiasingModel.Method)GraphicsSettings.AntiAliasingMode;
            postProcessingProfile.antialiasing.settings = antialiasingSettings;
            postProcessingProfile.antialiasing.enabled = true;
        }
    }

    private void ChangeVignetteMode()
    {
        foreach (var postProcessingProfile in PostProcessingProfile)
        {
            postProcessingProfile.vignette.enabled = GraphicsSettings.VignetteEnabled;
        }
    }

    public void ChangeColorGradingMode()
    {
        foreach (var postProcessingProfile in PostProcessingProfile)
        {
            postProcessingProfile.colorGrading.enabled = GraphicsSettings.ColorGradingEnabled;
        }
    }

    private void ChangeQualitySettings()
    {
        QualitySettings.SetQualityLevel(GraphicsSettings.QualitySettingsLevel);
    }

    public AxisConfiguration GetAxisSettings(string axisName)
    {
        foreach (AxisConfiguration axis in InputSettings.Axes)
        {
            if (axis.name == axisName)
            {
                return axis;
            }
        }

        return null;
    }

    public void ApplySettings()
    {
        ChangeScreenResolution();
        ChangeQualitySettings();
        ChangeVignetteMode();
        ChangeAntiAliasingMode();
        ChangeMusicVolume();
    }
}
