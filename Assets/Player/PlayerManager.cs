using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class PlayerManager : NetworkBehaviour {

    public GameObject canv;
    public Combat combatSc;
    private const string PLAYER_TAG = "Player";

    public float maxHP;
    public float minHP;

    [SyncVar]
    public float currentHP;





    // Use this for initialization
    void Start () {

        canv = GameObject.Find("Canvas");


        if (!isLocalPlayer)
        {
            gameObject.layer = LayerMask.NameToLayer("RemotePlayer");
            //canv.enabled = false;

        }

        if (isLocalPlayer)
        {
            GetComponent<AudioListener>().enabled = true;
        }

        combatSc = GetComponent<Combat>();


        RegisterPlayer();
        gameObject.tag = PLAYER_TAG;
        

    }
	
	// Update is called once per frame
	void Update () {

        if(isLocalPlayer)
            HUD(canv, combatSc.isAiming, GameObject.Find("HP"));

	}



    public void HUD(GameObject canvas, bool isAim, GameObject hpBar)
    {

        //only use crosshair when aiming;
        if (isAim)
            canvas.transform.FindChild("Crosshair").gameObject.SetActive(true);
        else
            canvas.transform.FindChild("Crosshair").gameObject.SetActive(false);

        hpBar.GetComponent<RectTransform>().localScale = new Vector3(1 / 100.0f * currentHP, 1, 1);

    }

    public void RegisterPlayer()
    {

        string _ID = "Player " + GetComponent<NetworkIdentity>().netId;
        transform.name = _ID;

    }

    [Command]
    public void CmdTakeDmg(float dmg)
    {

        if(currentHP > minHP)
            this.currentHP -= dmg;

    }

}
