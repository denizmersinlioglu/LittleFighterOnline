using UnityEngine;
using Photon;
public class RandomMatchmaker : Photon.PunBehaviour {

	
	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        if(isFailed){
		GUILayout.Label("Can't join random room!");
		}
    }
	// Update is called once per frame
	public override void OnJoinedLobby()
	{
    PhotonNetwork.JoinRandomRoom();
	} 

	void OnJoinedRoom(){
		GameObject Character = PhotonNetwork.Instantiate("Character", Vector3.zero, Quaternion.identity, 0);
		CharacterControl controller = Character.GetComponent<CharacterControl>();
    	controller.enabled = true;
    	CharacterCamera camera = Character.GetComponent<CharacterCamera>();
    	camera.enabled = true;
	}

	bool isFailed;
		void OnPhotonRandomJoinFailed()
	{	
		isFailed = true;
   	 	Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom(null);
		
	}
}
