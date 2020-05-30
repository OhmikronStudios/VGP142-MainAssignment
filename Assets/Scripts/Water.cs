using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().TakeDamage(100);
        }
        else if (other.gameObject.tag == "Rock" || other.gameObject.tag == "Projectile")
        {
            Destroy(other.gameObject, 1.0f);
        }
    }


}
