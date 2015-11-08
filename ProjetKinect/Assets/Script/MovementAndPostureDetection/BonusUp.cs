using UnityEngine;
using System.Collections;

public class BonusUp : MonoBehaviour {

    private GameObject RightHand;
    private GameObject LeftHand;
    private GameObject CenterHip;
    private GameObject Head;

    // Vitesse minimale pour la détection du mouvement vers la gauche
    [SerializeField]
    private float minSpeedYMvtUp = -1.8f;

    private Vector3 previousPosition;
    private Vector3 currentSpeed;

    IEnumerator CalculateSpeed()
    {
        while (Application.isPlaying)
        {
            previousPosition = Head.transform.position;
            yield return new WaitForEndOfFrame();
            currentSpeed = (previousPosition - Head.transform.position) / Time.deltaTime;
        }
    }

    // Use this for initialization
    void Start ()
    {
        Head = GameObject.Find("03_Head");

        if (Head == null)
        {
            Debug.Log("pas de tete trouvée");
        }

        RightHand = GameObject.Find("23_Hand_Right");
        LeftHand = GameObject.Find("13_Hand_Left");
        CenterHip = GameObject.Find("00_Hip_Center");
        StartCoroutine(CalculateSpeed());
    }
	
	// Update is called once per frame
	void Update ()
    {
        if ((currentSpeed.y < minSpeedYMvtUp && RightHand.transform.position.y < CenterHip.transform.position.y && LeftHand.transform.position.y < CenterHip.transform.position.y)
                || Input.GetKey(KeyCode.Alpha1)
                || Input.GetKey(KeyCode.Keypad1))
        {
            KinectManager.Instance.postureBonusUp();
            Debug.Log("BonusUp");
        }
    }
}
