using UnityEngine;

public class PlayerController : MonoBehaviour {

	public PlanetController planetTemplate;
	public GravitationalSimulator simulatorInstance;

	void Update () {
		const float MaximumRaycastDistance = 1000.0f;
		const string RayHitGameObjectTag = "GameField";

		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, MaximumRaycastDistance)) {
				if (hit.transform.CompareTag (RayHitGameObjectTag)) {
					PlanetController planet = Instantiate<PlanetController> (planetTemplate, hit.point, Quaternion.identity);
					simulatorInstance.AddPlanet (planet);
				}
			}
		}
	}

}
