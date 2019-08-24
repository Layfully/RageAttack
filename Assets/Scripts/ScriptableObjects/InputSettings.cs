using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class InputSettings : ScriptableObject
{
    public List<AxisConfiguration> Axes;
    public InputSettings()
    {
        Axes = new List<AxisConfiguration>();
    }
}
