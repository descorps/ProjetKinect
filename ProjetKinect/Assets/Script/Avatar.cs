using UnityEngine;
using System.Collections;



/********************/
/*                  */
/*      Avatar      */
/*                  */
/********************/

public class Avatar : MonoBehaviour {

    [SerializeField]
    public float hitBoxRadius {
        get;
        private set;
    }

    [SerializeField]
    public float hitRange {
        get;
        private set;
    }


    public int points {
        get;
        private set;
    }

    private int score = 0;

    public void addPoints() {
        score++;
    }

    public void hit() {
        score--;
    }

    public Avatar Instance;

    void Awake() {
        if (Instance != null) {
            Debug.LogError("There is multiple instance of singleton Avatar");
            return;
        }
        Instance = this;
    }
}

