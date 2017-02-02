using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour {

	public float startingHealth = 100;

	[SyncVar]
	public float currentHealth = 100;


	public bool isDead = false;
	Text playerHealth;

	// Use this for initialization
	void Start () {
		currentHealth = startingHealth;
		playerHealth = GameObject.Find("playerHealth").GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		SetPlayerHealth();
		if(currentHealth<0){
			currentHealth = 0;
		}
		if( ! isDead){
				if (currentHealth <= 0) {
					Death ();
			}
		}
	}

	[ClientRpc]
	public void RpcTakeDamage(float amount){
		if( ! isServer)
		return;

		if (isDead)
		return;

		currentHealth -= amount;

	}

	public void Death(){
		if (isDead)
			return;

		if(isServer){
			RpcDeath();
		}else{
			isDead = true;
			currentHealth = 0;
		}
	}

	[ClientRpc]
	public void RpcDeath(){
		if(isDead)
		return;
		isDead = true;
	}


	void SetPlayerHealth(){
	if(isLocalPlayer){
		playerHealth.text = "HP : " + currentHealth.ToString();
		}
	}
}
//
//			PlayerHealth playerHealth = c.gameObject.GetComponent<PlayerHealth> ();
//			if (playerHealth != null && playerHealth.currentHealth > 0) {
//				GameObject player = playerHealth.gameObject;
//				if (player.GetComponent<PlayerID> ().playerUniqueName != this.ownerName) {
//					CmdTellServerWhoGotShot (player.GetComponent<PlayerID> ().playerUniqueName, 20);
//					if (playerHealth.currentHealth <= 0) {
//						// player just died
//						IncreaseNumberOfKillsForPlayerWithName(this.ownerName);
//					}
//				}
//
//			}
//
//        }
//
//
//		[Command]
//		void CmdTellServerWhoGotShot(string uniqueID, int damage){
//			GameObject obj = GameObject.Find (uniqueID);
//			obj.GetComponent<PlayerHealth> ().TakeDamage (damage);
//			//Debug.Log ("player health: " + obj.GetComponent<PlayerHealth> ().currentHealth.ToString ());
//		}
//
//		void IncreaseNumberOfKillsForPlayerWithName(string playerUniqueName){
//			GameObject obj = GameObject.Find (playerUniqueName);
//			obj.GetComponent<PlayerKills>().KillCount++;
//			
//		}
//	}
//}