﻿using UnityEngine;
using System.Collections;

public class TriggerDetection : MonoBehaviour {

    [SerializeField]
    private string modeName;
    [SerializeField]
    private string modeKey;

    private bool inBox;
    private float chrono;

    private MeshRenderer rendBox;
    
  
    void OnTriggerEnter(Collider other)
    {
        rendBox.material.color = Color.blue;
        inBox = true;
    }

    void OnTriggerExit(Collider other)
    {
        rendBox.material.color = Color.white;
        inBox = false;
        chrono = 0;
    }
    
    void Start ()
    {
        rendBox = gameObject.GetComponent<MeshRenderer>();
        rendBox.material.color = Color.white;
    }
	

	void Update ()
    {
        if(Input.GetKey(modeKey)) 
        {
            rendBox.material.color = Color.blue;
            chrono += Time.deltaTime;
        }

        if (Input.GetKeyUp(modeKey))
        {
            rendBox.material.color = Color.white;
            chrono = 0;
        }

        if (inBox)
        {
            chrono += Time.deltaTime;
        }

        if (chrono > 2)
        {
            if (string.Compare(modeName, "LimitedTime") == 0)
                GameManager.Instance.runLimitedTime();
            if (string.Compare(modeName, "LimitedLife") == 0)
                GameManager.Instance.runLimitedLife();
            if (string.Compare(modeName, "Quit") == 0)
                GameManager.Instance.quit();
            chrono = 0;
        }
    }
}
