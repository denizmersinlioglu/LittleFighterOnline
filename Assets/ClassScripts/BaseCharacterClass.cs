using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterClass  {

	private string characterClassName;
	private string characterClassDescription;

	//Stats
	private float health;
	private List<GameObject> spawnObjects;

	public string CharacterClassName{
		get{return characterClassName;}
		set{characterClassName = value;}
	}

	public string CharacterClassDescription{
		get{return characterClassDescription;}
		set{characterClassDescription = value;}
	}

	public float Health{
		get{return health;}
		set{health = value;}
	}

	public List<GameObject> SpawnObjects{
		get{return spawnObjects;}
		set{spawnObjects = value;}
	}

}
