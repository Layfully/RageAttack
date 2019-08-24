using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject Options;
    public GameObject GraphicsOptions;
    public GameObject AudioOptions;
    public GameObject InputOptions;
    public GameObject Main;
    public GameObject SceneSelector;
    public GameObject CharacterSelector;

    public TextMeshProUGUI[] InputKeyDisplay;

    public TMP_Dropdown ResolutionDropdown;
    public TMP_Dropdown ScreenModeDropdown;
    public TMP_Dropdown QualityDropdown;
    public TMP_Dropdown AntiAliasingDropdown;

    public Toggle ColorGradingToggle;
    public Toggle VignettingToggle;
    public Slider VolumeSlider;

    public TextMeshProUGUI ResolutionValue;

    private Event KeyEvent { get; set; }
    private KeyCode NewKey { get; set; }
    private bool WaitingForKey { get; set; }


    void Start()
    {
        List<string> resolutionOptions = new List<string>();

        int index = 0;
        int currentResolutionIndex = 0;

        foreach (var resolution in Screen.resolutions)
        {
            resolutionOptions.Add(string.Format("{0} x {1}", resolution.width, resolution.height));

            if (Screen.currentResolution.width == resolution.width && Screen.currentResolution.height == resolution.height)
            {
                currentResolutionIndex = index;
            }
            index++;
        }

        ResolutionDropdown.ClearOptions();
        ResolutionDropdown.AddOptions(resolutionOptions);
        ResolutionDropdown.value = currentResolutionIndex;

        QualityDropdown.ClearOptions();
        QualityDropdown.AddOptions(QualitySettings.names.ToList());
        QualityDropdown.value = QualitySettings.GetQualityLevel();

        ResolutionDropdown.onValueChanged.AddListener(delegate (int i)
        {
            string[] splitResolution = ResolutionDropdown.options[i].text.Split('x');
            Settings.Instance.GraphicsSettings.ScreenResolution = new Resolution
            {
                width = int.Parse(splitResolution[0]),
                height = int.Parse(splitResolution[1]),
                refreshRate = 60

            };
        });
        ScreenModeDropdown.onValueChanged.AddListener(delegate (int i)
        {
            Settings.Instance.GraphicsSettings.ScreenMode = i;
        });
        QualityDropdown.onValueChanged.AddListener(delegate (int i)
        {
            Settings.Instance.GraphicsSettings.QualitySettingsLevel = i;
        });
        AntiAliasingDropdown.onValueChanged.AddListener(delegate (int i)
        {
            Settings.Instance.GraphicsSettings.AntiAliasingMode = i;
        });

        Settings.Instance.GraphicsSettings.VignetteEnabled = VignettingToggle.isOn;
        Settings.Instance.GraphicsSettings.ColorGradingEnabled = ColorGradingToggle.isOn;

        GameManager.Instance._playerPrefabsList.Clear();

        WaitingForKey = false;

        for (int i = 0; i < InputKeyDisplay.Length; i++)
        {
            switch ((PredefiniedInput)i)
            {
                case PredefiniedInput.LeftP1:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("HorizontalP1").negative.ToString();
                    break;
                case PredefiniedInput.RightP1:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("HorizontalP1").positive.ToString();
                    break;
                case PredefiniedInput.LeftP2:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("HorizontalP2").negative.ToString();
                    break;
                case PredefiniedInput.RightP2:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("HorizontalP2").positive.ToString();
                    break;
                case PredefiniedInput.ShootProjectileP1:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("ShootProjectileP1").positive.ToString();
                    break;
                case PredefiniedInput.ShootProjectileP2:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("ShootProjectileP2").positive.ToString();
                    break;
                case PredefiniedInput.JumpP1:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("JumpP1").positive.ToString();
                    break;
                case PredefiniedInput.JumpP2:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("JumpP2").positive.ToString();
                    break;
                case PredefiniedInput.LoadChiP1:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("LoadChiP1").positive.ToString();
                    break;
                case PredefiniedInput.LoadChiP2:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("LoadChiP2").positive.ToString();
                    break;
                case PredefiniedInput.PunchP1:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("PunchP1").positive.ToString();
                    break;
                case PredefiniedInput.PunchP2:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("PunchP2").positive.ToString();
                    break;
                case PredefiniedInput.UltimateP1:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("UltimateP1").positive.ToString();
                    break;
                case PredefiniedInput.UltimateP2:
                    InputKeyDisplay[i].text = Settings.Instance.GetAxisSettings("UltimateP2").positive.ToString();
                    break;
                    
            }
        }

    }

    void OnGUI()
    {
        KeyEvent = Event.current;

        if (KeyEvent.isKey && WaitingForKey)
        {
            NewKey = KeyEvent.keyCode;
            WaitingForKey = false;
        }
    }

    public void LoadScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void DisplayMenu(bool display)
    {
        if (display)
        {
            Main.SetActive(false);
            Options.SetActive(true);
        }
        else
        {
            Main.SetActive(true);
            Options.SetActive(false);
        }
    }

    public void DisplaySceneSelector(bool display)
    {
        if (display)
        {
            SceneSelector.SetActive(true);
            CharacterSelector.SetActive(false);
        }
        else
        {
            GameManager.Instance._playerPrefabsList.Clear();

            SceneSelector.SetActive(false);
            CharacterSelector.SetActive(true);
        }
    }

    public void DisplayCharacterSelector(bool display)
    {
        if (display)
        {
            CharacterSelector.SetActive(true);
            Main.SetActive(false);
        }
        else
        {

            CharacterSelector.SetActive(false);
            Main.SetActive(true);
        }
    }

    public void DisplayGraphicsOptions(bool display)
    {
        if (display)
        {
            GraphicsOptions.SetActive(true);
            Options.SetActive(false);
        }
        else
        {
            GraphicsOptions.SetActive(false);
            Options.SetActive(true);
        }
    }

    public void DisplayAudioOptions(bool display)
    {
        if (display)
        {
            AudioOptions.SetActive(true);
            Options.SetActive(false);
        }
        else
        {
            AudioOptions.SetActive(false);
            Options.SetActive(true);
        }
    }

    public void DisplayOptions(bool display)
    {
        if (display)
        {
            Options.SetActive(true);
            Main.SetActive(false);
        }
        else
        {
            Options.SetActive(false);
            Main.SetActive(true);
        }
    }

    public void DisplayInputOptions(bool display)
    {
        if (display)
        {
            InputOptions.SetActive(true);
            Options.SetActive(false);
        }
        else
        {
            InputOptions.SetActive(false);
            Options.SetActive(true);
        }
    }

    IEnumerator WaitForKey()
    {
        while (!KeyEvent.isKey)
        {
            yield return null;
        }

    }

    private IEnumerator AssignKey(int index)
    {
        WaitingForKey = true;

        yield return WaitForKey(); //Executes endlessly until user presses a key


        switch ((PredefiniedInput)index)
        {
            case PredefiniedInput.LeftP1:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("HorizontalP1").negative = NewKey;
                break;
            case PredefiniedInput.RightP1:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("HorizontalP1").positive = NewKey;
                break;
            case PredefiniedInput.LeftP2:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("HorizontalP2").negative = NewKey;
                break;
            case PredefiniedInput.RightP2:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("HorizontalP2").positive = NewKey;
                break;
            case PredefiniedInput.ShootProjectileP1:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("ShootProjectileP1").positive = NewKey;
                break;
            case PredefiniedInput.ShootProjectileP2:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("ShootProjectileP2").positive = NewKey;
                break;
            case PredefiniedInput.JumpP1:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("JumpP1").positive = NewKey;
                break;
            case PredefiniedInput.JumpP2:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("JumpP2").positive = NewKey;
                break;
            case PredefiniedInput.LoadChiP1:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("LoadChiP1").positive = NewKey;
                break;
            case PredefiniedInput.LoadChiP2:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("LoadChiP2").positive = NewKey;
                break;
            case PredefiniedInput.PunchP1:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("PunchP1").positive = NewKey;
                break;
            case PredefiniedInput.PunchP2:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("PunchP2").positive = NewKey;
                break;
            case PredefiniedInput.UltimateP1:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("UltimateP1").positive = NewKey;
                break;
            case PredefiniedInput.UltimateP2:
                InputKeyDisplay[index].text = NewKey.ToString();
                Settings.Instance.GetAxisSettings("UltimateP2").positive = NewKey;
                break;
        }
        yield return null;
    }

    public void StartAssignment(int index)
    {
        InputKeyDisplay[index].text = "";

        if (!WaitingForKey)
            StartCoroutine(AssignKey(index));
    }

    public void EnableVignetting()
    {
        Settings.Instance.GraphicsSettings.VignetteEnabled = VignettingToggle.isOn;
    }

    public void EnableColorGrading()
    {
        Settings.Instance.GraphicsSettings.ColorGradingEnabled = ColorGradingToggle.isOn;
    }

    public void ChangeSoundVolume()
    {
        Settings.Instance.SoundSettings.MusicVolume = VolumeSlider.value;
        Settings.Instance.ChangeMusicVolume();
    }

    public void Apply()
    {

        Settings.Instance.ApplySettings();
    }
}
