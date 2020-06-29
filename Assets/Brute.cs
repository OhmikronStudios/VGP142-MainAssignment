using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Brute : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;


    public GameObject target;
    public GameObject playerTarget;
    public GameObject axe;
    public GameObject axeSpawn;

    public int enemyType;

    public bool chasingPlayer = false;
    public bool moving = true;
    public float chaseRange = 8.0f;
    
    float attackRange = 2.0f;
    float projectileSpeed = 5.0f;

    public bool autoGenPath;
    public string pathName;
    public GameObject[] path;
    public int pathIndex;
    public float distanceToNextNode;
    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        //Random Enemy Type - alters behaviour
        enemyType = Random.Range(0, 3);
        // Type 0 - Brute
        // Type 1 - Ranged
        // Type 2 - Suicidal
        SetTypeParameters();

        playerTarget = GameObject.FindGameObjectWithTag("Player");

        if(string.IsNullOrEmpty(pathName))
        {
            pathName = "pathNode";
        }


        if(autoGenPath)
        {
            path = GameObject.FindGameObjectsWithTag("PathNode");
        }
        
        if(distanceToNextNode <= 0)
        {
            distanceToNextNode = 1.0f;
        }

        if(path.Length > 0)
        {
            pathIndex = 0;
            target = path[pathIndex]; }

       
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target.transform);

        if (moving)
        {
            TargetChoose();
            TargetChase();
        }

        if (Vector3.Distance(playerTarget.transform.position, transform.position) < attackRange && moving)
        {
            StartCoroutine(Attack());
        }
    }

    void SetTypeParameters()
    {
        if(enemyType == 0) // Brute
        {
            //Default Values, don't need to change anything
        }
        else if(enemyType == 1) // Range
        {
            chaseRange = 12.0f;
            attackRange = 10.0f;
        }
        else if(enemyType == 2) // Suicidal
        {
            chaseRange = 16.0f;
            attackRange = 1.0f;
        }
    }

    private void TargetChoose()
        {
            float distToTarget = Vector3.Distance(playerTarget.transform.position, transform.position);
            if (distToTarget < chaseRange)
            {
                chasingPlayer = true;
                target = playerTarget;
            }
            else
            {
                chasingPlayer = false;
                target = path[pathIndex];
            }
        }


    IEnumerator Attack()
    {
        moving = false;
        if (enemyType == 0) // Brute
            animator.SetTrigger("Melee");

        else if (enemyType == 1) // Range
            
        animator.SetTrigger("Throw");

        else if (enemyType == 2) // Suicidal
            animator.SetTrigger("Explode");

        yield return new WaitForSeconds(1.0f);
        moving = true;
    }

    private void ThrowAxe()
    {
        Rigidbody temp = Instantiate(axe.GetComponent<Rigidbody>(), axeSpawn.transform.position, axeSpawn.transform.rotation);
        temp.AddForce(axeSpawn.transform.forward * projectileSpeed, ForceMode.Impulse);
    }


    private void TargetChase()
    {
        if (!chasingPlayer)
        {
            if (agent.remainingDistance < distanceToNextNode)
            {
                if (path.Length > 0)
                {
                    pathIndex++;
                    pathIndex %= path.Length;

                }
                Debug.Log("proceding to" + path[pathIndex]);
                target = path[pathIndex];
            }
           

            animator.SetFloat("Speed", Mathf.Abs(transform.TransformDirection(agent.velocity).z));
        }
        agent.SetDestination(target.transform.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            agent.isStopped = true;
            //animator.SetTrigger("Dying");
            Destroy(gameObject, 2.5f);
        }
    }
}
