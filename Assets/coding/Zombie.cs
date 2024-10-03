using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 8.0f;
    public float gravity = 20.0f;
    public float rotationSpeed = 100.0f;

    public bool isGrounded = false;
    public bool isDef = false;
    public bool dancing = false;
    public bool walking = false;

    private Animator animator;
    private CharacterController characterController;
    private Vector3 inputVector = Vector3.zero;
    private Vector3 targetDirection = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Time.timeScale = 1;
        isGrounded = characterController.isGrounded;
    }

    void Update()
    {
        float z = Input.GetAxis("Horizontal");
        float x = Input.GetAxis("Vertical");

        animator.SetFloat("inputx", x);
        animator.SetFloat("inputz", z);

        if (z != 0 || x != 0)
        {
            walking = true;
            animator.SetBool("walking", walking);
        }
        else
        {
            walking = false;
            animator.SetBool("walking", walking);
        }

        isGrounded = characterController.isGrounded;
        if (isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpForce; // กระโดด
            }
        }

        // เพิ่มแรงโน้มถ่วง
        moveDirection.y -= gravity * Time.deltaTime;

        // เคลื่อนที่ตัวละคร
        characterController.Move(moveDirection * Time.deltaTime);

        inputVector = new Vector3(x, 0, z);
        UpdateMovement();
    }

    void UpdateMovement()
    {
        Vector3 motion = inputVector;
        motion = ((Mathf.Abs(motion.x) > 1) || (Mathf.Abs(motion.z) > 1)) ? motion.normalized : motion;

        RotateTowardMovement();
        ViewRelativeMovement();
    }

    void RotateTowardMovement()
    {
        if (inputVector != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void ViewRelativeMovement()
    {
        Transform cameraTransform = Camera.main.transform;
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0.0f;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0.0f, -forward.x);
        targetDirection = (Input.GetAxis("Horizontal") * right) + (Input.GetAxis("Vertical") * forward);
    }
}
