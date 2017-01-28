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

    public bool isJumpingOverObsticle;
    public float obsticleJumpSpeed;
    public float obsticalH;
    public float obsticalL;
    public int curJumpstate;
    public Vector3 endPos;

    // Use this for initialization
    void Start() {

        curJumpstate = 0;
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

        if (controller.isGrounded && !isJumpingOverObsticle)
        {

            IsPlayerWalking();

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
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(transform.position.x ,transform.position.y - 0.5f, transform.position.z), transform.forward, out hit, 2.0f))
                {
                    ObsticleManager omSc = hit.collider.GetComponent<ObsticleManager>();
                    obsticalH = omSc.height;
                    obsticalL = omSc.length;
                    anim.SetBool("smallJover", true);
                    isJumpingOverObsticle = true;
                    endPos = new Vector3(transform.position.x, transform.position.y + obsticalH + 0.7f, transform.position.z + hit.distance);
                    obsticleJumpSpeed = 0.12f;
                }
                else
                {
                    gravity = defaultGravity;
                    moveDirection.y = jumpspeed;
                }

            }

            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);

        }
        else if(isJumpingOverObsticle)
        {

            gravity = 0.0f;
            transform.position = Vector3.Lerp(transform.position, endPos, obsticleJumpSpeed);

            if (Vector3.Distance(transform.position, endPos) < 0.5f && curJumpstate == 0)
            {
                endPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + obsticalL + 1f);
                curJumpstate += 1;
                obsticleJumpSpeed = 0.04f;
            }
            else if (Vector3.Distance(transform.position, endPos) < 1f && curJumpstate == 1)
            {
                endPos = new Vector3(transform.position.x, transform.position.y - obsticalH - 0.5f, transform.position.z + 0.5f);
                curJumpstate += 1;
                obsticleJumpSpeed = 0.15f;
                anim.SetBool("smallJover", false);
            }
            else if (Vector3.Distance(transform.position, endPos) < 0.15f && curJumpstate == 2)
            {
                curJumpstate = 0;
                isJumpingOverObsticle = false;

            }




            }
            else
        {
            isWalking = false;
            gravity = defaultGravity;
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }




        

    }



    public void AnimateMove(float hMove, float vMove, Animator ac) {


        ac.SetBool("Sprinting", isSprinting);

        ac.SetFloat("Forward", vMove);
        ac.SetFloat("Strafe", hMove);

    }

    public void smallObsticleJump(Animator ac)
    {

    }

    void IsPlayerWalking()
    {

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {

            isWalking = false;
        }
        else
        {
            isWalking = true;
        }
    }

}
