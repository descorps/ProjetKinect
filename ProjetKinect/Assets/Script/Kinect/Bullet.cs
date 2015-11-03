using UnityEngine;
using System.Collections;


/********************/
/*                  */
/*      Bullet      */
/*                  */
/********************/

public class Bullet : MonoBehaviour {

    [SerializeField]
    private float speed = 1f;

    private bool initialized = false;
    
    Avatar player;


    void onDirection() {
        if (initialized && (player.transform.position - transform.position).magnitude < player.hitRange) {
            player.addPoints();
            // On sort une animation jolie
            Destroy(gameObject);
        }
    }

    public void Start() {
        if (GameManager.Instance == null) {
            Debug.LogError("GameManager was not created");
        }
        if (GameManager.Instance.Player == null) {
            Debug.LogError("Player was not initialized in GameManager");
        }
        player = GameManager.Instance.Player;
    }

    public void init(KinectManager.Direction d, Vector3 position) {
        if (initialized == false)
            return;
        if (KinectManager.Instance == null)
            Debug.LogError("MovementManager wasn't initialized");
        else {
            transform.position = position;
            switch (d) {
                case KinectManager.Direction.Up:
                    KinectManager.Instance.onPlayerMovementUpEvent += onDirection;
                    break;
                case KinectManager.Direction.Left:
                    KinectManager.Instance.onPlayerMovementLeftEvent += onDirection;
                    break;
                case KinectManager.Direction.Right:
                    KinectManager.Instance.onPlayerMovementRightEvent += onDirection;
                    break;
            }
        }
        initialized = true;
    }

    void Update() {
        if (initialized) {
            Vector3 v = player.transform.position - transform.position;

            if (player.hitBoxRadius > v.magnitude) {//Calcul de distance inférieur à la hitbox du joueur
                player.hit();
                // On sort une animation moche
                Destroy(gameObject);
            }
            else {
                transform.Translate(v.normalized);
            }
        }
    }
}