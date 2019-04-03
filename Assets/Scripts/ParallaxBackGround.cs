using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour {

    public float maxDisplacement = 30f;
    public float displacement = 0.01f;
    private float initPos;
	
    void Start()
    {
        initPos = transform.position.x;
    }
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x + displacement, transform.position.y, transform.position.z);
        if (Mathf.Abs(transform.position.x - initPos) > maxDisplacement)
        {
            transform.position = new Vector3(initPos, transform.position.y, transform.position.z);
        }
    }
}
