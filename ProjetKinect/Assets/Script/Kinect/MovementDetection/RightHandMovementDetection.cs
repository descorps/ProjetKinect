using UnityEngine;
using System.Collections;

using System;

public class RightHandMovementDetection : MonoBehaviour
{

    private Vector3 previousPosition;
    private Vector3 currentSpeed;

    IEnumerator CalculateSpeed()
    {
        while (Application.isPlaying)
        {
            previousPosition = transform.position;
            yield return new WaitForEndOfFrame();
            currentSpeed = (previousPosition - transform.position) / Time.deltaTime;
            //Debug.Log(currentSpeed);
        }
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(CalculateSpeed());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpeed.x < -1.5 && Math.Abs(currentSpeed.y) < 0.5 && transform.position.x > 0.3)
        {
            KinectManager.Instance.rightHandTowardRight();
            //Debug.Log("main droite vers la droite");
        }
        if (currentSpeed.y < -1.5 && Math.Abs(currentSpeed.x) < 0.7 && transform.position.y > 1)
        {
            KinectManager.Instance.rightHandTowardUp();
           // Debug.Log("main droite vers le haut");
        }
    }
}
