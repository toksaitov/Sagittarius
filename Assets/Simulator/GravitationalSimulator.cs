using System.Collections.Generic;

using UnityEngine;

public class GravitationalSimulator : MonoBehaviour {
	public float simulationSofteningLength = 100.0f;
	private float simulationSofteningLengthSquared;

	private List<PlanetController> planets;

	public void AddPlanet(PlanetController planet) {
		planets.Add (planet);
	}

	void Start () {
		simulationSofteningLengthSquared = simulationSofteningLength * simulationSofteningLength;

		planets = new List<PlanetController> ();
	}

	void FixedUpdate () {
		foreach (PlanetController planet in planets) {
			if (planet.state != PlanetState.Alive) {
				continue;
			}

			Vector2 totalAcceleration = new Vector2 ();
			foreach (PlanetController anotherPlanet in planets) {
				if (planet == anotherPlanet || anotherPlanet.state != PlanetState.Alive) {
					continue;
				}

				Vector2 acceleration = new Vector2(0, 0);

				Vector3 universeR = anotherPlanet.transform.position - planet.transform.position;
				Vector2 galacticPlaneR = new Vector2(universeR.x, universeR.z);

				float distanceSquared = galacticPlaneR.sqrMagnitude + simulationSofteningLengthSquared;
				float distanceSquaredCubed = distanceSquared * distanceSquared * distanceSquared;
				float inverse = 1.0f / Mathf.Sqrt(distanceSquaredCubed);
				float scale = anotherPlanet.mass * inverse;

				acceleration += galacticPlaneR * scale;
				totalAcceleration += acceleration;
			}

			planet.acceleration = totalAcceleration;
		}

		planets.RemoveAll (planet => planet.state != PlanetState.Alive);
	}
}
