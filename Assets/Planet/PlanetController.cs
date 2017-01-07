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
	public PlanetState state = PlanetState.Alive;

	private Rigidbody rigidBody;

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

	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		if (state == PlanetState.Alive) {
			Vector3 force = new Vector3 (acceleration.x, 0.0f, acceleration.y);
			rigidBody.AddForce(force);
		} else if (state == PlanetState.Leaving) {
			// ToDo

			state = PlanetState.Left;
			Destroy (this.gameObject);
		} else if (state == PlanetState.Exploding) {
			// ToDo

			state = PlanetState.Exploded;
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter (Collision collision) {
		const string CollisionGameObjectTag = "Planet";

		if (collision.gameObject.CompareTag (CollisionGameObjectTag)) {
			Explode ();
		}
	}

	void OnTriggerExit (Collider other) {
		const string TriggerGameObjectTag = "GameField";

		if (other.CompareTag (TriggerGameObjectTag)) {
			Leave ();
		}
	}
}
