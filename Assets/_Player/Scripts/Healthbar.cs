using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour {

public GameObject Player;
private float maxHealth;

public Color minColor = Color.red;
public Color maxColor = Color.green;

public float initialLength =1f;
public float currentLength = 1f;

private SpriteRenderer  rend;
	// Use this for initialization
	void Start () {
		Player = this.transform.parent.gameObject;
		maxHealth = Player.GetComponent<PlayerHealth>().startingHealth;
		rend = GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {

		float fraction = (float) Player.GetComponent<PlayerHealth>().currentHealth / maxHealth;
		rend.color = Color.Lerp(minColor,maxColor, Mathf.Lerp(0,1,fraction));

		transform.localScale = new Vector3 (initialLength* fraction,transform.localScale.y,transform.localScale.z);

		transform.LookAt(Camera.main.transform);
	}
}
