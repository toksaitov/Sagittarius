using UnityEngine;

public class PlayerController : MonoBehaviour {

	public GameObject planetTemplate;

	void Update () {
		const float MaximumRaycastDistance = 1000.0f;
		const string RayHitGameObjectTag = "GameField";

		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, MaximumRaycastDistance)) {
				if (hit.transform.CompareTag (RayHitGameObjectTag)) {
					Instantiate (planetTemplate, hit.point, Quaternion.identity);
				}
			}
		}
	}

}
