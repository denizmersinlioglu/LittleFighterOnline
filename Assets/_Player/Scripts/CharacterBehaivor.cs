using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CharacterBehaivor : NetworkBehaviour {
	
	public bool isWalking = false;
	public bool isRunning = false;

	public float speed = 2.0f;
	private float upSpeed;
	Vector3 movement;
	Rigidbody playerRigidBody;

	Vector3 cameraOffset;
    Animator anim;
	private GameObject joyStickObj;
	private Joystick joyStick;
	private float moveMagnitude;
	[SyncVar] float moveMagnitude_2;

	// Use this for initialization

	void Start () {
		cameraOffset = new Vector3(0,10,-30);
		anim = GetComponent<Animator>();
		joyStickObj = GameObject.FindGameObjectWithTag("JoyStick") ;
		joyStick = joyStickObj.GetComponent<Joystick>();

	
	}

	// Update is called once per frame
	void Update () {

	   CameraBehaivor();
	   moveMagnitude = Mathf.Abs(joyStick.magnitude / 150f);
	
	}


	void Awake() {
		playerRigidBody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate (){
		
		if( isLocalPlayer){
		if(GetComponent<PlayerHealth>().currentHealth <= 0){
		anim.SetBool("isDead",true);
		return;
		}
		anim.SetBool("isDead",false);
		// we need to get a hold of which keys (input) has been used by the Player
		float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		float v = CrossPlatformInputManager.GetAxisRaw ("Vertical");

		upSpeed = speed * moveMagnitude;
		Move (v, -h);
		Turning ();
	}



	}
	void ControlTheAnimation(){
		if (moveMagnitude <= 0.4f && moveMagnitude >= 0.05f) {
			isWalking =true;
			isRunning =false;
			anim.SetBool("isWalking",true);
			anim.SetBool("isRunning",false);

		} else if(moveMagnitude > 0.4f && moveMagnitude <= 1){
			isWalking =false;
			isRunning =true;
			anim.SetBool("isWalking",false);
			anim.SetBool("isRunning",true);
		}else{
			anim.SetBool("isWalking",false);
			anim.SetBool("isRunning",false);
		}
	}

	void Move( float h, float v){
		movement.Set (-v, 0f, h);
		movement = movement.normalized * Time.fixedDeltaTime * upSpeed;
		//transform.position = transform.position + movement;
		ControlTheAnimation();
		playerRigidBody.MovePosition (transform.position + movement);
	}



	public void Turning(){
		// 1) get to know where the mouse is located at
		// if it is in range, then rotate the character towards the mouse

		#if !MOBILE_INPUT
		this.transform.rotation = Quaternion.Euler(Vector3.zero);
		#else
		Vector3 turnDir = new Vector3(CrossPlatformInputManager.GetAxisRaw("Horizontal"), 0f, CrossPlatformInputManager.GetAxisRaw("Vertical"));
		if(turnDir != Vector3.zero){
			Vector3 playerToMouse = (transform.position + turnDir) - transform.position;
			playerToMouse.y = 0;
			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			//	transform.rotation = newRotation;
			playerRigidBody.MoveRotation(newRotation);
		}
		#endif
	}

	void CameraBehaivor()
	{
		if(isLocalPlayer){
		Vector3 cameraTarget = this.transform.position + cameraOffset;
		Camera.main.transform.LookAt(transform.position);
		Camera.main.transform.position = Vector3.Lerp(	Camera.main.transform.position , cameraTarget, Time.deltaTime);
		}
	}


}