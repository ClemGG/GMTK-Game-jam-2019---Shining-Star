using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Space(10)]
    [Header("Scripts & Components : ")]
    [Space(10)]

    [SerializeField] private Slider attentionMeter;
    [SerializeField] private Image timeImg;
    [SerializeField] private RectTransform handleRect;

    [Space(10)]

    [SerializeField] private GameObject winCanvas;
    [SerializeField] private TextMeshProUGUI victoryText, attentionText, ballerineText, attentionToReachText;

    [Space(10)]
    [Header("Inputs : ")]
    [Space(10)]

    [Range(0f, 1f)] [SerializeField] private float attentionToReach;
    [SerializeField] private float currentAttentionLevel;
    [SerializeField] private float gameDuration = 180f;
    private float timer;

    [Space(10)]

    [HideInInspector] public float shouldIncreaseValue = 1f;
    [Range(0f, 0.1f)][SerializeField] private float increaseSpeed = .02f;
    [Range(0f, -0.1f)][SerializeField] private float decreaseSpeed = -.01f;
    [Range(0f, 0.1f)][SerializeField] private float increaseValueEjection = .02f;
    [Range(0f, 1f)][SerializeField] private float délaiIncrease = .5f;
    private float increaseTimer;

    [Space(10)]

    [Range(0f, 1f)] [SerializeField] private float délaiCombo = .2f;
    private float comboTimer;
    private bool aDéjàUnCombo = false;



    [Space(10)]

    [HideInInspector] public int nbBallerinesTuées = 0;
    [HideInInspector] public bool jeuTerminé = false;




    #region Singleton
    public static ScoreManager instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }
    #endregion


    private void OnValidate()
    {
        attentionMeter.handleRect = handleRect;
        attentionMeter.value = attentionToReach;
        attentionMeter.handleRect = null;

        float attentionPercent = (Mathf.Round((attentionToReach) * 100f));
        attentionToReachText.text = $"{attentionPercent}%";
    }

    // Start is called before the first frame update
    void Start()
    {
        winCanvas.SetActive(false);

        OnValidate();
    }

    // Update is called once per frame
    void Update()
    {
        if (!jeuTerminé)
        {
            CalculateScore();
            Countdown();
        }

        UpdateTimerUI();
        UpdateScoreUI();

        if(timer > gameDuration && !jeuTerminé)
        {
            EndGame();
        }
    }






    private void CalculateScore()
    {
        if(increaseTimer < délaiIncrease)
        {
            increaseTimer += Time.deltaTime;
        }
        else
        {
            increaseTimer = 0f;
            currentAttentionLevel += Mathf.Sign(shouldIncreaseValue) == 1 ? increaseSpeed : decreaseSpeed;
            currentAttentionLevel = Mathf.Clamp(currentAttentionLevel, 0f, 1f);
        }
    }

    public void IncreaseAttentionLevel()
    {
        currentAttentionLevel += increaseValueEjection;
    }





    private void Countdown()
    {
        if(timer < gameDuration)
        {
            timer += Time.deltaTime;
        }
    }







    private void UpdateTimerUI()
    {
        timeImg.fillAmount = timer / gameDuration;
    }

    private void UpdateScoreUI()
    {
        attentionMeter.value = currentAttentionLevel;
    }


    public void StartCombo()
    {
        if (!aDéjàUnCombo)
        {
            aDéjàUnCombo = true;
            StartCoroutine(Combo());
        }
        else
        {
            if (comboTimer < délaiCombo)
            {
                StopCoroutine(Combo());
                comboTimer = 0f;
                aDéjàUnCombo = false;
                ApplauseCoroutine();
            }
        }

    }


    private IEnumerator Combo()
    {

        while(comboTimer < délaiCombo)
        {
            comboTimer += Time.deltaTime;
            yield return 0;
        }

        comboTimer = 0f;
        aDéjàUnCombo = false;

    }




    private void EndGame()
    {
        jeuTerminé = true;

        winCanvas.SetActive(true);

        float attentionPercent = (Mathf.Round((attentionMeter.value) * 100f));

        victoryText.text = currentAttentionLevel > attentionToReach ? "Victory!" : "Failure...";
        attentionText.text = currentAttentionLevel > attentionToReach ? $"You drew the attention of <color=#1144FF>{attentionPercent}</color>% of the audience !" : "You couldn't gain enough attention from your audience...";
        ballerineText.text = $"You stopped <color=#FF55FF>{nbBallerinesTuées}</color> ballerinas from stealing your show !";

        if (currentAttentionLevel > attentionToReach)
        {
            StartCoroutine(Applause());
        }
        else
        {
            AudioManager.instance.Play("déception");
        }
    }


    public void GoToNextLevel()
    {
        int nextLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1;

        if(Application.CanStreamedLevelBeLoaded(nextLevel))
            SceneFader.instance.FadeToScene(nextLevel);
        else
            SceneFader.instance.FadeToScene(0);
            
    }

    public void GoToMainMenu()
    {
        SceneFader.instance.FadeToScene(0);
    }






    public Coroutine applauseCo;



    public void ApplauseCoroutine()
    {
        if (applauseCo != null)
        {
            StopCoroutine(applauseCo);
        }

        applauseCo = StartCoroutine(Applause(3f));
    }


    public IEnumerator Applause(float duration)
    {
        AudioManager.instance.Play("applause");


        GameObject[] spectateurs = GameObject.FindGameObjectsWithTag("Spectateur");

        for (int i = 0; i < spectateurs.Length; i++)
        {
            spectateurs[i].GetComponent<Animator>().Play("clap");
        }
        
        yield return new WaitForSeconds(duration);

        for (int i = 0; i < spectateurs.Length; i++)
        {
            spectateurs[i].GetComponent<Animator>().Play("idle");
        }
    }


    private IEnumerator Applause()
    {
        AudioManager.instance.Play("aaahh");
        AudioManager.instance.Play("applause");

        GameObject[] spectateurs = GameObject.FindGameObjectsWithTag("Spectateur");

        for (int i = 0; i < spectateurs.Length; i++)
        {
            spectateurs[i].GetComponent<Animator>().Play("clap");
        }

        yield return 0;

    }


}
