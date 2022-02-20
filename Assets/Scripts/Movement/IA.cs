using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA : Ballerine
{

    [Space(10)]
    [Header("IA : ")]
    [Space(10)]


    [SerializeField] private float forceEjection = 5f;
    private Vector2 newDir;

    private Animator a;

    protected override void Start()
    {
        base.Start();

        a = t.GetChild(0).GetComponent<Animator>();

    }


    private void OnEnable()
    {
        if(a)
            a.Play("idle");



        isDead = false;

        if(col)
        col.enabled = true;

        if(sprite)
            sprite.rotation = Quaternion.identity;

    }


    protected override void OnCollisionEnter2D(Collision2D c)
    {
        base.OnCollisionEnter2D(c);
        
        if (c.gameObject.CompareTag("Player"))
        {
            //Si le joueur n'est pas en train d'attaquer, on rebondit, sinon on éjecte la ballerine

            Player p = c.gameObject.GetComponent<Player>();

            if (p.isAtking)
            {
                OnHit(p);
            }
            else
            {
                ChangeDirection(c.transform.position);
            }
        }
    }


    protected override void Update()
    {
        base.Update();

        if (isDead && !ScoreManager.instance.jeuTerminé)
        {
            if (Limits.instance.IsOffLimitsY(this) || Limits.instance.IsOffLimitsX(this))
            {
                BallerineSpawnerManager.instance.nbBallerinesEnScène--;
                BallerineSpawnerManager.instance.CheckIfMaxNbReached();

                gameObject.SetActive(false);
            }
        }
    }

    private void OnHit(Player p)
    {
        isDead = true;
        col.enabled = false;
        a.Play("death");

        /*On récupère la direction qui sépare les deux ballerines, puis on fait tourner l'IA pour qu'elle fasse face au joueur
         * Puis on lui ajoute une force pour l'éjecter hors de la scène
         */

        Vector2 newDir = (t.position - p.transform.position).normalized;
        sprite.rotation = Quaternion.LookRotation(sprite.forward, newDir);
        rb.AddRelativeForce(sprite.up * forceEjection, ForceMode2D.Impulse);

        //On pourra augmenter un peu la jauge d'attention quans on éjecte une ballerine
        ScoreManager.instance.IncreaseAttentionLevel();
        ScoreManager.instance.nbBallerinesTuées++;

        AudioManager.instance.PlayRandomSoundFromList(new string[] { "aie 1", "aie 2", "aie 3", "aie 4", "hey" });

        ScoreManager.instance.StartCombo();

    }


}
