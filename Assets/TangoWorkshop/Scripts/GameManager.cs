using UnityEngine;
using Tango;

namespace TangoWorkshop
{
    [AddComponentMenu("Tango Workshop/Game Manager"), DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        // this is used twice: once in LateUpdate() to offset the marker from the reconstruction surface, and again in OnGUI to remove that offset when placing shapes
        private const float POS_OFFSET = 0.025f;

        [Tooltip("The prefab used to mark the location where shapes will be created.")]
        public GameObject markerPrefab;
        [Tooltip("Drag & drop shape prefabs here to use them in the game. If you make your own prefabs, make sure they have a Mesh Filter, Mesh Renderer, Collider, Rigid Body and Shape Controller attached.")]
        public GameObject[] buildingPrefabs;
		public GameObject[] characterPrefabs;
		public bool characterMenu = false;
		public bool buildingMenu = false;
		public bool placementMenu = true;
		public float lastUnitPlaceTime = 0;
		public bool placedTownCenter = false;
		//public int kills;

        private GameObject marker;

        void Start()
        {
            // make an instance of the marker prefab
            marker = Instantiate(markerPrefab);
        }

        void LateUpdate()
        {
            // place the marker by casting a ray from the center of the screen
            RaycastHit hit;
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            if (Physics.Raycast(Camera.main.ScreenPointToRay(screenCenter), out hit))
            {
                // make the marker active if the raycast was successful
				if (!placementMenu) {
					marker.SetActive (true);
					// set the position (with slight offset) and rotation of the marker
					marker.transform.position = hit.point + hit.normal * POS_OFFSET;
					marker.transform.LookAt (hit.point + hit.normal);
				}
            }
            else
            {
                // make the marker inactive if the raycast wasn't successful
                marker.SetActive(false);
            }
        }

		void prefabPlacementGUI(GameObject[] prefabs) {
			float height = Screen.height - 256;

			// create buttons for each prefab shape
			foreach (GameObject prefab in prefabs)
			{
				// create the "throw" button and the code for when it's pressed
				/*
				if (GUI.Button(new Rect(32f, height, 256f, 96f), "<size=30>Throw a:\n" + buildingPrefab.name + "</size>"))
				{
					// create the new shape at the position with a default rotation (Quaternion.identity)
					GameObject newShape = Instantiate(buildingPrefab, Camera.main.transform.position, Camera.main.transform.rotation) as GameObject;
					// set the new shape's velocity
					newShape.GetComponent<Rigidbody>().velocity = newShape.transform.forward * 3f;
				}
				*/
				//GUI.TextArea (new Rect (0, 0, 128f, 32f), "" + kills);

				// only create the "place" button if we have a place to put it (i.e. the marker object is active)
				if (marker.activeSelf) {
					// create the "place" button and the code for when it's pressed
					if (GUI.Button (new Rect (320f, height, 256f, 96f), "<size=30>Place a:\n" + prefab.name + "</size>")) {
						// this position logic assumes that the new shape has a height of 1 (meter)
						// marker.transform.forward is used as opposed to marker.transform.up because the
						// marker is a quad primitive and Unity's quad primitives have to be rotated to lay flat
						Vector3 position = marker.transform.position + marker.transform.forward * ((prefab.transform.localScale.y / 2f) - POS_OFFSET);
						position.x = Mathf.Round (position.x * 50) / 50.0f;
						position.y = Mathf.Round (position.y * 50) / 50.0f;
						position.z = Mathf.Round (position.z * 50) / 50.0f;
						// create the new shape at the position with a default rotation (Quaternion.identity)
						GameObject obj = (GameObject)Instantiate (prefab, position, prefab.transform.rotation);
						TeamObject to = obj.GetComponent<TeamObject> ();
						//obj.tag 
						to.team = 0;
						if (characterMenu) {
							lastUnitPlaceTime = Time.time;
							Character character = obj.GetComponent<Character> ();
							character.aggressiveness = 1;
							character.military = true;
							character.team = 0;
							obj.tag = character.team + "";
						}
						if (buildingMenu)
							placedTownCenter = true;
					}
				} else {
					GUI.Label (new Rect (320f, height, 256f, 96f), "No Scan");
				}

				// move position up for the next row of buttons
				height -= 128f;
			}
		}


		public void drawBuildingMenu(int i) {
			prefabPlacementGUI (buildingPrefabs);
		}

		public void buildPlayerMenu(int i) {
			prefabPlacementGUI (characterPrefabs);
		}

		public void drawStartMenu(int i) {
			float height = Screen.height - 256;
			if (GUI.Button(new Rect(320f, height, 256f, 96f), "<size=30>Buildings</size>"))
			{
				buildingMenu = true;
				placementMenu = false;
				characterMenu = false;
				//GUI.ModalWindow(0, new Rect(32f, 32f, Screen.width-64f, Screen.height-64f), drawBuildingMenu,"Building Menu");
				// this position logic assumes that the new shape has a height of 1 (meter)
				// marker.transform.forward is used as opposed to marker.transform.up because the
				// marker is a quad primitive and Unity's quad primitives have to be rotated to lay flat
				//Vector3 position = marker.transform.position + marker.transform.forward * ((buildingPrefab.transform.localScale.y / 2f) - POS_OFFSET);

				// create the new shape at the position with a default rotation (Quaternion.identity)
				//Instantiate(buildingPrefab, position, Quaternion.identity);
			}
			height -= 128f;
			if (GUI.Button(new Rect(320f, height, 256f, 96f), "<size=30>Warriors</size>"))
			{
				//GUI.ModalWindow(0, new Rect(32f, 32f, Screen.width-64f, Screen.height-64f), buildPlayerMenu,"Player Menu");
				buildingMenu = false;
				placementMenu = false;
				characterMenu = true;
				// this position logic assumes that the new shape has a height of 1 (meter)
				// marker.transform.forward is used as opposed to marker.transform.up because the
				// marker is a quad primitive and Unity's quad primitives have to be rotated to lay flat
				//Vector3 position = marker.transform.position + marker.transform.forward * ((buildingPrefab.transform.localScale.y / 2f) - POS_OFFSET);

				// create the new shape at the position with a default rotation (Quaternion.identity)
				//Instantiate(buildingPrefab, position, Quaternion.identity);
			}
		}

        void OnGUI()
        {
			if (GUI.Button (new Rect (Screen.width-128f, 0, 96f, 96f), "<size=20>Back</size>")) {
				if (buildingMenu) {
					buildingMenu = false;
					placementMenu = true;
				} else if (characterMenu) {
					characterMenu = false;
					placementMenu = true;
				}
			}
            // set some initial variables
            GUI.color = Color.white;
			//prefabPlacementGUI (buildingPrefabs);
			if (placementMenu) {
				drawStartMenu (0);
			} else if (buildingMenu && !placedTownCenter) {
				drawBuildingMenu (0);
			} else if (characterMenu) {
				if ( (Time.time - lastUnitPlaceTime) > 2.0f)
					buildPlayerMenu (0);
			}
			//GUI.ModalWindow(0, new Rect(32f, 32f, Screen.width-64f, Screen.height-64f), drawStartMenu,"hi");
        }
    }
}

