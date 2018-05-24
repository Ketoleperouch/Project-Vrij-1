using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

	AudioSource audio;
	void Start()
	{
		audio  = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other)
	{
		if (!audio.isPlaying) {
		if(other.CompareTag("Player"))
		{
			audio.Play();
			audio.Play(44100);
			print ("Istriggerd");
			}
		}
	}
}
