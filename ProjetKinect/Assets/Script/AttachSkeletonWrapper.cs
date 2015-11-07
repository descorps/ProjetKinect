using UnityEngine;
using System.Collections;

public class AttachSkeletonWrapper : MonoBehaviour {

    SkeletonWrapper skeletonWrapper;
    KinectPointController kinectPointController;

	// Use this for initialization
	void Start () {
        skeletonWrapper = GameObject.Find("KinectPrefab(Clone)").GetComponent<SkeletonWrapper>();
        kinectPointController = gameObject.GetComponent<KinectPointController>();

        kinectPointController.sw = skeletonWrapper;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
