using UnityEngine;
using System.Collections;

using System;

public class LeftHandMovementDetection : MonoBehaviour {

    // Vitesse minimale pour la détection du mouvement vers la gauche
    [SerializeField]
    private float minSpeedXMvtLeft = 1.5f;

    // Vitesse minimale pour la détection du mouvement vers le haut
    [SerializeField]
    private float minSpeedYMvtUp = -1;

    // Vitesse max que peut prendre la main dans les directions non voulues pendant le mouvement vers la gauche
    [SerializeField]
    private float maxSpeed = 0.6f;

    // Position minimal délimitant la zone de détection de la main durant le mouvement vers la gauche
    [SerializeField]
    private float minPositionXMvtLeft = -0.3f;

    // Position minimal délimitant la zone de détection de la main durant le mouvement vers le haut
    [SerializeField]
    private float minPositionYMvtUp = 1;


    private Vector3 previousPosition;
    private Vector3 currentSpeed;

    IEnumerator CalculateSpeed()
    {
        while (Application.isPlaying)
        {
            previousPosition = transform.position;
            yield return new WaitForEndOfFrame() ;
            currentSpeed = (previousPosition - transform.position) / Time.deltaTime;
            //Debug.Log(currentSpeed);
        }
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(CalculateSpeed());
	}
	
	// Update is called once per frame
	void Update () {
        if (currentSpeed.x > minSpeedXMvtLeft && Math.Abs(currentSpeed.y) < maxSpeed && transform.position.x < minPositionXMvtLeft)
        {
            KinectManager.Instance.leftHandTowardLeft();
            //Debug.Log("main gauche vers la gauche");
        }
        if (currentSpeed.y < minSpeedYMvtUp && Math.Abs(currentSpeed.x) < maxSpeed && transform.position.y > minPositionYMvtUp)
        {
            KinectManager.Instance.leftHandTowardUp();
            //Debug.Log("main gauche vers le haut");
        }

    }
}
