using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
    

    private void OnGUI() {
        switch (GameManager.Instance.currentMode) {
            case GameManager.Mode.Menu:


                break;
            case GameManager.Mode.LifeLimited:
                GUI.Label(new Rect(0f, 0f, Screen.width / 2f, 60f), GameManager.Instance.points.ToString());
                GUI.Label(new Rect(Screen.width / 2f, 0f, Screen.width / 2f, 60f), GameManager.Instance.points.ToString());
                break;
            case GameManager.Mode.TimeLimited:
                GUI.Label(new Rect(0f, 0f, Screen.width / 2f, 60f), GameManager.Instance.points.ToString());
                GUI.Label(new Rect(Screen.width / 2f, 0f, Screen.width / 2f, 60f), GameManager.Instance.points.ToString());
                break;
        }
    }
}
