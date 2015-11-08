using UnityEngine;
using UnityEngine.UI;
using System;
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
            Debug.LogError("There is multiple instance of singleton GameManager");
            return;
        }
        Instance = this;
    }


    /******************/
    /*  Enumérations  */
    /******************/

    public enum Mode {                          /** Modes de fonctionnement de l'application */
        Init, Menu, TimeLimited, LifeLimited, Score, HighScore, Quit
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
    [SerializeField]
    private float coefDifficulty = 0.2f;


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
    private float nextBulletTime = 0;           // Instant où l'on pourra faire spawn une nouvelle Bullet


    /**************************************/
    /*  Accesseur de l'état de la partie  */
    /**************************************/

    /** \brief Fonction donnant la durée de la partie (tenant compte des bonus passés) 
     *  \return Durée de la partie en secondes
     */
    public float getLimitedTimeDuration() {
        return timeEndGame - timeBeginGame;
    }
    /** \brief Fonction indiquant le temps restant dans la partie
     *  \return Temps restant en secondes
     */
    public float getTimer() {                         
        return timeEndGame - Time.time;
    }
    /** \brief Fonction indiquand la durée écoulée depuis le début de la partie
     *  \return Temps écoulé en secondes
     */
    public float getCurrentTime() {
        return Time.time - timeBeginGame;
    }
    /** \brief Niveau actuel de difficulté
     *  \return entier représentant le niveau actuel de difficulté
     */
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
                int side = (int) UnityEngine.Random.Range(0, 3f + 3f * bonusRatio);
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
                        float typeBonus = UnityEngine.Random.Range(0.0f, 1.0f);
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
                nextBulletTime = t + UnityEngine.Random.Range(minDeltaTimeBetweenTwoBullet, maxDeltaTimeBetweenTwoBullet);
            }
        }
        if (currentMode == Mode.LifeLimited)
        {
            // Condition de fin
            if (life <= 0)
            {
                score = (int)getCurrentTime();
                displayScoreLimitedLife();
            }

            // Condition de passage de niveau
            if (Time.time > timeEndGame)
            {
                nextLevel();
                timeEndGame += limitedTimeDuration; // Pour le prochain passage de niveau
            }

        }
        else if (currentMode == Mode.TimeLimited)
        {
            // Condition de fin
            if (Time.time > timeEndGame)
                displayScoreLimitedTime();
        }
    }


    /********************************/
    /*  Méthodes de gestion du jeu  */
    /********************************/

    /** \brief Fonction indiquant qu'un Bullet a été manquée 
     */
    public void miss(KinectManager.Direction d) {
        if (d == KinectManager.Direction.Left || d == KinectManager.Direction.Right || d == KinectManager.Direction.Up) {
            if (currentMode == Mode.LifeLimited)
                life--;
            else if (currentMode == Mode.TimeLimited)
                score--;
        }
    }

    /** \brief Fonction indiquant qu'un Bullet a été atteint
     */
    public void hit(KinectManager.Direction d) {
        if (d == KinectManager.Direction.Left || d == KinectManager.Direction.Right || d == KinectManager.Direction.Up) {
            score += difficultylvl;
        }
        else if (d == KinectManager.Direction.BonusDown) {
            if (currentMode == Mode.LifeLimited)
                life += lifeBonus;
            if (currentMode == Mode.TimeLimited)
                timeEndGame += timeBonus;
        }
        else if (d == KinectManager.Direction.BonusUp) {
            score += scoreBonus * difficultylvl;
        }
    }

    /****************************************************/
    /*  Fonctions de transition entre les états du jeu  */
    /****************************************************/

    /** \brief Fonction lançant le jeu en mode temps limité
     */
    public void runLimitedTime() {
        score = 0;
        timeBeginGame = Time.time;
        timeEndGame = Time.time + limitedTimeDuration;
        nextBulletTime = Time.time;
        currentMode = Mode.TimeLimited;
        Application.LoadLevel("LimitedTime");
        Debug.Log("run limited time");
    }
    /** \brief Fonction lançant le jeu en mode vie limitée
     */
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

    /** \brief Fonction augmentant la difficulté du jeu en mode vie limitée */
    public void nextLevel() {
        difficultylvl++;
        // Effets peut être à modifier
        minDeltaTimeBetweenTwoBullet *= 1f - coefDifficulty;
        maxDeltaTimeBetweenTwoBullet *= 1f - coefDifficulty;
        Bullet.modifySpeed(1f + coefDifficulty);
    }


    /** \brief Fonction déclenchant un retour au menu */
    public void GoBackToMenu() {
        Application.LoadLevel("MainMenu");
        currentMode = Mode.Menu;
    }

    public void displayScoreLimitedLife()
    {
        Application.LoadLevel("Score");
        GameObject.Find("TextScore").GetComponent<Text>().text = "LIMITED LIFE" + Environment.NewLine + "Score : " + score + " points";
        currentMode = Mode.Score;
    }

    public void displayScoreLimitedTime()
    {
        Application.LoadLevel("Score");
        GameObject.Find("TextScore").GetComponent<Text>().text = "LIMITED TIME" + Environment.NewLine + "Score : " + score + " points";
        currentMode = Mode.Score;
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
