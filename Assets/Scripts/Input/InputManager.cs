using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; set; }

    public static bool Enabled = true;

    public InputSettings InputSettings;

    private Dictionary<string, AxisConfiguration> _axesDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
        InputSettings = Settings.Instance.InputSettings;

        _axesDictionary = new Dictionary<string, AxisConfiguration>();
        _axesDictionary.Clear();

        foreach (AxisConfiguration axisConfig in InputSettings.Axes)
        {
            if (!_axesDictionary.ContainsKey(axisConfig.name))
            {
                _axesDictionary.Add(axisConfig.name, axisConfig);
            }
            else
            {
                Debug.LogWarning(string.Format("Input configuration already contains an axis named \'{0}\'", axisConfig.name));
            }
        }
    }

    void Update()
    {
        if (Enabled)
        {
            foreach (AxisConfiguration axis in InputSettings.Axes)
            {
                axis.Update();
            }
        }
    }

    public static float GetAxis(string axisName)
    {
        if (Enabled)
        {
            AxisConfiguration axisConfig;
            if (Instance._axesDictionary.TryGetValue(axisName, out axisConfig))
            {
                return axisConfig.GetAxis();
            }
        }
        return 0;
    }

    public static float GetAxisRaw(string axisName)
    {
        if (Enabled)
        {
            AxisConfiguration axisConfig;
            if (Instance._axesDictionary.TryGetValue(axisName, out axisConfig))
            {
                return axisConfig.GetAxisRaw();
            }
        }
        return 0;
    }

    public static bool GetButton(string buttonName)
    {
        if (Enabled)
        {
            AxisConfiguration axisConfig;
            if (Instance._axesDictionary.TryGetValue(buttonName, out axisConfig))
            {
                return axisConfig.GetButton();
            }
        }
        return false;
    }

    public static bool GetButtonDown(string buttonName)
    {
        if (Enabled)
        {
            AxisConfiguration axisConfig;
            if (Instance._axesDictionary.TryGetValue(buttonName, out axisConfig))
            {
                return axisConfig.GetButtonDown();
            }
        }
        return false;
    }

    public static bool GetButtonUp(string buttonName)
    {
        if (Enabled)
        {
            AxisConfiguration axisConfig;
            if (Instance._axesDictionary.TryGetValue(buttonName, out axisConfig))
            {
                return axisConfig.GetButtonUp();
            }
        }
        return false;
    }
}
