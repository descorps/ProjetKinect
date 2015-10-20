using UnityEngine;
using System.Collections;

public class CollisionTextLancer : MonoBehaviour {

    // Main droite
    private SphereCollider mainDroiteCollider;
    private Transform mainDroiteTransform;
    private GameObject mainDroite;

    // Raycast partant de la main droite
    private RaycastHit hit;

    // Bouton "Commencer"
    private GameObject boutonCommencer;
    private BoxCollider boutonCommencerCollider;
  

	// Use this for initialization
	void Start ()
    {
        mainDroite = GameObject.Find("23_Hand_Right");
        mainDroiteCollider = mainDroite.GetComponent<SphereCollider>();
        mainDroiteTransform = mainDroite.GetComponent<Transform>();
        
        boutonCommencerCollider = gameObject.GetComponent<BoxCollider>();
    }
	

	// Update is called once per frame
	void Update ()
    {
        if (mainDroiteCollider.Raycast(new Ray(mainDroiteTransform.position, Vector3.back), out hit, 100) || mainDroiteCollider.Raycast(new Ray(mainDroiteTransform.position, Vector3.forward), out hit, 100))
        {
            Debug.Log("yooooo");
        }

        Debug.DrawRay(mainDroiteTransform.position, Vector3.back);
        Debug.DrawRay(mainDroiteTransform.position, Vector3.forward);



        // Détection à la barbare
        /*float posX = boutonCommencer.transform.position.x;
        float posY = boutonCommencer.transform.position.y;
        float scaleX = boutonCommencer.transform.localScale.x;
        float scaleY = boutonCommencer.transform.localScale.y;

        if (mainDroite.transform.position.x < posX + scaleX / 2 &&
            mainDroite.transform.position.x > posX - scaleX / 2 &&
            mainDroite.transform.position.y < posY + scaleY / 2 &&
            mainDroite.transform.position.x > posY - scaleY / 2)
        {
            Debug.Log("grgr");
        }
        
        Debug.Log("posX : " + posX + ". posY : " + posY + ". sizeX : " + sizeX + ". sizeY : " + sizeY + "maindroiteX" + mainDroite.transform.position.x + "maindroiteY" + mainDroite.transform.position.y);*/
    }
}
