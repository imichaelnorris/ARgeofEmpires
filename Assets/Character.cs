using UnityEngine;
using UnityEngine.UI;
using TangoWorkshop;

using System.Collections;

public class Character : MonoBehaviour {
	private Character target;
	public float attackRadius = 0.1f;
	public float losRadius = 100.0f;
	public int hp = 100;
	public int attackPower = 25;
	public float speed = 100.0f;
	public bool military = true;
	public int team;

	//public Slider healthSlider;
	// Use this for initialization

	/**
	 * int between 0-3. 0 attacks anything and 3 is friendly
	 */
	public int aggressiveness = 0;

	float barDisplay = 0;
	//Vector2 pos = new Vector2(20,40);
	Vector2 size = new Vector2(60,20);
	public Texture2D progressBarEmpty;// : Texture2D;
	public Texture2D progressBarFull;// : Texture2D;

	void Start () {
		
		this.tag = ""+team;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -10.0f || hp <= 0)
			Destroy (this);
		
		if (target == null || target.hp <= 0) {
			int team = this.GetComponent<TeamObject> ().team;
			//target = FindClosestEnemy (GameObject.FindGameObjectsWithTag ("" + (team + 1) % 2));
			target = FindClosestEnemy(GameObject.FindObjectsOfType<Character>());
			//target = target.transform.parent.gameObject;
			//this.transform.parent.gameObject.transform.LookAt (target.transform.position);
			Vector3 transformLook = new Vector3(target.transform.position.x, 0, target.transform.position.z);
			
			this.transform.LookAt(transformLook);
		}
		if (target == null && !this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle_Spear")) {
			//GetComponent<Animation>().Play ("Idle_Spear");
			GetComponent<Animator>().Play("Idle_Spear");
			return;
		}
		// go towards enemy
		// if inside RADIUS, then attack
		// if target is null, then be idle 
		if (aggressiveness == 0 ||
		    Vector3.Distance (transform.position, target.transform.position) < losRadius) {
			if (Vector3.Distance (transform.position, target.transform.position) > attackRadius) {
				//GetComponent<Animator>().runtimeAnimatorController.
				Vector3 positionTo = target.transform.position;
				positionTo = new Vector3 (positionTo.x, 0, positionTo.z);
				this.transform.parent.transform.position = Vector3.MoveTowards (transform.position, positionTo, 
					speed * Time.deltaTime);
				//this.transform.LookAt(target.transform);
				if (!GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Run_Spear")) {
					GetComponent<Animator> ().Play ("Run_Spear");
					//GetComponent<Animator>().
				}
			} else {
				if (!GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Atack_Spear")) {
					GetComponent<Animator> ().Play ("Atack_Spear");
					//Character targetCharacter = target.GetComponent<Character> ();
					target.hp -= attackPower;

					//targetCharacter.animation.Play ("GetDamage_Spear");
					//if (target.hp <= 0) {
					Destroy (target.transform.parent.gameObject);
					target = null;
					//((GameManager)GameObject.Find ("GameManager").GetComponent<GameManager>()).kills += 1;
					//}
				}
			}
		}

	}

	Character FindClosestEnemy(Character[] gos) {
		Character closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (Character go in gos) {
			if (go.team == this.team)
				continue;
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		return closest;

	}
}
