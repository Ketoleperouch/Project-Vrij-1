using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFix : MonoBehaviour {

    private FlameBehaviour flame;
    // Use this for initialization
    void Start () {
        flame = GetComponentInChildren<FlameBehaviour>();
        flame.vitality = 100;
    }
	
	// Update is called once per frame
	void Update () {
        if (flame.vitality <= 30)
        {
            flame.vitality = 100;
        }
        
	}
}
