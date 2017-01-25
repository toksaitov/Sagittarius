using System.Collections.Generic;
using System.IO;

using UnityEngine;

public class WireframeController : MonoBehaviour {
	[HideInInspector]
	public List<PlanetController> planets;

	private Texture2D bodyDataTexture;

	private int bodyDataTexturePropertyID;
	private int bodyCountProperyID;

	private Material material;

	void Start () {
		planets = new List<PlanetController> ();

		bodyDataTexturePropertyID = Shader.PropertyToID ("_BodyDataTex");
		bodyCountProperyID = Shader.PropertyToID ("_BodyCount");

		material = GetComponent<Renderer> ().material;

		RecreateBodyDataTexture ();
		UpdateBodyDataTextureData ();
	}

	void Update () {
		if (planets.Count > bodyDataTexture.width) {
			RecreateBodyDataTexture ();
		}

		UpdateBodyDataTextureData ();
	}

	private void RecreateBodyDataTexture () {
		int textureWidth = 2;
		bool mipmaps = false;

		int planetCount = planets.Count;
		while (textureWidth < planetCount) {
			textureWidth *= 2;
		}
		int textureHeight = textureWidth;
		bodyDataTexture = new Texture2D (textureWidth, textureHeight, TextureFormat.RGBAFloat, mipmaps);
		bodyDataTexture.name = "BodyDataTexture";
		bodyDataTexture.filterMode = FilterMode.Point;
		bodyDataTexture.wrapMode = TextureWrapMode.Clamp;
		bodyDataTexture.anisoLevel = 0;
		for (int y = 0; y < bodyDataTexture.height; ++y) {
			for (int x = 0; x < bodyDataTexture.width; ++x) {
				bodyDataTexture.SetPixel (x, y, Color.black);
			}
		}
		bodyDataTexture.Apply ();

		material.SetTexture (bodyDataTexturePropertyID, bodyDataTexture);
	}

	private void UpdateBodyDataTextureData () {
		int planetCount = planets.Count;
		if (planetCount > 0) {
			for (int y = 0; y < bodyDataTexture.height; ++y) {
				for (int x = 0; x < bodyDataTexture.width; ++x) {
					bodyDataTexture.SetPixel (x, y, Color.black);
				}
			}
			for (int x = 0, y = 0; x < planetCount; ++x) {
				PlanetController planet = planets [x];

				Vector3 planetPosition = planet.transform.localPosition;
				float planetMass = planet.mass;

				Color positionAsColor = new Color (planetPosition.x, planetPosition.y, planetPosition.z, planetMass);
				bodyDataTexture.SetPixel (x, y, positionAsColor);
			}
			bodyDataTexture.Apply ();

			material.SetInt (bodyCountProperyID, planetCount);
		}
	}
}
