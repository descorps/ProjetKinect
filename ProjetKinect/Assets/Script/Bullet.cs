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

    [SerializeField]
    private float z_hit = 10f;

    [SerializeField]
    private float z_lost = 0f;
    

    private bool initialized = false;
    

    void onDirection() {
        if (initialized && transform.position.z < z_hit) {
            //GameManager.Instance.addPoints();
            // On sort une animation jolie
            Destroy(gameObject);
        }
    }

    public void Start() {
        if (GameManager.Instance == null) {
            Debug.LogError("GameManager was not created");
        }
    }

    public void init(KinectManager.Direction d, Vector3 position) {
        if (initialized == false)
            return;
        if (KinectManager.Instance == null) {
            Debug.LogError("MovementManager wasn't initialized");
            return;
        }
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
            if (transform.position.z < z_hit) {
                GetComponent<Renderer>().material.color = Color.red;
            }
            if (transform.position.z < z_lost) {//Calcul de distance inférieur à la hitbox du joueur
                GameManager.Instance.hit();
                // On sort une animation moche
                Destroy(gameObject);
            }
            else {
                transform.Translate(new Vector3(0,0,-speed));
            }
        }
    }
}