using UnityEngine;
using System.Collections;

public class TeamObject : MonoBehaviour {
	public int team;

	public static bool isSameTeam(GameObject obj1, GameObject obj2) {
		return obj1.GetComponent<TeamObject>().team ==
			obj2.GetComponent<TeamObject>().team;
	}

	// Use this for initialization
	void Start () {
		//this.tag = ""+team;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
