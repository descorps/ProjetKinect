using UnityEngine;
using System.Collections;


/** \file GameManager.cs
 */

public class GameManager : MonoBehaviour {


    /***********************/
    /*  Gestion singleton  */
    /***********************/

    public static GameManager Instance {    /** Référence vers l'instance du singleton */
        get;
        private set;
    }
    /** \brief Fonction Awake pour Unity3D, gère le singleton */
    void Awake() {
        if (Instance != null) {         // Si l'instance existe déjà, erreur
            Debug.LogError("There is multiple instance of singleton GameManager2");
            return;
        }
        Instance = this;
    }


    /******************/
    /*  Enumérations  */
    /******************/

    public enum Mode {                          /** Modes de fonctionnement de l'application */
        Init, Menu, TimeLimited, LifeLimited, Quit
    }


    /****************/
    /*  Paramètres  */
    /****************/

    [SerializeField]
    Vector3 leftStartPosition;                  /** Position de départ des Bullet correspondant au mouvement vers la gauche */
    [SerializeField]
    Vector3 rightStartPosition;                 /** Position de départ des Bullet correspondant au mouvement vers la droite */
    [SerializeField]
    Vector3 upStartPosition;                    /** Position de départ des Bullet correspondant au mouvement vers le haut (y compris Bonus) */
    [SerializeField]
    Vector3 downStartPosition;                  /** Position de départ des Bullet correspondant au mouvement vers le bas (Bonus) */
    [SerializeField]
    private float minDeltaTimeBetweenTwoBullet; /** Durée min entre deux apparitions de Bullet */
    [SerializeField]
    private float maxDeltaTimeBetweenTwoBullet; /** Durée max entre deux apparitions de Bullet */
    [SerializeField]
    private float bonusRatio;                   /** Ratio de Bonus parmi les Bullets */
    [SerializeField]
    private float limitedTimeDuration = 60;     /** Temps de base d'une partie en temps limité */
    [SerializeField]
    private int scoreBonus;
    [SerializeField]
    private int lifeBonus;
    [SerializeField]
    private float timeBonus;
    [SerializeField]
    private int maxLife;


    /***********************************/
    /*  Variables de gestion générale  */
    /***********************************/

    public Mode currentMode {                   /** Mode de fonctionnement actuel */
        get;
        private set;
    }
    public int score {                          /** Score du joueur */
        get;
        private set;
    }
    public int life {                           /** Vie du joueur */
        get;
        private set;
    }


    /**********************************/
    /*  Variables de gestion interne  */
    /**********************************/

    private Camera cam;

    private Canvas canvas;
    public Canvas canvasPrefab;

    private KinectSensor kinect;
    public KinectSensor kinectPrefab;

    private KinectPointController kinectPointController;
    public KinectPointController kinectPointControllerPrefab;

    float timeBeginGame;                        // Pour la gestion de la partie en temps limité
    float timeEndGame;                          // Pour la gestion de la partie en temps limité
    private float nextBulletTime = 0;                     // Instant où l'on pourra faire spawn une nouvelle Bullet


    /**************************************/
    /*  Accesseur de l'état de la partie  */
    /**************************************/

    public float getLimitedTimeDuration {       /** Accesseurs pour la durée de fin de la partie */
        get { return timeEndGame - timeBeginGame; }
    }
    public float getTimer {
        get {  return timeEndGame - Time.time; }
    }
    public float getCurrentTime {               /** Accesseurs pour la durée actuelle de la partie */
        get { return Time.time - timeBeginGame; }
    }
    public int difficultylvl { get; private set; }


    /****************************/
    /*  Fonctions pour Unity3D  */
    /****************************/

    // ------------------------ Start ------------------------ //
    void Start()
    {
        GoBackToMenu();

        kinect = Instantiate(kinectPrefab);
        kinectPointController = Instantiate(kinectPointControllerPrefab);
        
        KinectManager.Instance.onPlayerMovementRightEvent += displayTextRight;
        KinectManager.Instance.onPlayerMovementLeftEvent += displayTextLeft;
        KinectManager.Instance.onPlayerMovementUpEvent += displayTextUp;

    }


    // ------------------------ Update ------------------------ //
    void Update() {
        if (currentMode == Mode.TimeLimited || currentMode == Mode.LifeLimited) {
            // Spawn des trucs
            float t = Time.time;
            if (t > nextBulletTime) {
                int side = (int) Random.Range(0, 3f + 3 * bonusRatio);
                Bullet bullet = BulletFactory.getBullet();
                switch (side) {
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
                        float typeBonus = Random.Range(0.0f, 1.0f);
                        Debug.Log(typeBonus);
                        if (typeBonus > 0.5) {
                            bullet.Position = upStartPosition;
                            bullet.init(KinectManager.Direction.BonusUp);
                        }
                        else {
                            bullet.Position = downStartPosition;
                            bullet.init(KinectManager.Direction.BonusDown);
                        }
                        break;
                }
                nextBulletTime = t + Random.Range(minDeltaTimeBetweenTwoBullet, maxDeltaTimeBetweenTwoBullet);
            }
        }
        if (currentMode == Mode.LifeLimited) {
            // Condition de fin
            if (life <= 0)
                GoBackToMenu();
        }
        else if (currentMode == Mode.TimeLimited) {
            // Condition de fin
            if (Time.time > timeEndGame)
                GoBackToMenu();

            // Condition de passage de niveau
            if (Time.time > timeEndGame)
                nextLevel();
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            GoBackToMenu();
        }
    }


    /********************************/
    /*  Méthodes de gestion du jeu  */
    /********************************/

    public void miss(KinectManager.Direction d) {
        if (d == KinectManager.Direction.Left || d == KinectManager.Direction.Right || d == KinectManager.Direction.Up) {
            if (currentMode == Mode.LifeLimited)
                life--;
            else if (currentMode == Mode.TimeLimited)
                score--;
        }
    }

    public void hit(KinectManager.Direction d) {
        if (d == KinectManager.Direction.Left || d == KinectManager.Direction.Right || d == KinectManager.Direction.Up) {
            score++;
        }
        else if (d == KinectManager.Direction.BonusDown) {
            if (currentMode == Mode.LifeLimited)
                life += lifeBonus;
            if (currentMode == Mode.TimeLimited)
                timeEndGame += timeBonus;
        }
        else if (d == KinectManager.Direction.BonusUp) {
            score += scoreBonus;
        }
    }

    public void runLimitedTime() {
        score = 0;
        timeBeginGame = Time.time;
        timeEndGame = Time.time + limitedTimeDuration;
        nextBulletTime = Time.time;
        currentMode = Mode.TimeLimited;
        Application.LoadLevel("LimitedTime");
        Debug.Log("run limited time");
    }
    public void runLimitedLife() {
        life = maxLife;
        difficultylvl = 1;
        score = 0;
        timeBeginGame = Time.time;
        timeEndGame = Time.time + limitedTimeDuration;
        nextBulletTime = Time.time;
        currentMode = Mode.LifeLimited;
        Application.LoadLevel("LimitedLife");
        Debug.Log("run limited life");
    }

    public void nextLevel() {
        difficultylvl++;
        // Effets peut être à modifier
        minDeltaTimeBetweenTwoBullet *= 0.9f;
        maxDeltaTimeBetweenTwoBullet *= 0.9f;
        Bullet.modifySpeed(1.1f);
    }

    public void GoBackToMenu() {
        Application.LoadLevel("MainMenu");
        currentMode = Mode.Menu;
    }

    /*********************/
    /*  Autres méthodes  */
    /*********************/


    public void quit()
    {
        Application.Quit();
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

}
