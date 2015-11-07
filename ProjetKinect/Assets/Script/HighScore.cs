using UnityEngine;
using System.Collections;
using System.IO;

public class HighScore : MonoBehaviour {

    [SerializeField]
    string scoresFilePath = "scores.txt";

    int[] scores = new int[10];

	// Use this for initialization
	void Start () {
        int i = 0;

        for (i = 0; i < 10; i++)
            scores[i] = 0;

        if (File.Exists(scoresFilePath)) {
            string[] content = File.ReadAllLines(scoresFilePath);
            foreach (string l in content) {
                if (i < 10)
                    scores[i] = int.Parse(l);
                i++;
            }
        }
	}
	
    void post(int score) {
        if (score > scores[9]) {                // Si le score mérite d'être sauvegardé
            for (int i = 8; i >= 0; i--) {      // Pour chaque score existant
                if (score > scores[i])          // Si le nouveau score est meilleur alors on recul d'un cran pour lui faire de la place
                    scores[i + 1] = scores[i]; 
                else {                          // Sinon on place le nouveau score au cran d'avant et on arrête
                    scores[i + 1] = score;
                    return;
                }
            }
        }
    }

    int[] get() {
        return scores;
    }
    
    void OnDestroy() {
        if (File.Exists(scoresFilePath))
            File.Delete(scoresFilePath);
        string[] content = new string[10];
        for (int i = 0; i < 10; i++)
            content[i] = scores[i].ToString();
        File.WriteAllLines(scoresFilePath, content);
    }
}
