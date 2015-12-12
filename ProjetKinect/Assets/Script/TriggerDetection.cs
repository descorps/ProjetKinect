using UnityEngine;
using System.Collections;

public class TriggerDetection : MonoBehaviour {

    [SerializeField]
    private GameManager.Mode modeName; // nom du mode correspondant au bouton
    [SerializeField]
    private string modeKey; // touche correspondant au bouton
    [SerializeField]
    private Color selectedColor = Color.yellow; // couleur lorsque le bouton est sélectionné
    [SerializeField]
    private Color unselectedColor = Color.white; // couleur lorsque le bouton n'est pas sélectionné

    private float chrono;
    private MeshRenderer rendBox;

    void Start()
    {
        rendBox = gameObject.GetComponent<MeshRenderer>();
        rendBox.material.color = unselectedColor;
    }

    /** \brief Ce qu'il se passe quand on est en cours de sélection du bouton
     */
    void selection()
    {
        rendBox.material.color = selectedColor;
        chrono += Time.deltaTime;
    }
    /** \brief Ce qu'il se passe lorsqu'on sort de la zone de sélection du bouton
     */
    void selectionEnd()
    {
        rendBox.material.color = unselectedColor;
        chrono = 0;
    }

    /** \brief Lorsqu'une main est en collision avec le collider du bouton
     */
    void OnTriggerStay(Collider other)
    {
        selection();
    }
    /** \brief Lorsqu'une main sort de la collision avec le collider du bouton
     */
    void OnTriggerExit(Collider other)
    {
        selectionEnd();
    }
    

	void Update ()
    {
        if(Input.GetKey(modeKey)) // Lorsque la touche correspondante au bouton est pressée
        {
            selection();
        }

        if (Input.GetKeyUp(modeKey)) // Lorsque la touche correspondante au bouton est lachée
        {
            selectionEnd();
        }

        if (chrono > 2) // Lorsque le bouton est sélectionné pendant 2 secondes, on appelle la fonction correspondante au bouton
        {
            if (modeName == GameManager.Mode.TimeLimited)
                GameManager.Instance.runLimitedTime();
            else if (modeName == GameManager.Mode.LifeLimited)
                GameManager.Instance.runLimitedLife();
            else if (modeName == GameManager.Mode.Quit)
                GameManager.Instance.quit();
            else if (modeName == GameManager.Mode.Menu)
                GameManager.Instance.GoBackToMenu();
            else if (modeName == GameManager.Mode.HighScore)
                GameManager.Instance.displayHighScore();
            chrono = 0;
        }
    }
}
