using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    public List<Transform> Targets = new List<Transform>();
    public static bool IsShaking { private get; set; }

    [SerializeField]
    private Background _background;
    [SerializeField]
    private BoxCollider2D _bottomSnapCollider;
    [SerializeField]
    private CameraSettings _mySettings;

    //Cache
    private float _minCameraY;
    private Transform _myTransform;
    private Camera _mainCamera;
    private Bounds _cameraBounds;

    void Start()
    {
        _mainCamera = Camera.main;
        _myTransform = transform;
        enabled = _mySettings.Enabled;
    }

    void LateUpdate()
    {
        if (Targets.Count == 0)
        {
            return;
        }

        if (!IsShaking)
        {
            GetCameraBounds();
            Zoom();
            Move();
        }

        _background.ResizeSpriteToScreen();
    }

    private void Zoom()
    {
        _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, Mathf.Lerp(_mySettings.MinCameraSize, _mySettings.MaxCameraSize, GetLargestDistance() / 50), Time.deltaTime * _mySettings.LerpSpeed);
    }

    private void Move()
    {
        _myTransform.position = GetCenterPoint();
    }

    private void GetCameraBounds()
    {
        _minCameraY = _mainCamera.orthographicSize - _bottomSnapCollider.bounds.size.y / 2.0f + _bottomSnapCollider.transform.position.y;
    }

    private Vector3 GetCenterPoint()
    {
        if (Targets.Count == 1)
        {
            return Targets[0].position;
        }

        _cameraBounds = new Bounds(Targets[0].position, Vector3.zero);
        for (int i = 0; i < Targets.Count; i++)
        {
            _cameraBounds.Encapsulate(Targets[i].position);
        }

        return new Vector3(_cameraBounds.center.x, Mathf.Clamp(_cameraBounds.center.y, _minCameraY, 1000), -10);
    }

    private float GetLargestDistance()
    {
        _cameraBounds = new Bounds(Targets[0].position, Vector3.zero);  
        for (int i = 0; i < Targets.Count; i++)
        {
            _cameraBounds.Encapsulate(Targets[i].position);
        }

        return _cameraBounds.size.x;
    }
}
