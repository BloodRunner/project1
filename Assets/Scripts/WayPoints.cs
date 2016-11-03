using UnityEngine;
using System.Collections;

public class WayPoints : MonoBehaviour
{
	const int N_ORGANS = 8; // total number of organs
	// 3 alternative paths
	// Update to Left and Right Lungs and Kidneys when we have them
	string[,] organPaths = {{ "Heart", "Liver", "Brain", "Lungs", "Thymus", "Stomach","KidneyL","KidneyR"},
		{ "Thymus", "Lungs", "Stomach","KidneyL", "Liver", "KidneyR", "Heart", "Brain" },
		{ "KidneyL", "Stomach","KidneyR", "Heart", "Brain", "Lungs", "Thymus", "Liver"}
	};
	Vector3[,] waypoints = new Vector3[N_ORGANS,3];
	// Use this for initialization
	void Awake ()
	{
		makeWayPoints ();
	}

	public float getOrgansScores() {
		float total = 0;
		//	GameObject obj = GameObject.Find(name);
		OrganController[] organs = GameObject.FindObjectsOfType (typeof(OrganController)) as OrganController[];
		foreach (OrganController organ in organs) {
			total += organ.get_stats_health ();
		}
		Debug.Log ("Score =" + total);
		return total;
	}
	private void makeWayPoints() {
		int k;
		for (int i = 0; i < 3; i++) {
			k = 0;
			for (int j = 0; j < N_ORGANS; j++) {
				GameObject organ = GameObject.Find (organPaths[i,j]);
				if (organ) { // skip dead organs or non existent ones
					waypoints [i, k] = organ.transform.position;
					k++;
				}
			}
		}
	}
}

