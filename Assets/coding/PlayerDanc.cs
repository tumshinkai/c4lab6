using System.Collections;
using UnityEngine;

public class PlayerDance : MonoBehaviour
{
    public float speed = 10.0f;
    public float jumpForce = 8.0f;
    public float gravity = 20.0f;
    public float rotationSpeed = 100.0f;

    public bool isGrounded = false;
    public bool isDif = false;
    public bool dancing = false;
    public bool isWalking = false;
    public bool isTaking = false;

    private Animator animator;
    private CharacterController characterController;
    private Vector3 inputVector = Vector3.zero;
    private Vector3 targetDirection = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private GameController gameController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Time.timeScale = 1;
        isGrounded = characterController.isGrounded;

        if (gameController == null)
        {
            GameObject controllerObject = GameObject.Find("GameController");
            if (controllerObject != null)
            {
                gameController = controllerObject.GetComponent<GameController>();
            }
            else
            {
                Debug.LogError("GameController object not found!");
            }
        }
    }

    void Update()
    {
        float z = Input.GetAxis("Horizontal");
        float x = -Input.GetAxis("Vertical");

        animator.SetFloat("inputX", -x);
        animator.SetFloat("inputZ", z);

        isWalking = x != 0 || z != 0;
        animator.SetBool("isWalking", isWalking);

        isGrounded = characterController.isGrounded;

        inputVector = new Vector3(x, 0, z);
        UpdateMovement();

        if (isTaking && Input.GetKeyDown(KeyCode.E))
        {
            animator.SetBool("isTaking", isTaking);
            StartCoroutine(WaitForTaking(4.7f));
            gameController.GetItem();
        }
    }

    IEnumerator WaitForTaking(float time)
    {
        yield return new WaitForSeconds(time);
        isTaking = false;
        animator.SetBool("isTaking", isTaking);
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
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(targetDirection),
                rotationSpeed * Time.deltaTime
            );
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "item")
        {
            isTaking = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "item")
        {
            isTaking = false;
        }
    }
}
