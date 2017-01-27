using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Movement : NetworkBehaviour {

    public Combat combatSc;

    public float h;
    public float v;
    public float walkSpeed;
    public float sprintSpeed;
    public float speed;
    public float jumpspeed;
    public float defaultGravity;
    public float gravity;
    private Vector3 moveDirection = Vector3.zero;

    public bool isWalking;
    public bool isSprinting;

    // Use this for initialization
    void Start() {

        defaultGravity = 20.0f;

        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }

    }

    // Update is called once per frame
    void Update() {

        CharacterController controller = GetComponent<CharacterController>();
        Animator anim = GetComponent<Animator>();
        combatSc = GetComponent<Combat>();


        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
       // anim.SetBool("isGrounded", controller.isGrounded);

        if (controller.isGrounded)
        {

            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {

                isWalking = false;
            }
            else
            {
                isWalking = true;
            }

            if (isWalking && Input.GetKey(KeyCode.LeftShift) && v > 0 && combatSc.isAiming == false)
            {
                speed = sprintSpeed;
                isSprinting = true;
            }
            else
            {
                speed = walkSpeed;
                isSprinting = false;
            }


            gravity = defaultGravity * 9;


            if (isSprinting)
            {
                h = 0;

            }

            moveDirection = new Vector3(h, 0, v);

            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            AnimateMove(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), anim);





            if (Input.GetButton("Jump"))
            {
                gravity = defaultGravity;
                moveDirection.y = jumpspeed;
                //anim.SetTrigger("Jump");
            }



        }
        else
        {
            isWalking = false;
            gravity = defaultGravity;
        }


        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        

    }


    public void AnimateMove(float hMove, float vMove, Animator ac) {


        ac.SetBool("Sprinting", isSprinting);

        ac.SetFloat("Forward", vMove);
        ac.SetFloat("Strafe", hMove);

    }
}
