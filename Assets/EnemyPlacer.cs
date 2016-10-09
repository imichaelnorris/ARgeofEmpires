using UnityEngine;
using System.Collections;
using TangoWorkshop;

public class EnemyPlacer : MonoBehaviour {
	public float initializeTime = 0.0f;

	// Use this for initialization
	void Start () {
		initializeTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (/*(Time.time - initializeTime > 10.0f) &&*/ Random.value > .90) {
			debugPlaceRandomObject ();
		}
	}

	void debugPlaceRandomObject() {
		//Vector3 pos = new Vector3 (1.0f, 0, 0);
		float x = Random.value;
		float y = Mathf.Sqrt (1 - x * x);
		Vector3 height = new Vector3 (0.0f, 5.0f, 0.0f);
		Vector3 pos = new Vector3 (x, 0, y);
		placeRandomObject (height + 2*pos);
	}

	void placeRandomObject(Vector3 position) {
		GameManager manager = GameObject.FindObjectsOfType<GameManager> ()[0];
		GameObject[] prefabs = manager.characterPrefabs;
		GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

		placeObject (position, prefab);
	}

	void placeObject(Vector3 position, GameObject prefab) {
		GameObject obj = (GameObject)Instantiate (prefab, position, prefab.transform.rotation);
		obj.transform.position.Scale (new Vector3 (1.5f, 1.5f, 1.5f));
		Character character = (Character)obj.GetComponent<Character>();
		character.team = 1;
		obj.tag = character.team + "";
		obj.GetComponent<TeamObject> ();
		TeamObject to = obj.GetComponent<TeamObject> ();
		to.team = 0;
	}
}
