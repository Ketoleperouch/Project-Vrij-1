using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Sanity : MonoBehaviour {

	public PostProcessingProfile profile;
	public float Startintensity = 0f;
	public float Newintensity = 1f;
	static float t = 0.0f;
	public bool stuff;


	void Start()
	{
		stuff = false;
		profile.vignette.enabled = true;
		Begin ();

	}

	void Update()
	{
		if (stuff == true && t <=1f) {
			t += 0.5f * Time.deltaTime;

			VignetteModel.Settings vignetteSettings = profile.vignette.settings;
			vignetteSettings.intensity = Mathf.Lerp (0.0f, 1.0f, t);
			profile.vignette.settings = vignetteSettings;
		} 
			
		if (stuff == false && t >=0f) {
			t -= 0.5f * Time.deltaTime;
			VignetteModel.Settings vignetteSettings = profile.vignette.settings;
			vignetteSettings.intensity = Mathf.Lerp (0f, 1f, t);
			profile.vignette.settings = vignetteSettings;
		} 

	}/// <summary>
	/// End of the update
	/// </summary>

	void Begin()
	{
		VignetteModel.Settings vignetteSettings = profile.vignette.settings;
		vignetteSettings.intensity = Startintensity;
		profile.vignette.settings = vignetteSettings;
	}

	void OnTriggerEnter(Collider other)
	{
			if (other.tag == "enemy") {
				stuff = true;
				return;
			}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "enemy") 
		{
			stuff = false;
		}
	}

	void Mcguffin()
	{
		stuff = false;
	}
}
