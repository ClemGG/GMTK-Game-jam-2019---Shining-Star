using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour {
    

    public Image fadeImg;

    [Tooltip("Plus la valeur est haute, plus le fondu sera rapide, et inversement plus la valeur est basse.")]
    public float fadeSpeed = 1f;

    public Color fadeColor = Color.black;
    public AnimationCurve fadeCurve;


    public static SceneFader instance;




    private void Awake()
    {
        if (instance != null)
        {
            print("More than one SceneFader in scene !");
            return;
        }

        instance = this;
        
    }





    private void Start()
    {
        fadeImg.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
    }









    /// <summary>
    /// Permet de réaliser un fondu entre les scènes
    /// </summary>
    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeOut(sceneIndex));
    }



    /// <summary>
    /// Permet de réaliser un fondu avant de quitter le jeu.
    /// </summary>
    public void FadeToQuitScene()
    {
        StartCoroutine(FadeQuit());
    }


    /// <summary>
    /// Permet de jouer des sons de flashs pour le menu ppal.
    /// </summary>
    public void Flash()
    {
        
    }







    




    /// <summary>
    /// Diminue l'alpha du fondu pour faire apparaître la scène
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeIn()
    {
        Time.timeScale = 1f;
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.unscaledDeltaTime * fadeSpeed;
            float a = fadeCurve.Evaluate(t);
            fadeImg.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, a);
            yield return 0;
        }

        fadeImg.gameObject.SetActive(false);
    }








    /// <summary>
    /// Augmente l'alpha du fondu pour faire disparaître la scène
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOut(int sceneIndex)
    {
        fadeImg.gameObject.SetActive(true);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;
            float a = fadeCurve.Evaluate(t);
            fadeImg.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, a);
            yield return 0;
        }

        if (sceneIndex == 0)
            Time.timeScale = 1f;

        SceneManager.LoadScene(sceneIndex);
    }


    




    /// <summary>
    /// Augmente l'alpha du fondu pour faire disparaître la scène et quitter le jeu.
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeQuit()
    {
        fadeImg.gameObject.SetActive(true);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;
            float a = fadeCurve.Evaluate(t);
            fadeImg.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, a);
            yield return 0;
        }

        Application.Quit();
    }
}
