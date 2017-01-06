using UnityEngine;

public enum PlanetState {
	Alive,
	Leaving,
	Left,
	Exploding,
	Exploded
}

public class PlanetController : MonoBehaviour {
	public float mass = 0.015625f;

	[HideInInspector]
	public Vector2 acceleration = new Vector2();
	[HideInInspector]
	public Vector2 velocity = new Vector2();

	[HideInInspector]
	public PlanetState state = PlanetState.Alive;

	public void Leave () {
		if (state == PlanetState.Alive) {
			state = PlanetState.Leaving;

			// ToDo
		}
	}

	public void Explode () {
		if (state == PlanetState.Alive) {
			state = PlanetState.Exploding;

			// ToDo
		}
	}

	void FixedUpdate () {
		if (state == PlanetState.Alive) {
			velocity += acceleration * Time.fixedDeltaTime;

			Vector2 positionChange = velocity * Time.fixedDeltaTime;
			transform.position += new Vector3 (positionChange.x, 0, positionChange.y);
		} else if (state == PlanetState.Leaving) {
			// ToDo

			state = PlanetState.Left;
		} else if (state == PlanetState.Exploding) {
			// ToDo

			state = PlanetState.Exploded;
		}
	}
}
