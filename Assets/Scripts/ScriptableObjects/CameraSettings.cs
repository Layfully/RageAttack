using UnityEngine;

[CreateAssetMenu]
public class CameraSettings : ScriptableObject
{
    public int MinCameraSize;
    public int MaxCameraSize;
    public float LerpSpeed;
    public bool Enabled;
}
