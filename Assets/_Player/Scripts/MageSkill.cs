using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MageSkill : NetworkBehaviour {
	
	public GameObject fireBallPrefab;
	public GameObject frostBallPrefab;


	public void CastFireBall(Transform source_position ){

		GameObject FireBall = GameObject.Instantiate(fireBallPrefab,source_position.position,source_position.rotation);
		NetworkServer.Spawn(FireBall);

	}

	public void CastFrostBall(Transform source_position ){

		GameObject FrostBall = GameObject.Instantiate(frostBallPrefab,source_position.position,source_position.rotation);
		NetworkServer.Spawn(FrostBall);

	}
}
