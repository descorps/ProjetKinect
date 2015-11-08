using UnityEngine;
using System.Collections;

public class TriggerDetection : MonoBehaviour {

    [SerializeField]
    private GameManager.Mode modeName;
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
            if (modeName == GameManager.Mode.TimeLimited)
                GameManager.Instance.runLimitedTime();
            else if (modeName == GameManager.Mode.LifeLimited)
                GameManager.Instance.runLimitedLife();
            else if (modeName == GameManager.Mode.Quit)
                GameManager.Instance.quit();
            else if (modeName == GameManager.Mode.Menu)
                GameManager.Instance.GoBackToMenu();
            else if (modeName == GameManager.Mode.HighScore)
                GameManager.Instance.displayHighScore();
            chrono = 0;
        }
    }
}
