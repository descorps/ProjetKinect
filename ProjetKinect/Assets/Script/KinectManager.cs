using UnityEngine;
using System.Collections;


/** \file KinectManager.cs
 * \brief Classe intermédiaire pour interfacer la gestion des mouvements du joueur avec le jeu
 */


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

    // Gestion des événements correspondant aux mouvements du joueurs significatifs
    public delegate void PlayerMovementUpEvent();
    public delegate void PlayerMovementLeftEvent();
    public delegate void PlayerMovementRightEvent();
    public delegate void PlayerMovementBonusUpEvent();
    public delegate void PlayerMovementBonusDownEvent();
    public event PlayerMovementUpEvent onPlayerMovementUpEvent;
    public event PlayerMovementLeftEvent onPlayerMovementLeftEvent;
    public event PlayerMovementRightEvent onPlayerMovementRightEvent;
    public event PlayerMovementBonusUpEvent onPlayerMovementBonusUpEvent;
    public event PlayerMovementBonusDownEvent onPlayerMovementBonusDownEvent;

    // Pour faciliter l'usage ultérieur

    public enum Direction
    {
        Up, Left, Right, BonusUp, BonusDown
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

    public void postureBonusUp()
    {
        sendPlayerMovementEvent(Direction.BonusUp);
    }

    public void postureBonusDown()
    {
        sendPlayerMovementEvent(Direction.BonusDown);
    }

    /** \brief Fonction factorisant l'envoi d'événements aux bullets concernés lors d'un mouvement significatif du joueur
     *  \param d : Mouvement effectué
     */
    private void sendPlayerMovementEvent(Direction d) {
        switch (d)                                              // Selon le mouvement effectué
        {
            case Direction.Up:                                  // Mouvement simple vers le haut
                if (onPlayerMovementUpEvent != null)            // S'il y a des inscrits à l'event
                    onPlayerMovementUpEvent();                  // On lance l'event
                break;
            case Direction.Left:                                // Mouvement simple vers la gauche
                if (onPlayerMovementLeftEvent != null)
                    onPlayerMovementLeftEvent();
                break;
            case Direction.Right:
                if (onPlayerMovementRightEvent != null)         // Mouvement simple vers la droite
                    onPlayerMovementRightEvent();
                break;
            case Direction.BonusUp:
                if (onPlayerMovementBonusUpEvent != null)       // Mouvement pour le bonus du haut
                    onPlayerMovementBonusUpEvent();
                break;
            case Direction.BonusDown:
                if (onPlayerMovementBonusDownEvent != null)     // Mouvement pour le bonus du bas
                    onPlayerMovementBonusDownEvent();
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
