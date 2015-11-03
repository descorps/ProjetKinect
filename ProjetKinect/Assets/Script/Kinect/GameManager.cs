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

    public static GameManager Instance {
        get;
        private set;
    }

    public Avatar Player {
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

    public static void runLimitedTime()
    {
        Debug.Log("run limited time");
    }
    public static void runLimitedLife()
    {
        Debug.Log("run limited life");
    }
    public static void quit()
    {
        Debug.Log("quit");
    }

    void Start() {

    }

    void Update() {
        // Spawn des trucs

    }



}
