using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour {

[SyncVar] public string playerUniqueName;
private NetworkInstanceId playerNetID;

	public override void OnStartLocalPlayer(){
		
		GetNetIdentity();   //Getting network ID
		SetIdentity();  	   //Set the ID of the player

	}

	
	// Update is called once per frame
	void Update () {
		if(transform.name =="" || transform.name == "Character(Clone)" ){
		SetIdentity();
		}
	}

	[Client]
	void GetNetIdentity(){
		playerNetID =GetComponent<NetworkIdentity>().netId;
		// tell all client about the player ID
		CmdTellServerMyIdentity(MakeUniqueIdentity());
	}

	void SetIdentity(){
		if( ! isLocalPlayer){
			this.transform.name = playerUniqueName;
		}
		else{
			// if it is local player wee need to create it
			transform.name = MakeUniqueIdentity();
		}
	}

	string MakeUniqueIdentity(){
		return "Player" + playerNetID.ToString();
	}

	[Command]
	void CmdTellServerMyIdentity(string name){
		playerUniqueName = name;
	}


}
