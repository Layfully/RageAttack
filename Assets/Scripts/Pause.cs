using System;
using TMPro;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public TextMeshProUGUI[] InputKeyDisplay;


    public void LoadMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePause();
        }
    }

    private void Start()
    {
        for (int i = 0; i < InputKeyDisplay.Length; i++)
        {
            if (InputKeyDisplay[i].text != null)
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
    }

    public void Mute()
    {
        if (Math.Abs(AudioListener.volume) < 0.05)
        {
            AudioListener.volume = Settings.Instance.SoundSettings.MusicVolume;

        }
        else
        {
            AudioListener.volume = 0;
        }
    }
}
