using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// คลาสควบคุมการเคลื่อนไหวของตัวละคร
public class RenyMovement : MonoBehaviour
{
    Animator anim;
    public float speed = 10.0f; 
    public float rotationSpeed = 100.0f; 
    public float jumpForce = 7.0f; 
    public float gravity = 20.0f;

    public bool isGrounded = false;
    public bool suprised = false; 
    public bool combo = false; 
    public bool walk = false; 

    Vector3 moveDirection = Vector3.zero;
    CharacterController controller; 
    void Start()
    {
        
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        Time.timeScale = 1;
    }

    void Update()
    {
        
        isGrounded = controller.isGrounded;

        
        float x = -(Input.GetAxis("Vertical"));
        float z = Input.GetAxis("Horizontal");
        Vector3 inputDirection = new Vector3(x, 0, z).normalized;

        
        if (Input.GetKey(KeyCode.W))
        {
            
            anim.SetBool("suprised", true);
            anim.SetBool("walk", true);
            anim.SetBool("combo", false);
            Debug.Log("W key is pressed: suprised and walking");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            
            anim.SetBool("combo", true);
            anim.SetBool("walk", false);
            anim.SetBool("suprised", false);
            Debug.Log("S key is pressed: combo");
        }
        else
        {
           
            anim.SetBool("walk", false);
            anim.SetBool("suprised", false);
            anim.SetBool("combo", false);
        }

        
        if (isGrounded)
        {
            
            moveDirection = inputDirection * speed;

           
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpForce;
                anim.SetTrigger("jump");
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

      
        controller.Move(moveDirection * Time.deltaTime);
    }
}
