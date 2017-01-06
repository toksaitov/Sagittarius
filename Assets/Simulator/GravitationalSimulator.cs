using System.Collections.Generic;

using UnityEngine;

public class GravitationalSimulator : MonoBehaviour {
	public float simulationSofteningLength = 100.0f;
	private float simulationSofteningLengthSquared;

	public Collider gameFieldCollider;

	private List<PlanetController> planets;
	private List<PlanetController> deadPlanets;

	public void AddPlanet(PlanetController planet) {
		planets.Add (planet);
	}

	void Start () {
		simulationSofteningLengthSquared = simulationSofteningLength * simulationSofteningLength;

		planets = new List<PlanetController> ();
		deadPlanets = new List<PlanetController> ();
	}

	void FixedUpdate () {
		Bounds gameFieldBounds = gameFieldCollider.bounds;
		foreach (PlanetController planet in planets) {
			if (planet.state != PlanetState.Alive) {
				continue;
			}

			Vector3 planetPosition = planet.transform.position;
			if (!gameFieldBounds.Contains(planetPosition)) {
				planet.Leave ();

				continue;
			}

			Bounds planetBounds = planet.GetComponentInChildren<Collider> ().bounds;

			Vector2 totalAcceleration = new Vector2 ();
			foreach (PlanetController anotherPlanet in planets) {
				if (planet == anotherPlanet || anotherPlanet.state != PlanetState.Alive) {
					continue;
				}

				Bounds anotherPlanetBounds = anotherPlanet.GetComponentInChildren<Collider> ().bounds;
				if (planetBounds.Intersects(anotherPlanetBounds)) {
					planet.Explode ();
					anotherPlanet.Explode ();

					break;
				}

				Vector2 acceleration = new Vector2(0, 0);

				Vector3 anotherPlanetPosition = anotherPlanet.transform.position;
				Vector3 universeR = anotherPlanetPosition - planetPosition;
				Vector2 galacticPlaneR = new Vector2(universeR.x, universeR.z);

				float distanceSquared = galacticPlaneR.sqrMagnitude + simulationSofteningLengthSquared;
				float distanceSquaredCubed = distanceSquared * distanceSquared * distanceSquared;
				float inverse = 1.0f / Mathf.Sqrt(distanceSquaredCubed);
				float scale = anotherPlanet.mass * inverse;

				acceleration += galacticPlaneR * scale;
				totalAcceleration += acceleration;
			}

			if (planet.state == PlanetState.Alive) {
				planet.acceleration = totalAcceleration;
			}
		}

		planets.RemoveAll (planet => {
			bool shouldRemove = planet.state != PlanetState.Alive;
			if (shouldRemove) {
				deadPlanets.Add(planet);
			}

			return shouldRemove;
		});

		deadPlanets.RemoveAll (deadPlanet => {
			bool shouldRemove = deadPlanet.state == PlanetState.Left || deadPlanet.state == PlanetState.Exploded;
			if (shouldRemove) {
				Destroy(deadPlanet.gameObject);
			}

			return shouldRemove;
		});
	}
}
