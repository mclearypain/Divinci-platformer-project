using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Declare refrence veriables 
    CharacterController controller;
    Animator animator;

    //variables to store optimized setter/getter parameter IDs
    int isWalkingHash;
    int isRunningHash;

    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float gravityScale;
    [SerializeField] float turnSmoothTime = 0.1f;
    bool isMovementPressed;
    float turnSmoothVelocity;

    public Vector3 moveDirection;
    

    // Start is called before the first frame update
    void Start()
    {
        //initially set reference variables
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        handleAnimation();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //Store player input in Move direction
        moveDirection = new Vector3(horizontal, moveDirection.y, vertical);

        if(controller.isGrounded){
            if(Input.GetButtonDown("Jump")){
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        isMovementPressed = moveDirection.x != 0f || moveDirection.z != 0f;
    
        handleRotation();
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);


    }

    void handleAnimation()
    {
        //get parameter values for animator
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);

        //start walking if movement is pressed is true and not already walking
        if(isMovementPressed && !isWalking){
             animator.SetBool(isWalkingHash, true);
        }else if(!isMovementPressed && isWalking){
            animator.SetBool(isWalkingHash, false);
        }
    }

    void handleRotation(){
        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f); 
    }
       
}
