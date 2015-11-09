using UnityEngine;
using System.Collections;

public class TriggerDetection : MonoBehaviour {

    [SerializeField]
    private GameManager.Mode modeName;
    [SerializeField]
    private string modeKey;
    [SerializeField]
    private Color selectedColor = Color.yellow;
    [SerializeField]
    private Color unselectedColor = Color.white;

    private float chrono;
    private MeshRenderer rendBox;

    void Start()
    {
        rendBox = gameObject.GetComponent<MeshRenderer>();
        rendBox.material.color = unselectedColor;
    }

    void selectionStart()
    {
        rendBox.material.color = selectedColor;
        chrono += Time.deltaTime;
    }
    void selectionEnd()
    {
        rendBox.material.color = unselectedColor;
        chrono = 0;
    }

    void OnTrigger(Collider other)
    {
        selectionStart();
    }
    void OnTriggerExit(Collider other)
    {
        selectionEnd();
    }
    

	void Update ()
    {
        if(Input.GetKey(modeKey)) 
        {
            selectionStart();
        }

        if (Input.GetKeyUp(modeKey))
        {
            selectionEnd();
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
