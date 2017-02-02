using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerRespawn : NetworkBehaviour {

	private bool isRespawning =false;
	public int countDownStartValue = 9;
	private int countDownCurrentValue;
	// Use this for initialization
	void Start () {
		isRespawning = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(this.GetComponent<PlayerHealth>().currentHealth <= 0 && !isRespawning)
		{	
			isRespawning = true;
			Invoke("RpcRespawn",countDownStartValue);

			if(isLocalPlayer){
			GameObject respawnTextObject = GameObject.Find("respawnText");
			Text respawnText = respawnTextObject.GetComponent<Text>();
			countDownCurrentValue = countDownStartValue;
			InvokeRepeating("UpdateRespawnText",1.0f,1.0f);
			respawnText.text= countDownCurrentValue.ToString();
			}
		}
	}
	[ClientRpc]
	public void RpcRespawn(){
		Transform spawn = NetworkManager.singleton.GetStartPosition();
		transform.position = spawn.position;

		GetComponent<PlayerHealth>().currentHealth = GetComponent<PlayerHealth>().startingHealth;

		GetComponent<PlayerHealth>().isDead = false;
		isRespawning =false;
	}

	void UpdateRespawnText(){
		GameObject respawnTextObject = GameObject.Find("respawnText");
		Text respawnText = respawnTextObject.GetComponent<Text>();
		countDownCurrentValue--;
		respawnText.text= countDownCurrentValue.ToString();
			if(countDownCurrentValue <= 0){
			CancelInvoke("UpdateRespawnText");
			respawnText.text= "";
			}
	}
}
