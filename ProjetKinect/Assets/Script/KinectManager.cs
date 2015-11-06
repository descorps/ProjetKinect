using UnityEngine;
using System.Collections;


/************************************************/
/*                                              */
/*      Gestionnaire de mouvements Kinect       */
/*                                              */
/************************************************/


public class KinectManager : MonoBehaviour {

    /*********************/
    /*  Gestion générale */
    /*********************/

    public static KinectManager Instance {
        get;
        private set;
    }


    /*******************************/
    /*  Gestion pour l'activité 2  */
    /*******************************/

    // Gestion des événements
    public delegate void PlayerMovementUpEvent();
    public event PlayerMovementUpEvent onPlayerMovementUpEvent;
    // Gestion des événements
    public delegate void PlayerMovementLeftEvent();
    public event PlayerMovementLeftEvent onPlayerMovementLeftEvent;
    // Gestion des événements
    public delegate void PlayerMovementRightEvent();
    public event PlayerMovementRightEvent onPlayerMovementRightEvent;

    // Pour faciliter l'usage ultérieur

    public enum Direction {
        Up, Left, Right
    };

    private bool leftHandUp = false;
    private bool rightHandUp = false;
    private float chrono;

    public void rightHandTowardUp()
    {
        if (leftHandUp)
            sendPlayerMovementEvent(Direction.Up);
        else
            rightHandUp = true;
    }

    public void leftHandTowardUp()
    {
        if (rightHandUp)
            sendPlayerMovementEvent(Direction.Up);
        else
            leftHandUp = true;
    }

    public void rightHandTowardRight()
    {
        sendPlayerMovementEvent(Direction.Right);
    }

    public void leftHandTowardLeft()
    {
        sendPlayerMovementEvent(Direction.Left);
    }


    private void sendPlayerMovementEvent(Direction d) {
        switch (d)
        {
            case Direction.Up:
                if (onPlayerMovementUpEvent != null)
                    onPlayerMovementUpEvent();
                break;
            case Direction.Left:
                if (onPlayerMovementLeftEvent != null)
                    onPlayerMovementLeftEvent();
                break;
            case Direction.Right:
                if (onPlayerMovementRightEvent != null)
                    onPlayerMovementRightEvent();
                break;
        }
    }


    /****************************/
    /*  Fonctions pour Unity3D  */
    /****************************/

    public void Awake() {
        if (Instance != null)
        {
            Debug.LogError("There is multiple instance of singleton MovementManager");
            return;
        }
        Instance = this;
    }

    public void Start() {
    }

    public void Update() {
        // Not my problem

        // Pour indiquer que vous frappez dans une direction : 
        // sendPlayerMovementEvent(Direction.Left)

        if ((leftHandUp || rightHandUp) && chrono < 1)
            chrono += Time.deltaTime;
        else
        {
            rightHandUp = false;
            leftHandUp = false;
            chrono = 0;
        }
    }
}
