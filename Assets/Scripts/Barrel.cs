using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{

    [SerializeField] MeshRenderer rend;
    [SerializeField] Material BarrelBrown;
    [SerializeField] Material BarrelRed;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] float flashSpeed = 0.2f;

    

    [SerializeField] GameObject player;
    [SerializeField] bool triggered = false;
    [SerializeField] float triggerRadius = 5.0f;
    [SerializeField] float explosionTimer = 5.0f;
    [SerializeField] float blastForce = 5.0f;
    [SerializeField] int blastDamage = 20;
    //[SerializeField] Material BarrelRed;


    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material = BarrelBrown;

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < triggerRadius && !triggered)
        {
            triggered = true;
            StartCoroutine(FlashRed());
            StartCoroutine(StartTimer());
        }
    }

    IEnumerator FlashRed()
    {
        rend.material = BarrelRed;
        yield return new WaitForSeconds(flashSpeed);
        StartCoroutine(FlashBrown());
    }
    IEnumerator FlashBrown()
    {
        rend.material = BarrelBrown;
        yield return new WaitForSeconds(flashSpeed);
        StartCoroutine(FlashRed());
    }
    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(explosionTimer);
        Explode();
    }

    private void Explode()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < triggerRadius)
        {
            Vector3 blastVector = (player.transform.position - transform.position);
            player.GetComponent<CharacterController>().Move(blastVector * blastForce);
            //TODO - implement a more physics based approach to the blast to have it throw the player rather than move them. Currently tried but player does not react to below.
            //player.GetComponent<Rigidbody>().AddForce(blastVector * blastForce);

            player.GetComponent<Player>().TakeDamage(blastDamage);
            //TODO - Blast damage to be dependant based on proximity to the barrel, more damage the closer player is
        }
        ParticleSystem temp = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Explode();
        }
    }

}
