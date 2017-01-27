using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Combat : NetworkBehaviour {



    public bool isAiming;
    
    public Transform rayStart;
    public LayerMask mask;

    public GameObject decal;
    public GameObject blood;
    public AudioSync audioSync;

	// Use this for initialization
	void Start () {

        audioSync = GameObject.Find("Audio Manager").GetComponent<AudioSync>();
    }
	
	// Update is called once per frame
	void Update () {



        if (isLocalPlayer)
        {
            Animator animC = GetComponent<Animator>();

            if (Input.GetMouseButton(1))
            {

                animC.SetBool("Aiming", true);
                isAiming = true;

            }
            else
            {
                animC.SetBool("Aiming", false);
                isAiming = false;
            }



            if (isAiming && Input.GetMouseButtonDown(0) && isLocalPlayer)
            {

                Shoot();

            }

        }



    }


    [Client]
    public void Shoot()
    {

        RaycastHit hit;

        Debug.DrawRay(rayStart.position, rayStart.transform.forward * 100.0f, Color.blue);

        if (Physics.Raycast(rayStart.position, rayStart.transform.forward, out hit, 100.0f)) {

            CmdCallAudioSync();

            if (hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name);

                CmdHitParticle(hit.point, hit.normal, "Player");
            }
            else
            {
                CmdHitParticle(hit.point, hit.normal, "Object");
            }
            
        }

    }

    [Command]
    void CmdPlayerShot(string _ID)
    {

        Debug.Log(_ID + "got shot");
        GameObject.Find(_ID).GetComponent<PlayerManager>().CmdTakeDmg((int)Random.Range(5f, 1f));


    }

    [Command]
    void CmdHitParticle(Vector3 hitpoint, Vector3 hitnormal, string obj)
    {


        if (obj == "Player")
        {
            GameObject bl = (GameObject)Instantiate(blood, hitpoint, Quaternion.FromToRotation(Vector3.forward, hitnormal));
            NetworkServer.Spawn(bl);
        }else if(obj == "Object")
        {

            GameObject de = (GameObject)Instantiate(decal, hitpoint, Quaternion.FromToRotation(Vector3.forward, hitnormal));
            NetworkServer.Spawn(de);
        }


    }

    [Command]
    void CmdCallAudioSync()
    {
        audioSync.PlaySound(0, gameObject.name);
    }

}
