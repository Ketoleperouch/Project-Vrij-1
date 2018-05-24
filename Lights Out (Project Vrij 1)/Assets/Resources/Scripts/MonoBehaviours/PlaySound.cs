using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

	AudioSource source;
	void Start()
	{
        source = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other)
	{
        if (!source.isPlaying)
        {
            if (other.CompareTag("Player"))
            {
                source.Play();
                source.Play(44100);
            }
        }
	}
}
