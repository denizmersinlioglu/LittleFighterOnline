using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerKills : NetworkBehaviour {

	[SyncVar] public int KillCount =0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		SetKillCountText();
	}
	public void SetKillCountText(){
		if(isLocalPlayer){
			GameObject killsText = GameObject.Find("killText");
			killsText.GetComponent<Text>().text = "Kills : " + KillCount.ToString();
		}
	}
}
