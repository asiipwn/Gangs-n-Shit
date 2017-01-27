using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach : MonoBehaviour {

    public Transform gun;

    public Quaternion newRot;
    public Vector3 newPos;
    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {

        if (!GameObject.Find("Player(Clone)"))
            return;

        gun = gameObject.transform;

        gun.parent = GameObject.Find("mixamorig:RightHand").transform;

        gun.localPosition = new Vector3(newPos.x, newPos.y, newPos.z);
        gun.localRotation = Quaternion.Euler(newRot.x,newRot.y, newRot.z);

    }
}
