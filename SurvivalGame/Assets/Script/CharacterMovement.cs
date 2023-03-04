using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    Animator animator;

    CharacterController characterController;

    public float speed = 2.0f;

    public float roatationSpeed = 25;

    public float jumpSpeed = 7.5f;

    public float gravity = 20.0f;

    public Inventory inventory;

    private Vector3 moveDirection = Vector3.zero;

    Vector3 inputVec;

    Vector3 targetDirection;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = -(Input.GetAxisRaw("Vertical"));
        float z = Input.GetAxisRaw("Horizontal");
        inputVec = new Vector3(x, 0, z);
    
        animator.SetFloat("Input X", z);
        animator.SetFloat("Input Z", -(x));
        bool runPressed = Input.GetKey("left shift");
        bool idle = x == 0 && z == 0;
        bool Moveing = x != 0 || z != 0;

        if (idle)
        {
            animator.SetBool("Moveing", false);
        }
        if (Moveing)
        {
            animator.SetBool("Moveing", true);
        }
        if (idle || !runPressed)
        {
            animator.SetBool("Running", false);
        }
        if (Moveing && runPressed)
        {
            animator.SetBool("Running", true);
        }
        //Jump
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;
        }
        characterController.Move(moveDirection * Time.deltaTime);
        UpdateMovement();
    }
    void UpdateMovement()
    {
        Vector3 motion = inputVec;
        motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? .7f : 1;
        RotateTowardMovementDirection();
        getCameraRealtive();
    }
    void RotateTowardMovementDirection()
    {
        if(inputVec != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(targetDirection),Time.deltaTime * roatationSpeed);
        }
    }
    void getCameraRealtive()
    {
        Transform cameraTransform = Camera.main.transform;
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        targetDirection = (h * right) + (v * forward);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        IInventoryItem item = hit.collider.GetComponent<IInventoryItem>();
        if (item != null)
        {
            inventory.AddItem(item);
        }
    }
}
