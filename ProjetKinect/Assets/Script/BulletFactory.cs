using UnityEngine;
using System.Collections.Generic;

/**
 *  \file BulletFactory.cs
 *  \brief Factory pour l'instanciation et la gestion des bullets (ennemis et bonus)
 */

public class BulletFactory : MonoBehaviour {

    private int bulletCount = 0; /** Compte les bullets existantes*/

Queue<Bullet> availableBullets = new Queue<Bullet>();   /** Recense les bullets qui ne sont actuellement pas utilisées */

    [SerializeField]
    private GameObject bulletPrefab; /** Préfab des bullets */

    [SerializeField]
    private int numberOfBulletsToPreinstantiate; /** Paramètre indiquant le nombre de bullets à initialiser au démarrage */

    private static BulletFactory Instance { /** Pour design pattern Singleton */
        get;
        set;
    }

    /** \brief Méthode statique permettant de récupérer une bullet active
     *  \return Une bullet active, prête à être utilisée
     */
    public static Bullet getBullet() {
        Bullet bullet = null;                                           // Référence pour réception du bullet
        if (BulletFactory.Instance.availableBullets.Count > 0) {        // Si des Bullets sont disponibles
            bullet = BulletFactory.Instance.availableBullets.Dequeue(); // On en prend une
        }
        if (bullet == null) {                                           // Si aucune Bullet n'a été récupérée
            bullet = InstantiateBullet();                               // On en instancie une nouvelle
        }

        bullet.gameObject.SetActive(true);                              // On active la Bullet
        return bullet;                                                  // On retourne la Bullet activée
    }

    /** \brief Méthode statique instanciant une nouvelle Bullet
     */
    private static Bullet InstantiateBullet() {
        GameObject gameObject = (GameObject) GameObject.Instantiate(Instance.bulletPrefab); // Instantiation
        gameObject.SetActive(false);                                                        // Désactivation
        gameObject.transform.parent = BulletFactory.Instance.gameObject.transform;          // Association dans le graphe du monde
        Bullet bullet = gameObject.GetComponent<Bullet>();                                  // Récupération du composant Bullet
        BulletFactory.Instance.bulletCount++;                                               // Comptage de la nouvelle Bullet
        return bullet;
    }

    /** \brief Méthode statique libèrant une bullet pour un usage futur
     *  \param Bullet à libérer
     */
    public static void ReleaseBullet(Bullet bullet) {
        bullet.gameObject.SetActive(false);                         // Désactivation de la Bullet
        BulletFactory.Instance.availableBullets.Enqueue(bullet);    // Ajout de la Bullet à la file des Bullets disponibles
    }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is multiple instance of singleton BulletsFactory");
            return;
        }
        Instance = this;
    }

    /** \brief Méthode d'initialisation par Unity3D
     */
    private void Start() {
        if (this.bulletPrefab == null) {                            // Vérification de l'existance du prefab
            Debug.LogError("A bullet prefab is not set.");          // Si absent, erreur
            return;
        }

        for (int i = 0; i < numberOfBulletsToPreinstantiate; i++) { // Autant de fois que l'on doit préinstancier des Bullets
            Bullet bullet = InstantiateBullet();                    // On instancie une Bullet

            if (bullet == null) {                                   // En cas d'echec, erreur.
                Debug.LogError(string.Format("Failed to instantiate {0} bullets.", numberOfBulletsToPreinstantiate));
                break;
            }

            BulletFactory.Instance.availableBullets.Enqueue(bullet);// On ajoute la nouvelle Bullet à la liste des Bullets disponibles.
        }
    }
    
}
