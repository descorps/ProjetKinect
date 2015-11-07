using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BonusDown : MonoBehaviour {

    private GameObject RightShoulder;
    private GameObject LeftShoulder;
    private GameObject RightElbow;
    private GameObject LeftElbow;
    private GameObject RightWrist;
    private GameObject LeftWrist;

    private List<float> listCoordsY;

    [SerializeField]
    private float minDistanceHands = 1.1f;

    private float maxPosY;
    private float minPosY;

    private float getMax(List<float> list)
    {
        float max = list[0];
        for (int i = 0; i < list.Count - 1; i++)
        {
            if (max < list[i])
                max = list[i];
        }
        return max;
    }

    private float getMin(List<float> list)
    {
        float min = list[0];
        for (int i = 0; i < list.Count - 1; i++)
        {
            if (min > list[i])
                min = list[i];
        }
        return min;
    }

    // Use this for initialization
    void Start ()
    {
        RightShoulder = GameObject.Find("20_Shoulder_Right");
        LeftShoulder = GameObject.Find("10_Shoulder_Left");
        RightElbow = GameObject.Find("21_Elbow_Right");
        LeftElbow = GameObject.Find("11_Elbow_Left");
        RightWrist = GameObject.Find("22_Wrist_Right");
        LeftWrist = GameObject.Find("12_Wrist_Left");
        listCoordsY = new List<float>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        listCoordsY.Add(RightShoulder.transform.position.y);
        listCoordsY.Add(LeftShoulder.transform.position.y);
        listCoordsY.Add(RightElbow.transform.position.y);
        listCoordsY.Add(LeftElbow.transform.position.y);
        listCoordsY.Add(RightWrist.transform.position.y);
        listCoordsY.Add(LeftWrist.transform.position.y);

        maxPosY = getMax(listCoordsY);
        minPosY = getMin(listCoordsY);

        if ((maxPosY - minPosY < 0.2 && maxPosY < 1 && RightWrist.transform.position.x - LeftWrist.transform.position.x > minDistanceHands)
                || Input.GetKey(KeyCode.Alpha2)
                || Input.GetKey(KeyCode.Keypad2))
        {
            KinectManager.Instance.postureBonusDown();
            Debug.Log("BonusDown");
        }

        listCoordsY.Clear();
    }
}
