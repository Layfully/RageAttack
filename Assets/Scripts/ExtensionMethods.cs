using System.Collections;
using UnityEngine;

public static class ExtensionMethods
{
    private static readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    public static IEnumerator Shake(this Camera camera, float duration, float magnitude, float frequency)
    {
       
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            Vector3 offset = new Vector3(Mathf.Clamp01(Mathf.PerlinNoise(Time.time * frequency, 0f) - 0.5f), Mathf.Clamp01(Mathf.PerlinNoise(0f, Time.time * frequency)) - 0.5f, 0);
            offset *= magnitude;

            camera.transform.position -= offset;
            Camera2D.IsShaking = true;
            yield return new WaitForEndOfFrame();
            camera.transform.position += offset;
            Camera2D.IsShaking = false;
        }
    }

    public static IEnumerator Shake(this Camera camera)
    {
        ScreenShakeSettings settings = Settings.Instance.ScreenShakeSettings;

        for (float time = 0; time < settings.Duration ; time += Time.deltaTime)
        {
            Vector3 offset = new Vector3(Mathf.Clamp01(Mathf.PerlinNoise(Time.time * settings.Frequency, 0f) - 0.5f), Mathf.Clamp01(Mathf.PerlinNoise(0f, Time.time * settings.Frequency)) - 0.5f, 0);
            offset *= settings.Magnitude;

            camera.transform.position -= offset;
            Camera2D.IsShaking = true;
            yield return new WaitForEndOfFrame();
            camera.transform.position += offset;
            Camera2D.IsShaking = false;
        }
    }

    public static IEnumerator RotateConstantly(this Transform transform, Vector3 rotation, float rotationSpeed)
    {
        while (true)
        {
            transform.Rotate(rotation * Time.deltaTime * rotationSpeed);
            transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);
            yield return waitForEndOfFrame;
        }
    }

    public static void FollowTargetWithRotation(this Rigidbody2D myRigidbody, Transform target, float distanceToStop, float speed)
    {
        if (Vector2.Distance(myRigidbody.transform.position, target.position) > distanceToStop)
        {
            Vector2 direction = (target.position - myRigidbody.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            myRigidbody.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            myRigidbody.AddForce(direction * speed, ForceMode2D.Force);
        }
    }
}
