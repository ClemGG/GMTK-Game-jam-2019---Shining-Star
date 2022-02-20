using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallerineSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ballerinePrefab;
    [SerializeField] private int nbBallerinesToSpawn;
    [SerializeField] private float intervalleDeSpawn = .5f;
    public bool hasFinished = true;


    private Transform t;

    // Start is called before the first frame update
    void OnEnable()
    {
        t = transform;
    }

    public void Activate()
    {
        hasFinished = false;
        StartCoroutine(SpawnBallerines());
    }

    private IEnumerator SpawnBallerines()
    {
        int nbBallerinesSpawnées = 0;

        WaitForSeconds w = new WaitForSeconds(intervalleDeSpawn);

        while (!hasFinished)
        {
            if (!BallerineSpawnerManager.instance.nbMaxReached)
            {
                IA ia = ObjectPooler.instance.SpawnFromPool(ballerinePrefab.name, t.position, Quaternion.identity).GetComponent<IA>();
                ia.SetMoveDir = t.right;

                nbBallerinesSpawnées++;

                BallerineSpawnerManager.instance.nbBallerinesEnScène++;
                BallerineSpawnerManager.instance.CheckIfMaxNbReached();


                if (nbBallerinesSpawnées == nbBallerinesToSpawn)
                    hasFinished = true;
            }
            yield return w;
        }
    }
}
