using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    CharacterController cc;
    Animator anim;
    [SerializeField] Text kunaiCounter;
    //Rigidbody rb;

    //Movement Parameters
    public float speed = 10.0f;
    public float jumpSpeed = 8.0f;
    public float rotationSpeed = 5.0f;
    public float gravitySpeed = 9.81f;
    public float projectileSpeed = 10.0f;

    //Gameplay Parameters
    [SerializeField] int health = 100;
    [SerializeField] Slider healthBar;
    public int ammo = 10;
    
    
    //Objects
    [SerializeField] GameObject kunai;
    [SerializeField] GameObject kunaiSpawn;


   


    // Setting type to 0 means use SimpleMove()
    // Setting type to 1 means use Move()
    [SerializeField] int type = 1;

    Vector3 moveDirection;

    //[SerializeField] bool isGodMode;
    //[SerializeField] float timerGodMode;

    //[SerializeField] float jumpBoost;
    //[SerializeField] float timerJumpBoost;




    // Start is called before the first frame update
    void Start()
    {
        try
        {
            cc = GetComponent<CharacterController>();
            //rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();

            if (speed <= 0)
            { speed = 6.0f; Debug.Log("Speed not set on" + name + "defaulting to" + speed); }

            if (jumpSpeed <= 0)
            { jumpSpeed = 8.0f; Debug.Log("JumpSpeed not set on" + name + "defaulting to" + jumpSpeed); }

            if (rotationSpeed <= 0)
            { rotationSpeed = 10.0f; Debug.Log("RotationSpeed not set on" + name + "defaulting to" + rotationSpeed); }

            if (gravitySpeed <= 0)
            { gravitySpeed = 9.81f; Debug.Log("GravitySpeed not set on" + name + "defaulting to" + gravitySpeed); }

            //isGodMode = false;
            //if (timerGodMode <= 0)
            //{ timerGodMode = 2.0f; Debug.Log("timerGodMode not set on" + name + "defaulting to" + timerGodMode); }
            //if (jumpBoost <= 0)
            //{ timerGodMode = 20.0f; Debug.Log("JumpBoost not set on" + name + "defaulting to" + jumpBoost); }
            //if (timerJumpBoost <= 0)
            //{ timerGodMode = 5.0f; Debug.Log("timerJumpBoost not set on" + name + "defaulting to" + timerJumpBoost); }

            moveDirection = Vector3.zero;
        }

        catch (ArgumentNullException e)
        { Debug.LogWarning(e.Message); }
        SetMaxHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attack();

        RaycastHit hit;
        Debug.DrawRay(kunaiSpawn.transform.position, kunaiSpawn.transform.forward * 15.0f, Color.red);
        if (Physics.Raycast(kunaiSpawn.transform.position, kunaiSpawn.transform.forward, out hit, 15.0f))
        {
            //Debug.Log("Raycast hit " + hit.transform.name);
            //if (hit.transform.tag == "Enemy")
            //{
            //    hit.transform.GetComponent<Sheep>().Freeze();
            //}
        }

        SetHealth(health);
        SetKunai();


    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.R))
        { Punch(); }
        else if (Input.GetKeyDown(KeyCode.E))
        { Kick(); }
        if (Input.GetButtonDown("Fire1"))
        {
            if (ammo > 0)
            {
                ammo -= 1;
                anim.SetTrigger("Throw");
            }
            //Firekunai();
        }

    }

    private void Movement()
    {
        // Using SimpleMove()
        if (type == 0)
        {
            //use if not using MouseLook.CS
            //transform.Rotate (0, Input.GetAxis("Horizontal" * rotationSpeed, 0);

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            float curSpeed = Input.GetAxis("Vertical") * speed;
            cc.SimpleMove(forward * curSpeed);

        }

        else if (type == 1)
        {
            if (cc.isGrounded)
            {

                moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

                //use if not using MouseLook.CS
                //transform.Rotate (0, Input.GetAxis("Horizontal" * rotationSpeed, 0);

                moveDirection = transform.TransformDirection(moveDirection);

                moveDirection *= speed;
                anim.SetFloat("Speed", moveDirection.z);
                if (Input.GetButtonDown("Jump"))
                    moveDirection.y = jumpSpeed;
            }

            moveDirection.y -= gravitySpeed * Time.deltaTime;

            cc.Move(moveDirection * Time.deltaTime);


        }
    }

    private void Punch()
    {
        anim.SetTrigger("Attack");
    }
    private void Kick()
    {
        anim.SetTrigger("Attack2");
    }
    private void FireKunai()
    {
        Rigidbody temp = Instantiate(kunai.GetComponent<Rigidbody>(), kunaiSpawn.transform.position, kunaiSpawn.transform.rotation);
        temp.AddForce(kunaiSpawn.transform.forward * projectileSpeed, ForceMode.Impulse);
    }


    void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    void SetHealth(int health)
    {
        healthBar.value = health;
    } 
    void SetKunai()
    {
        kunaiCounter.text = ammo.ToString();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            FindObjectOfType<GameManager>().LoadGameOver();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("OnCollisionEnter with: " + hit.gameObject.name);
        switch (hit.gameObject.tag)
        {
            case "Pickup_Speed":
                StartCoroutine(SpeedBoost());
                Destroy(hit.gameObject);
                break;
            case "Pickup_Ammo":
                GainAmmo(5);
                Destroy(hit.gameObject);
                break;
            case "Pickup_Health":
                GainHealth(25);
                Destroy(hit.gameObject);
                break;
            case "Pickup_Win":
                Win();
                Destroy(hit.gameObject);
                break;
        }

    }
    
    //Pickup Effects
    IEnumerator SpeedBoost()
    {
        speed *= 2;
        jumpSpeed *= 2;
        yield return new WaitForSeconds(10.0f);
        speed /= 2;
        jumpSpeed /= 2;
    }

    void GainAmmo(int a)
    {
        ammo += a;
        Debug.Log("Ammo is at: " + ammo);    
    }

    void GainHealth(int h)
    {
        health += h;
        if (health > 100)
        {
            health = 100;
            Debug.Log("health is at: " + health);
        }
        Debug.Log("health is at: " + health);
    }

    void Win()
    {
        FindObjectOfType<GameManager>().LoadWinScreen();
    }

}

