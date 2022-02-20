using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallerineSpawnerManager : MonoBehaviour
{
    [SerializeField] private BallerineSpawner[] spawners; //Si on ne veut plus qu'ils soient dans le désordre, les ajouter à la main et commenter la 1ère ligne de Awake
    [SerializeField] private float délaiActivation;

    private int currentSpawnerIndex = 0;


    [HideInInspector] public bool nbMaxReached = false;
    [HideInInspector] public int nbBallerinesEnScène;
    public int nbMaxBallerinesEnScène = 10;


    public static BallerineSpawnerManager instance;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }

        instance = this;



        spawners = FindObjectsOfType<BallerineSpawner>();

        //On désactive tous les spawners sauf le premier
        for (int i = 1; i < spawners.Length; i++)
        {
            spawners[i].gameObject.SetActive(false);
        }

    }


    public void CheckIfMaxNbReached()
    {
        nbMaxReached = nbBallerinesEnScène == nbMaxBallerinesEnScène;
    }




    // Update is called once per frame
    IEnumerator Start()
    {

        spawners[0].Activate();


        WaitForSeconds w = new WaitForSeconds(délaiActivation);

        yield return w;



        while (true)
        {
            if (spawners[currentSpawnerIndex].hasFinished)
            {
                spawners[currentSpawnerIndex].gameObject.SetActive(false);


                currentSpawnerIndex++;
                if(currentSpawnerIndex == spawners.Length)
                {
                    currentSpawnerIndex = 0;
                }

                yield return w;

                spawners[currentSpawnerIndex].gameObject.SetActive(true);
                spawners[currentSpawnerIndex].Activate();

            }
            yield return 0;
        }

    }
}
