using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour {

    public GameObject camObj;
    public Combat combatSc;
    public Movement moveSc;
    public Animator animC;
    public LayerMask mask;
    public Transform rayStartTrans;

    public float rotX;
    public float rotY;
    public float rotXV;
    public float rotYV;
    public float newRotX;
    public float newRotY;
    public float smoothTime = 0.1f;
    public float changeOffSpeed;
    public float sprintChangeOffSpeed;


    public Quaternion newCamRot;
    public Vector3 newCamPos;
    public Vector3 defaultOff;
    public Vector3 aimOff;
    public Vector3 sprintOff;
    public Vector3 offset;
    public Vector3 rayStartPos;
    public float hitOffset;



    public Transform spine;
    // Use this for initialization
    void Start () {

        camObj = GameObject.Find("Main Camera");

        if (!isLocalPlayer)
        {
            Destroy(this);
            return;
        }


        animC = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!spine)
        {
            Transform[] children = GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (child.name == "mixamorig:Spine")
                {
                    spine = child.transform;
                }
            }
        }


        Movement moveSc = GetComponent<Movement>();
        combatSc = GetComponent<Combat>();
        moveSc = GetComponent<Movement>();

        rotX += Input.GetAxis("Mouse X") * 2.0f;
        rotY -= Input.GetAxis("Mouse Y") * 2.0f;

        if (moveSc.isWalking || combatSc.isAiming)
            transform.rotation = Quaternion.Euler(0, rotX, 0);

        if (combatSc.isAiming)
        {
            offset = Vector3.Lerp(offset, aimOff, changeOffSpeed);

        }
        else if (moveSc.isSprinting)
        {
            offset = Vector3.Lerp(offset, sprintOff, sprintChangeOffSpeed);
        }
        else
        {
            offset = Vector3.Lerp(offset, defaultOff, changeOffSpeed);
        }

        //Clamp Rot
        rotY = Mathf.Clamp(rotY, -80, 80);

        //Smoothing
        newRotX = Mathf.SmoothDamp(newRotX, rotX, ref rotXV, smoothTime);
        newRotY = Mathf.SmoothDamp(newRotY, rotY, ref rotYV, smoothTime);




        newCamRot = Quaternion.Euler(newRotY, newRotX, 0);

        //Check if camera hit obsticle.
        RaycastHit hit;
        //Debug.DrawRay(newCamRot * new Vector3(offset.x, offset.y, 0) + transform.position, -camObj.transform.forward * -offset.z, Color.red);
        if (Physics.Raycast( newCamRot * new Vector3(offset.x, offset.y, 0) + transform.position, -camObj.transform.forward, out hit, -offset.z, mask))
        {
            hitOffset = -offset.z - hit.distance;

        }
        else
        {
            hitOffset = 0.0f;
        }

        newCamPos = newCamRot * new Vector3(offset.x, offset.y, offset.z  + hitOffset) + transform.position;


        camObj.transform.position = newCamPos;
        camObj.transform.rotation = newCamRot;

        // Position of rayStart (where you shoot)
        rayStartTrans = transform.FindChild("rayStart").transform;
        rayStartTrans.position = newCamRot * rayStartPos + transform.position;
        rayStartTrans.rotation = newCamRot;

    }


    public void LateUpdate()
    {



        if (combatSc.isAiming && combatSc != null)
        {

            
            if (rotY < 0)
            {
                spine.rotation = spine.rotation * Quaternion.Euler(rotY, 0, 0);
            }
            else if (rotY > 0)
            {
                spine.rotation = spine.rotation * Quaternion.Euler(rotY, 0, 0);
            }
        }
    }
}
