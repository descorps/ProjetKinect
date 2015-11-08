using UnityEngine;
using System.Collections;
using System.IO;

public class HighScore : MonoBehaviour {


    /***********************/
    /*  Gestion singleton  */
    /***********************/

    public static HighScore Instance {    /** Référence vers l'instance du singleton */
        get;
        private set;
    }
    /** \brief Fonction Awake pour Unity3D, gère le singleton */
    void Awake() {
        if (Instance != null) {         // Si l'instance existe déjà, erreur
            Debug.LogError("There is multiple instance of singleton HighScore");
            return;
        }
        Instance = this;
    }


    /****************/
    /*  Paramètres  */
    /****************/

    [SerializeField]
    string scoresFilePath = "scores.txt";   /** Chemin du fichier de mémorisation des scores */


    /**********************************/
    /*  Variables de gestion interne  */
    /**********************************/

    int[] scoresLimitedLife = new int[10];  /** Tableau des highscores pour le mode vie limitée */
    int[] scoresLimitedTime = new int[10];  /** Tableau des highscores pour le mode temps limité /



    /****************************/
    /*  Fonctions pour Unity3D  */
    /****************************/

    // Initialise et charge les scores
    void Start() {
        int i = 0;

        for (i = 0; i < 10; i++)
            scoresLimitedLife[i] = 0;
        for (i = 0; i < 10; i++)
            scoresLimitedTime[i] = 0;

        if (File.Exists(scoresFilePath)) {
            string[] content = File.ReadAllLines(scoresFilePath);
            foreach (string l in content) {
                if (i < 10)
                    scoresLimitedLife[i] = int.Parse(l);
                else if (i < 20)
                    scoresLimitedTime[i - 10] = int.Parse(l);
                i++;
            }
        }
    }
    
    // Sauvegarde les scores
    void OnDestroy() {
        if (File.Exists(scoresFilePath))
            File.Delete(scoresFilePath);
        string[] content = new string[20];
        for (int i = 0; i < 10; i++)
            content[i] = scoresLimitedLife[i].ToString();
        for (int i = 0; i < 10; i++)
            content[i + 10] = scoresLimitedTime[i].ToString();
        File.WriteAllLines(scoresFilePath, content);
    }


    /*********************/
    /*  Autres méthodes  */
    /*********************/

    /** \brief Soumet un nouveau score en mode vie limitée
     *  \param score : valeur du nouveau score à soumettre
     *  Cette fonction mémorise le score si celui-ci figure au top 10 des meilleurs scores jamais effectués
     */
    public void postLimitedLife(int score) {
        if (score > scoresLimitedLife[9]) {                 // Si le score mérite d'être sauvegardé
            for (int i = 8; i >= 0; i--) {                  // Pour chaque score existant
                if (score > scoresLimitedLife[i])           // Si le nouveau score est meilleur alors on recul d'un cran pour lui faire de la place
                    scoresLimitedLife[i + 1] = scoresLimitedLife[i];
                else {                                      // Sinon on place le nouveau score au cran d'avant et on arrête
                    scoresLimitedLife[i + 1] = score;
                    return;
                }
            }
            scoresLimitedLife[0] = score;
        }
    }

    /** \brief Soumet un nouveau score en mode temps limité
     *  \param score : valeur du nouveau score à soumettre
     *  Cette fonction mémorise le score si celui-ci figure au top 10 des meilleurs scores jamais effectués
     */
    public void postLimitedTime(int score) {
        if (score > scoresLimitedTime[9]) {                // Si le score mérite d'être sauvegardé
            for (int i = 8; i >= 0; i--) {      // Pour chaque score existant
                if (score > scoresLimitedTime[i])          // Si le nouveau score est meilleur alors on recul d'un cran pour lui faire de la place
                    scoresLimitedTime[i + 1] = scoresLimitedTime[i];
                else {                          // Sinon on place le nouveau score au cran d'avant et on arrête
                    scoresLimitedTime[i + 1] = score;
                    return;
                }
            }
            scoresLimitedTime[0] = score;
        }
    }

    /** \brief Fonction donnant accès au leaderboard pour le mode vie limitée 
     *  \return Tableau de dix int représentant les dix meilleurs scores en ordre décroissant
     */
    public int[] getLimitedLife() {
        return scoresLimitedLife;
    }

    /** \brief Fonction donnant accès au leaderboard pour le mode temps limité
     *  \return Tableau de dix int représentant les dix meilleurs scores en ordre décroissant
     */
    public int[] getLimitedTime() {
        return scoresLimitedTime;
    }
}
