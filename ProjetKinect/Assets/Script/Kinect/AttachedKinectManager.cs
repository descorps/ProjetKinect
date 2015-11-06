using UnityEngine;
using System.Collections;

public class AttachedKinectManager : MonoBehaviour {

    private KinectPointController kinectPointController;
    private SkeletonWrapper skeletonWrapper;

    // Use this for initialization
    void Start () {
        kinectPointController = gameObject.GetComponent<KinectPointController>();
        skeletonWrapper = GameObject.Find("KinectPrefab").GetComponent<SkeletonWrapper> ();
        if ( skeletonWrapper == null)
        {
            Debug.Log("skeletonWrapper null");
        }
        kinectPointController.sw = skeletonWrapper;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
