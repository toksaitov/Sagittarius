using UnityEngine;

public class PlayerController : MonoBehaviour {

	public PlanetController planetTemplate;
	public GravitationalSimulator simulatorInstance;

	public bool treatBoundsAsCircle = true;

	private float galacticPlaneY = 0.0f;

	void Start () {
		galacticPlaneY = planetTemplate.transform.position.y;
	}

	void Update () {
		const float MaximumRaycastDistance = 1000.0f;
		const string RayHitGameObjectTag = "GameField";

		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			RaycastHit[] hits = Physics.RaycastAll (ray, MaximumRaycastDistance);
			foreach (RaycastHit hit in hits) {
				Collider collider = hit.collider;
				Transform transform = hit.transform;

				if (collider.isTrigger || !transform.CompareTag (RayHitGameObjectTag)) {
					continue;
				}

				Vector3 position = hit.point;
				position.y = galacticPlaneY;

				Bounds bounds = collider.bounds;
				Vector3 center = bounds.center;
				center.y = galacticPlaneY;
				Vector3 extents = bounds.extents;
				float radiusSquared = Mathf.Max(extents.x, extents.z);
				radiusSquared *= radiusSquared;

				if ((position - center).sqrMagnitude < radiusSquared) {
					AddPlanet (position);

					break;
				}
			}
		}
	}

	private void AddPlanet (Vector3 position) {
		PlanetController planet = Instantiate<PlanetController> (planetTemplate, position, Quaternion.identity);
		simulatorInstance.AddPlanet (planet);
	}
}
