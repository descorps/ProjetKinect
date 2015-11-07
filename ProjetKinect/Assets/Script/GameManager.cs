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
    private Camera cam;

    private Canvas canvas;
    public Canvas canvasPrefab;

    private KinectSensor kinect;
    public KinectSensor kinectPrefab;

    private KinectPointController kinectPointController;
    public KinectPointController kinectPointControllerPrefab;

    public static GameManager Instance {
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

    [SerializeField]
    Vector3 leftStartPosition;

    [SerializeField]
    Vector3 rightStartPosition;

    [SerializeField]
    Vector3 upStartPosition;


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
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();

        kinect = Instantiate(kinectPrefab);
        kinectPointController = Instantiate(kinectPointControllerPrefab);

        if (Application.loadedLevel == 0)
        {
            canvas = Instantiate(canvasPrefab);
            canvas.worldCamera = cam;
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

    [SerializeField]
    private float minDeltaTimeBetweenTwoBullet;
    [SerializeField]
    private float maxDeltaTimeBetweenTwoBullet;
    [SerializeField]
    private float bonusRatio;

    private float next = 0;

    void Update()
    {
        if (Application.loadedLevel != 0)
        {
            if (Input.GetKeyDown("a"))
                Application.LoadLevel(0);

            cam.orthographic = false;
            // Spawn des trucs
            float t = Time.time;
            if (t > next)
            {
                int side = (int)Random.Range(0, 3f + 3 * bonusRatio);
                Bullet bullet = BulletFactory.getBullet();
                switch (side)
                {
                    case 0:
                        bullet.Position = leftStartPosition;
                        bullet.init(KinectManager.Direction.Left);
                        break;
                    case 1:
                        bullet.Position = rightStartPosition;
                        bullet.init(KinectManager.Direction.Right);
                        break;
                    case 2:
                        bullet.Position = upStartPosition;
                        bullet.init(KinectManager.Direction.Up);
                        break;
                    default:
                        float typeBonus = Random.Range(0, 1);
                        if (typeBonus < 0.5)
                        {
                            // Bonus en haut
                        }
                        else
                        {
                            // Bonus en bas
                        }
                        break;
                }
                next = t + Random.Range(minDeltaTimeBetweenTwoBullet, maxDeltaTimeBetweenTwoBullet);
            }
        }
    }

    public int points
    {
        get;
        private set;
    }

    public void hit(KinectManager.Direction d)
    {
        points++;
    }

    public void miss(KinectManager.Direction d)
    {
        points--;
    }

}
