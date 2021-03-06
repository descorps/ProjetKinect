﻿using UnityEngine;
using System.Collections;


/** \file Bullet.cs
 *  \brief Gestion des Bullets du jeu
 */

public class Bullet : MonoBehaviour {

   
    /****************/
    /*  Paramètres  */
    /****************/

    [SerializeField]
    private static float speed = 70f;      /** Vitesse de déplacement des Bullets de l'arrière plan vers le premier plan */
    [SerializeField]
    private float z_hit = 10f;              /** Coordonnée z à partir de laquelle les Bullets sont touchables */
    [SerializeField]
    private float z_lost = 0f;              /** Coordonnée z à partir de laquelle les Bullets sont considérés comme manqués */
    [SerializeField]
    private float deathDuration = 0.5f;     /** Temps au bout duquel une Bullet touchée ou manquée disparait */
    /**********************************/
    /*  Variables de gestion interne  */
    /**********************************/

    private bool initialized = false;   // Indique si le Bullet a été initialisé
    KinectManager.Direction direction;  // Direction, mouvement associé au Bullet

    bool markedForRelease = false;
    float releaseTime;

    /*******************************/
    /*  Fonction d'initialisation  */
    /*******************************/

    /** \brief Fonction d'initialisation de la Bullet
     *  \param d : Mouvement à associer au Bullet
     */
    public void init(KinectManager.Direction d) {
        if (KinectManager.Instance == null) {                                           // Vérification de l'existence du KinectManager
            Debug.LogError("MovementManager wasn't initialized");                       // Si absent, erreur
            return;
        }
        else {                                                                          // Si présent
            switch (d) {                                                                // Suivant le mouvement demandé
                case KinectManager.Direction.Up:
                    KinectManager.Instance.onPlayerMovementUpEvent += onDirection;      // On s'inscrit à l'évènement correspondant
                    GetComponent<Renderer>().material.color = new Color(1, 0.4f, 0.4f);    // Rouge clair
                    break;
                case KinectManager.Direction.Left:
                    KinectManager.Instance.onPlayerMovementLeftEvent += onDirection;
                    GetComponent<Renderer>().material.color = new Color(1, 0.4f, 0.4f);
                    break;
                case KinectManager.Direction.Right:
                    KinectManager.Instance.onPlayerMovementRightEvent += onDirection;
                    GetComponent<Renderer>().material.color = new Color(1, 0.4f, 0.4f);
                    break;
                case KinectManager.Direction.BonusUp:
                    KinectManager.Instance.onPlayerMovementBonusUpEvent += onDirection;
                    GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1); // bleu clair
                    break;
                case KinectManager.Direction.BonusDown:
                    KinectManager.Instance.onPlayerMovementBonusDownEvent += onDirection;
                    GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 1); ;
                    break;
            }
        }
        direction = d;                                                                  // On mémorise le mouvement
        initialized = true;                                                             // On indique que l'initialisation a eu lieu
    }

    void OnDestroy()
    {
        switch (direction)
        {                                                                // Suivant le mouvement demandé
            case KinectManager.Direction.Up:
                KinectManager.Instance.onPlayerMovementUpEvent -= onDirection;      // On s'inscrit à l'évènement correspondant
                break;
            case KinectManager.Direction.Left:
                KinectManager.Instance.onPlayerMovementLeftEvent -= onDirection;
                break;
            case KinectManager.Direction.Right:
                KinectManager.Instance.onPlayerMovementRightEvent -= onDirection;
                break;
            case KinectManager.Direction.BonusUp:
                KinectManager.Instance.onPlayerMovementBonusUpEvent -= onDirection;
                break;
            case KinectManager.Direction.BonusDown:
                KinectManager.Instance.onPlayerMovementBonusDownEvent -= onDirection;
                break;
        }
    }

    /****************************/
    /*  Fonctions pour Unity3D  */
    /****************************/

    // ------------------------ Update ------------------------ //
    /** \Brief Fonction d'actualisation pour Unity3D
     */
    void Update() {
        if (initialized) {                                                  // Si la Bullet a été initialisée
            if (!markedForRelease) {                                        // Si le Bullet n'est pas en train de mourir
                if (transform.position.z < z_hit) {                         // Si la bullet est touchable
                    if (direction == KinectManager.Direction.Left || direction == KinectManager.Direction.Right || direction == KinectManager.Direction.Up)
                        GetComponent<Renderer>().material.color = Color.red;
                    else
                        GetComponent<Renderer>().material.color = Color.blue;
                }

                if (transform.position.z < z_lost) {                        // Si la Bullet a été manquée
                    GameManager.Instance.miss(direction);                   // On indique au GameManager que la Bullet a été manquée
                    markForRelease(false);
                }
                else {
                    transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));           // Sinon on déplace la Bullet
                }
            }
            else {                                                          // Si le Bullet est en train de mourir
                if (Time.time > releaseTime) {                              // Si la durée de disparition est atteinte
                    markedForRelease = false;
                    initialized = false;                                    // Reset l'initialisation
                    BulletFactory.ReleaseBullet(this);                      // On libère la Bullet pour usage futur
                }
                else
                    transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));           // Sinon on déplace la Bullet
            }
        }
    }

    void markForRelease(bool hit) {
        if (hit)
            GetComponent<Renderer>().material.color = Color.yellow;
        else
            GetComponent<Renderer>().material.color = Color.black;
        markedForRelease = true;
        releaseTime = Time.time + deathDuration;
        switch (direction)
        {                                                                // Suivant le mouvement demandé
            case KinectManager.Direction.Up:
                KinectManager.Instance.onPlayerMovementUpEvent -= onDirection;      // On s'inscrit à l'évènement correspondant
                break;
            case KinectManager.Direction.Left:
                KinectManager.Instance.onPlayerMovementLeftEvent -= onDirection;
                break;
            case KinectManager.Direction.Right:
                KinectManager.Instance.onPlayerMovementRightEvent -= onDirection;
                break;
            case KinectManager.Direction.BonusUp:
                KinectManager.Instance.onPlayerMovementBonusUpEvent -= onDirection;
                break;
            case KinectManager.Direction.BonusDown:
                KinectManager.Instance.onPlayerMovementBonusDownEvent -= onDirection;
                break;
        }
    }

    /*********************/
    /*  Autres méthodes  */
    /*********************/
    
    /** \brief Excesseur pour la position de la Bullet
     */
    public Vector3 Position {   /** Position du Bullet */
        get { return transform.position; }
        set { transform.position = value; }
    }

    /** \brief Fonction permettant de modifier la vitesse de la Bullet
     *  \param ratio : flottant par lequel doit être multipliée la vitesse
     */
    public static void modifySpeed(float ratio) {
        speed *= ratio;
    }

    /** \brief Fonction appelée sur évenement lors que le mouvement correspondant au Bullet a été effectué par le joueur */
    void onDirection()
    {
        if (initialized && transform.position.z < z_hit)
        {  // Si le Bullet est touchable
            GameManager.Instance.hit(direction);            // On indique au GameManager qu'on a été touché
            markForRelease(true);                           // On indique que ce Bullet est désormais disponible
        }
     }
}