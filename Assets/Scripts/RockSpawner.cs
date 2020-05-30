using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] GameObject rock;
    [SerializeField] float minRockForce = 100.0f;
    [SerializeField] float maxRockForce = 100.0f;
    [SerializeField] float minSpawn = 1.0f;
    [SerializeField] float maxSpawn = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRock());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRock()
    {
        Rigidbody temp = Instantiate(rock, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        float rockForce = Random.Range(minRockForce, maxRockForce);
        temp.AddForce(Vector3.forward * (rockForce), ForceMode.Impulse);
        Debug.Log("Rock Launched with " + rockForce);

        yield return new WaitForSeconds(Random.Range(minSpawn, maxSpawn));
        StartCoroutine(SpawnRock());
    }
}
