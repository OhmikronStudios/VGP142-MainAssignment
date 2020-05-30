using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    public GameObject target;
    public GameObject playerTarget;
    public GameObject spawner;
    public BoxCollider coll;

    public bool chasingPlayer = false;
    public bool moving = true;
    float attackRange = 2.0f;

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

        if (string.IsNullOrEmpty(pathName))
        {
            pathName = "pathNode";
        }


        if (autoGenPath)
        {
            path = GameObject.FindGameObjectsWithTag("PathNode");
        }

        if (distanceToNextNode <= 0)
        {
            distanceToNextNode = 1.0f;
        }

        if (path.Length > 0)
        {
            pathIndex = 0;
            target = path[pathIndex];
        }


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

    IEnumerator Attack()
    {
        moving = false;
        animator.SetTrigger("Attacking");
        yield return new WaitForSeconds(1.0f);
        moving = true;

    }

    private void TargetChoose()
    {
        float distToTarget = Vector3.Distance(playerTarget.transform.position, transform.position);
        if (distToTarget < 10.0f)
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
            Debug.DrawLine(transform.position, target.transform.position, Color.white);
            Debug.DrawRay(transform.position, transform.forward * 1.0f, Color.red);
            Debug.DrawRay(transform.position, transform.up * 1.0f, Color.blue);
            Debug.DrawRay(transform.position, transform.right * 1.0f, Color.green);

            animator.SetFloat("Speed", Mathf.Abs(transform.TransformDirection(agent.velocity).z));
        }
        agent.SetDestination(target.transform.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            coll.enabled = false;
            agent.isStopped = true;
            animator.SetTrigger("Dying");
            Destroy(gameObject, 2.5f);
            Instantiate(spawner, transform.position, Quaternion.identity);

        }
    }


}



