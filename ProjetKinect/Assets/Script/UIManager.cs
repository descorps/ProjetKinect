using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    [SerializeField]
    float deltaTimeDisplayLvl = 1;

    float time = 0;
    int level = 0;
    private void OnGUI() {
        switch (GameManager.Instance.currentMode) {
            case GameManager.Mode.Menu:

                break;
            case GameManager.Mode.LifeLimited:
                GUI.Label(new Rect(0f, 0f, Screen.width / 2f, 60f), GameManager.Instance.life.ToString());
                GUI.Label(new Rect(Screen.width / 2f, 0f, Screen.width / 2f, 60f), GameManager.Instance.score.ToString());
                break;
            case GameManager.Mode.TimeLimited:
                GUI.Label(new Rect(0f, 0f, Screen.width / 2f, 60f), GameManager.Instance.score.ToString());
                GUI.Label(new Rect(Screen.width / 2f, 0f, Screen.width / 2f, 60f), GameManager.Instance.score.ToString());  //Temps à afficher au lieu du score
                break;
        }
        if (level != GameManager.Instance.difficultylvl) {  // En cas de changement de niveau
            level = GameManager.Instance.difficultylvl;     // Nouveau niveau
            time = Time.time + deltaTimeDisplayLvl;             // On déclenche l'affichage du numéro du niveau
        }
        if (Time.time < time)                                   // S'il faut afficher le numéro du niveau
            GUI.Label(new Rect(Screen.width / 4f, Screen.height / 4f, Screen.width / 2f, Screen.width / 2f), "Level " + level.ToString());  //Temps à afficher au lieu du score
    }
}
