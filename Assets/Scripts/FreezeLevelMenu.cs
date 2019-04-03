using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeLevelMenu : MonoBehaviour {

    Vector3 FreezePosition;

	// Use this for initialization
	void Start () {
        FreezePosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = FreezePosition;
	}
}
