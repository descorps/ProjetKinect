using UnityEngine;
using System.Collections;


/** \file Bullet.cs
 *  \brief Gestion des Bullets du jeu
 */

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float speed = 1f;   /** Vitesse de déplacement des Bullets de l'arrière plan vers le premier plan */

    [SerializeField]
    private float z_hit = 10f;  /** Coordonnée z à partir de laquelle les Bullets sont touchables */

    [SerializeField]
    private float z_lost = 0f;  /** Coordonnée z à partir de laquelle les Bullets sont considérés comme manqués */
    

    private bool initialized = false;   // Indique si le Bullet a été initialisé

    public Vector3 Position {   /** Position du Bullet */
        get {
            return transform.position;
        }
        set {
            transform.position = value;
        }
    }

    KinectManager.Direction direction;  // Direction, mouvement associé au Bullet
    

    /** \brief Fonction appelée sur évenement lors que le mouvement correspondant au Bullet a été effectué par le joueur */
    void onDirection() {
        if (initialized && transform.position.z < z_hit) {  // Si le Bullet est touchable
            GameManager.Instance.hit(direction);            // On indique au GameManager qu'on a été touché
            // On sort une animation jolie
            Debug.Log("Touché !");                          // En attendant une animation
            initialized = false;                            // Reset l'initialisation
            BulletFactory.ReleaseBullet(this);              // On indique que ce Bullet est désormais disponible
        }
    }
    
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
                    break;
                case KinectManager.Direction.Left:
                    KinectManager.Instance.onPlayerMovementLeftEvent += onDirection;
                    break;
                case KinectManager.Direction.Right:
                    KinectManager.Instance.onPlayerMovementRightEvent += onDirection;
                    break;
                case KinectManager.Direction.BonusUp:
                    KinectManager.Instance.onPlayerMovementBonusUpEvent += onDirection;
                    break;
                case KinectManager.Direction.BonusDown:
                    KinectManager.Instance.onPlayerMovementBonusDownEvent += onDirection;
                    break;
            }
        }
        direction = d;                                                                  // On mémorise le mouvement
        initialized = true;                                                             // On indique que l'initialisation a eu lieu
    }

    /** \Brief Fonction d'actualisation pour Unity3D
     */
    void Update() {
        if (initialized) {                                              // Si la Bullet a été initialisée
            if (transform.position.z < z_hit) {                         // Si la bullet est touchable
                GetComponent<Renderer>().material.color = Color.red;    // À changer
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.white;    // À changer
            }

            if (transform.position.z < z_lost) {                        // Si la Bullet a été manquée
                GameManager.Instance.miss(direction);                   // On indique au GameManager que la Bullet a été manquée
                // On sort une animation moche
                Debug.Log("Raté !");                                    // En attente d'une animation
                initialized = false;                                    // Reset l'initialisation
                BulletFactory.ReleaseBullet(this);                      // On libère la Bullet pour usage futur
            }
            else {
                transform.Translate(new Vector3(0,0,-speed));           // Sinon on déplace la Bullet
            }
        }
    }
}