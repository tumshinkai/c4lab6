using System.Collections;
using UnityEngine;


public class CharMovement : MonoBehaviour
{
    Animator anim; 
    public float speed = 10.0f; 
    public float rotationSpeed = 100.0f; 
    public float jumpForce = 7.0f; 
    public float gravity = 20.0f; 

    public bool isGrounded = false; 
    CharacterController controller; 
    Vector3 moveDirection = Vector3.zero;

    void Awake()
    {

        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {

        anim.SetBool("suprised", true);
        anim.SetBool("combo", false);
        Time.timeScale = 1; 
    }

    void Update()
    {

        isGrounded = controller.isGrounded;


        float x = -(Input.GetAxis("Vertical"));
        float z = Input.GetAxis("Horizontal");
        Vector3 inputDirection = new Vector3(x, 0, z).normalized;


        anim.SetBool("walk", inputDirection != Vector3.zero);


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

        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("suprised", true);
            anim.SetBool("combo", false);
            Debug.Log("W key is pressed: walking forward");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("suprised", true);
            anim.SetBool("combo", false);
            Debug.Log("S key is pressed: walking backward");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("suprised", true);
            anim.SetBool("combo", true);
            Debug.Log("D key is pressed: right combo");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("combo", true);
            anim.SetBool("walk", true);
            Debug.Log("A key is pressed: left combo");
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            anim.SetBool("combo", true);
            anim.SetBool("walk", false);
            anim.SetBool("suprised", false);
            Debug.Log("Z key is pressed");
        }
        else
        {

            anim.SetBool("suprised", false);
            anim.SetBool("combo", false);
        }
    }
}
