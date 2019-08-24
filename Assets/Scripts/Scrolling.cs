using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    public bool scrolling, paralax;

    public float backgroundSize;

    public float paralaxSpeed;

    private Transform cameraTransform;

    private Transform[] layers;

    public float viewZone = 10;

    private int leftIndex;

    private int rightIndex;

    private float lastCameraX;

	// Use this for initialization
	void Start ()
	{
	    cameraTransform = Camera.main.transform;
	    lastCameraX = cameraTransform.position.x;
	    layers = new Transform[transform.childCount];
	    for (int i = 0; i < transform.childCount; i++)
	    {
	        layers[i] = transform.GetChild(i);
	    }

	    leftIndex = 0;
	    rightIndex = layers.Length - 1;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
	    if (paralax)
	    {
	        float deltaX = cameraTransform.position.x - lastCameraX;
	        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right * (deltaX * paralaxSpeed),Time.deltaTime * 5);
        }

	    lastCameraX = cameraTransform.position.x;

        if (scrolling)
	    {
	        if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))
	        {
	            ScrollLeft();
	        }

	        if (cameraTransform.position.x > (layers[leftIndex].transform.position.x + viewZone))
	        {
	            ScrollRight();
	        }
        }
	}

    private void ScrollLeft()
    {
        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
        {
            rightIndex = layers.Length - 1;
        }
    }

    private void ScrollRight()
    {
        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }
    }
}
