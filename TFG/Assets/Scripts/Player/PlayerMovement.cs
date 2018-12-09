using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 6f;
    public float walkingSpeed = 4f;
    public float rotation = 100f;
    Animator anim;
    bool enablingMove;
    bool isProtected;

    public Inventory inventory;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        enablingMove = true;

        if (GameSystemInGame.MyInstance.LastCheckPointPos != new Vector3(0,0,0))
        {
            transform.position = GameSystemInGame.MyInstance.LastCheckPointPos;
        }
    }

    void Update () {
        Move();
        Animating();
    }

    void Move()
    {
        if (enablingMove == true)
        {
            if (Input.GetKey(KeyCode.W) && isProtected == false)
            {
                transform.Translate(new Vector3(0, 0, 1) * speed * Time.deltaTime);
            }else if(Input.GetKey(KeyCode.W) && isProtected == true)
            {
                transform.Translate(new Vector3(0, 0, 1) * walkingSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, -1) * walkingSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(new Vector3(0, 1, 0) * rotation * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(new Vector3(0, -1, 0) * rotation * Time.deltaTime);
            }
        }
    }

    void Animating()
    {
        if (enablingMove == true)
        {
            if (Input.GetKey(KeyCode.W) && isProtected == false)
            {
                anim.SetBool("IsRunning", true);
            }
            else if (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.W) && isProtected == true))
            {
                anim.SetBool("IsRunning", false);
                anim.SetBool("IsWalking", true);
            }
            else
            {
                anim.SetBool("IsRunning", false);
                anim.SetBool("IsWalking", false);
            }
        }
    }

    public void EnableMovement()
    {
        enablingMove = true;
    }
    public void DisableMovement()
    {
        enablingMove = false;
    }
    public void PlayerProtected(bool playerProtected)
    {
        isProtected = playerProtected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            inventory.AddItem(other.GetComponent<Item>());
            Destroy(other.gameObject);
        }
    }
}
