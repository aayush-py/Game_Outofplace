using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movetrail : MonoBehaviour {

    public int moveSpeed = 50;
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        Destroy(this.gameObject,1);
	}
}
