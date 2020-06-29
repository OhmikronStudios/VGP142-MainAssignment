using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    bool spawnerReady = true;
    int spawnCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnerReady && spawnCount < 20)
        {
            StartCoroutine(SpawnBrute());
        }
    }

    IEnumerator SpawnBrute()
    {
        spawnerReady = false;
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        spawnCount++;
        yield return new WaitForSeconds(5.0f);
        spawnerReady = true;
        
    }

}
