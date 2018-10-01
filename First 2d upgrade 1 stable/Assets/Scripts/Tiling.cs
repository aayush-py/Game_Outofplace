using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

    public int offset = 2;//camera and endofsprite offset
    //check for buddies left and right
    public bool hasABudL = false;
    public bool hasABudR = false;
    public Transform parens;

    public bool revScale = false;//for the nontiled objects

    private float spriteWidth = 0f;
    private Camera cam;
    private Transform myTransform;
    
    void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }
	// Use this for initialization
	void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;

	}
	
	// Update is called once per frame
	void Update () {
        if (!(hasABudL && hasABudR))
        {
            float camHorizon = cam.orthographicSize * Screen.width / Screen.height;//sees the half width of camera vision
            //calculate the x position where the camera can see the edge of Sprite
            float edgeRight = (myTransform.position.x + spriteWidth / 2) - camHorizon;
            float edgeLeft = (myTransform.position.x - spriteWidth / 2) + camHorizon;
            //check for edge of sprite that camera sees and make a buddy
            if (cam.transform.position.x>=edgeRight - offset && !hasABudR)
            {
                MakeBuddy(1);
                hasABudR = true;
            }
            else if(cam.transform.position.x <= edgeLeft + offset && !hasABudL)
            {
                MakeBuddy(-1);
                hasABudL = true;
            }
        }
    }
    void MakeBuddy(int RorL) //buddy maker
    {
        if (revScale)
        {
            Vector3 newPos = new Vector3(myTransform.position.x + spriteWidth * RorL, myTransform.position.y, myTransform.position.z);//new Buddy position
            //here is the infinity boi/instantiator
            Transform newBuddy = (Transform)Instantiate(myTransform, newPos, myTransform.rotation);
            //for non tilables, invert the x
            // newBuddy.transform.localScale = new Vector3(newBuddy.transform.localScale.x * -1, newBuddy.transform.localScale.y, newBuddy.transform.localScale.z);
            Vector3 theScale = newBuddy.transform.localScale;
            theScale.x *= -1;
            newBuddy.transform.localScale = theScale;
            newBuddy.parent = parens;
            if (RorL > 0)
            {
                newBuddy.GetComponent<Tiling>().hasABudL = true;
            }
            if (RorL < 0)
            {
                newBuddy.GetComponent<Tiling>().hasABudR = true;
            }
        }
        else
        {
            Vector3 newPos = new Vector3(myTransform.position.x + spriteWidth * RorL, myTransform.position.y, myTransform.position.z);//new Buddy position
            //here is the infinity boi/instantiator
            Transform newBuddy = (Transform)Instantiate(myTransform, newPos, myTransform.rotation);
            //for non tilables, invert the x

            newBuddy.transform.parent = parens.transform;
            if (RorL > 0)
            {
                newBuddy.GetComponent<Tiling>().hasABudL = true;
            }
            if (RorL < 0)
            {
                newBuddy.GetComponent<Tiling>().hasABudR = true;
            }
        }
    }
}
