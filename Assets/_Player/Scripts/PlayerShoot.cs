using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";


	public GameObject TargetIndicatorPrefab;
	public float ShootingAngle = 30f;

	[SerializeField]
	public GameObject spawnPoint;

	[SerializeField]
	private LayerMask mask;

	private bool isBlocking;
	private PlayerWeapon currentWeapon;
	public GameObject Target;


	void Start ()
	{
		InitializeButtons();
		if (spawnPoint == null)
		{
			Debug.LogError("PlayerShoot: No spawnPointera referenced!");
			this.enabled = false;
		}
		mask = -1;

	}

	private bool isTargeted= false;

	void Update ()
	{
		if(! isLocalPlayer)
	{
	return;
	}
			TargetPlayer();
			if(Target!= null && Target.GetComponent<PlayerHealth>().currentHealth >0){
				TargetEnemy();
				//Debug.Log("TargetName : " + Target.GetComponent<PlayerID>().playerUniqueName);	
			}else if(Target!= null && Target.GetComponent<PlayerHealth>().currentHealth <=0)
			{
			UnTargetEnemy();
			}
			if(Target!= null && Target.GetComponent<PlayerHealth>().currentHealth >0){
			float distance = Vector3.Magnitude(this.transform.position - Target.transform.position);
			if(distance >=20f)
			{
				UnTargetEnemy();
				Target=null;
			}
			}
	}

	void TargetPlayer(){

		RaycastHit _hit;
		if (Physics.Raycast(spawnPoint.transform.position, spawnPoint.transform.forward, out _hit, 20f, mask) )
		{
			
			if (_hit.collider.tag == PLAYER_TAG )
			{	
				Target = _hit.collider.gameObject;
				if( Target.GetComponent<PlayerHealth>().currentHealth <= 0){
					return;
				}
					
			}
			if(Target != null){
			CheckBlockingObjects();
			}
	}
}

	private void CheckBlockingObjects(){
		
		RaycastHit _BlockingObject;
		Vector3 directionVector = Target.transform.position - this.transform.position;
		if(Physics.Raycast(this.transform.position, directionVector,out _BlockingObject,20f)){
			if(_BlockingObject.collider.tag != PLAYER_TAG){
				isBlocking = true;
			}else{
				isBlocking =false;
			}
		}
	}

	private GameObject targetIndicator;

	[Client]
	void TargetEnemy()
	{
		if(! isTargeted)
		{
			Debug.Log("Starting Target Indicator");
			targetIndicator = GameObject.Instantiate(TargetIndicatorPrefab,Target.transform.position,Quaternion.identity) ;
			targetIndicator.transform.SetParent(Target.transform);
			isTargeted = true;	
		}
	}

	[Client]
	void UnTargetEnemy()
	{	
		if(isTargeted){
		Destroy(targetIndicator);
			if(Target.GetComponent<PlayerHealth>().currentHealth <=0){
			this.GetComponent<PlayerKills>().KillCount++;}
		isTargeted = false;
		}
	}

	//Is called on the server when a player shoots
	[Command]
	void CmdOnShoot ()
	{
		RpcDoShootEffect();
    }

	//Is called on all clients when we need to to
	// a shoot effect
	[ClientRpc]
	void RpcDoShootEffect ()
	{
		//Skill effect
	}

	//Is called on the server when we hit something
	//Takes in the hit point and the normal of the surface
	[Command]
	void CmdOnHit (Vector3 _pos, Vector3 _normal)
	{
		RpcDoHitEffect(_pos, _normal);
    }

	//Is called on all clients
	//Here we can spawn in cool effects
	[ClientRpc]
	void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
	{
		// The hit effect
		//GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
		//Destroy(_hitEffect, 2f);
	}
	private float damage = 10f;
	void GiveDamage(){
			CmdPlayerShot(Target.transform.name,damage);
		}

	[Client]
	void Shoot ()
	{
		if (!isLocalPlayer) // \\ not enough mana
		{
			return;
		}
		if(Target!= null && this.GetComponent<PlayerHealth>().currentHealth >0){
		Vector3 distanceVector = Target.transform.position - this.transform.position;
		float angle = Vector3.Angle(this.transform.forward,distanceVector);
			if(Mathf.Abs(angle )<= ShootingAngle){
				Debug.Log("I am in the shoot");
				Invoke("GiveDamage",0.8f);
				if(skill_number ==1){
				this.GetComponent<MageSkill>().CastFireBall(spawnPoint.transform);
				}
				if(skill_number ==2){
					this.GetComponent<MageSkill>().CastFrostBall(spawnPoint.transform);
				}
			}else{
				Debug.Log("Aiming Wrong Direction");
			}
		}



		//currentWeapon.bullets--;  mana = -- mana

		//We are shooting, call the OnShoot method on the server
//		CmdOnShoot();
//
//
//		RaycastHit _hit;
//		if (Physics.Raycast(spawnPoint.transform.position, spawnPoint.transform.forward, out _hit, 20f, mask) )
//		{
//			Debug.Log(_hit.collider.gameObject.transform.name);
//			if (_hit.collider.tag == PLAYER_TAG)
//			{
//				CmdPlayerShot(_hit.collider.name, 20f);
//			}
//
//			// We hit something, call the OnHit method on the server
//			CmdOnHit(_hit.point, _hit.normal);
//		}


	}

	[Command]
	void CmdPlayerShot (string _playerID, float _damage)
	{	
		
		Debug.Log(_playerID + " has been shot.");

        PlayerHealth _player = GameObject.Find(_playerID).GetComponent<PlayerHealth>();
        _player.RpcTakeDamage(_damage);
	}




#region SkillRegion
	private Button Skill_0;
	private Button Skill_1;
	private Button Skill_2;
	private Button Skill_3;
	private Button Skill_4;

	public int skill_number;
public void Cast_Skill_0(){

		if(!isBlocking){
		damage = 15f;
		skill_number= 1;
		Invoke("Shoot",0.1f);
			
		}
		else{
			Debug.Log("There is blocking object");
		}
}
public void Cast_Skill_1(){
		if(!isBlocking){
		damage = 20f;
		skill_number= 2;
		Invoke("Shoot",0.1f);
		}else{
			Debug.Log("There is blocking object");
		}
}
public void Cast_Skill_2(){
		if(!isBlocking){
		damage = 30f;
		Invoke("Shoot",0.8f);
		}else{
			Debug.Log("There is blocking object");
		}
}
public void Cast_Skill_3(){
		if(!isBlocking){
		damage = 40f;
		Invoke("Shoot",1f);
		}else{
			Debug.Log("There is blocking object");
		}
}
public void Cast_Skill_4(){
		if(!isBlocking){
		damage = 50f;
		Invoke("Shoot",1.2f);
		}else{
			Debug.Log("There is blocking object");
		}
}

void InitializeButtons(){
		Skill_0= GameObject.Find("Skill_0").GetComponent<Button>();
		Skill_0.onClick.AddListener(() => Cast_Skill_0());

		Skill_1= GameObject.Find("Skill_1").GetComponent<Button>();
		Skill_1.onClick.AddListener(() => Cast_Skill_1());

		Skill_2= GameObject.Find("Skill_2").GetComponent<Button>();
		Skill_2.onClick.AddListener(() => Cast_Skill_2());

		Skill_3= GameObject.Find("Skill_3").GetComponent<Button>();
		Skill_3.onClick.AddListener(() => Cast_Skill_3());

		Skill_4= GameObject.Find("Skill_4").GetComponent<Button>();
		Skill_4.onClick.AddListener(() => Cast_Skill_4());
}
#endregion
}
