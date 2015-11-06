using UnityEngine;
using System.Collections;



/*************************/
/*                       */
/*      GameManager      */
/*                       */
/*************************/


public class GameManager : MonoBehaviour {

    /*********************/
    /*  Gestion générale */
    /*********************/

    private Canvas canvas;
    public Canvas canvasPrefab;

    public static GameManager Instance {
        get;
        private set;
    }

    public Avatar Player {
        get;
        private set;
    }

    public enum Mode
    {
        Menu, TimeLimited, LifeLimited
    }

    public Mode currentMode
    {
        get;
        private set;
    }

    /****************************/
    /*  Fonctions pour Unity3D  */
    /****************************/

    void Awake() {
        if (Instance != null) {
            Debug.LogError("There is multiple instance of singleton GameManager2");
            return;
        }
        Instance = this;
    }

    public void runLimitedTime()
    {
        Application.LoadLevel("LimitedTime");
        Debug.Log("run limited time");
    }
    public void runLimitedLife()
    {
        Application.LoadLevel("LimitedLife");
        Debug.Log("run limited life");
    }
    public void quit()
    {
        Debug.Log("quit");
    }

    public void displayTextUp()
    {
        Debug.Log("eventUp");
    }

    public void displayTextLeft()
    {
        Debug.Log("eventLeft");
    }

    public void displayTextRight()
    {
        Debug.Log("eventRight");
    }

    void Start() {
        if(Application.loadedLevel == 0)
        {
            canvas = Instantiate(canvasPrefab);
            canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
        else if (Application.loadedLevel == 1)  // LimitedLife
        {

        }
        else if (Application.loadedLevel == 2)  // LimitedTime
        {

        }
        if(KinectManager.Instance == null)
        {
            Debug.Log("no instance of KinectManager");
        }
        KinectManager.Instance.onPlayerMovementRightEvent += displayTextRight;
        KinectManager.Instance.onPlayerMovementLeftEvent += displayTextLeft;
        KinectManager.Instance.onPlayerMovementUpEvent += displayTextUp;
    }

    void Update() {
        // Spawn des trucs

    }

    public int points
    {
        get;
        private set;
    }

    public void addPoints()
    {
        points++;
    }

    public void hit()
    {
        points--;
    }

}
