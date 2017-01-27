using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class AudioSync : NetworkBehaviour {

    public AudioSource audioS;
    public AudioClip[] clips;

	// Use this for initialization
	void Start () {

    }
	
	public void PlaySound(int clipID, string playerID)
    {
        CmdSendSoundToServer(clipID, playerID);
        Debug.Log("PlaySound clipID: " + clipID + " playerID: " + playerID);
    }

    [Command]
    void CmdSendSoundToServer(int clipID, string playerID)
    {
        RpcSendSoundToClient(clipID, playerID);
    }

    [ClientRpc]
    void RpcSendSoundToClient(int clipID, string playerID)
    {
        audioS = GameObject.Find(playerID).GetComponent<AudioSource>();
        audioS.PlayOneShot(clips[clipID]);
    }
}
