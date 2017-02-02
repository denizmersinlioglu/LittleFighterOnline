
using UnityEngine;
using UnityEngine.Networking;

public class Fireball_Script : NetworkBehaviour {

public string targetName;

float fireBallVelocity = 300f;
float turn =5f;
Rigidbody fireballRigidBody;


public AudioClip fireballAudioClip;
GameObject owner;
public GameObject target;

private Vector3 initialPosition;
	// Use this for initialization
	void Start () {
		fireballRigidBody = this.GetComponent<Rigidbody>();
		AudioSource.PlayClipAtPoint(fireballAudioClip,transform.position);
		initialPosition = this.transform.position;
		targetName = null;
	}


	// Update is called once per frame
	void FixedUpdate () {
		GameObject target_X = GameObject.FindGameObjectWithTag("Target");
		target= target_X;
		if(target == null || fireballRigidBody == null){
			return;
		}
		Vector3 targetPosition = target.transform.position + new Vector3(0,1.5f,0);
		fireBallVelocity = ((Vector3.Magnitude(targetPosition - initialPosition))/20f)*20f;
		fireballRigidBody.velocity = transform.forward * fireBallVelocity ;

		Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
		fireballRigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation,targetRotation,turn));
		if(Mathf.Abs(Vector3.Magnitude(this.transform.position - targetPosition)) <=1f ){
			Debug.Log("Reached The target");
			Destroy(this.gameObject);
		}
	}
}
