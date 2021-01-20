using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {
    public Transform[] backgrounds;//parallax these objects
    private float[] parScales; //amount of parallaxing
    public float smoother=1f; //>0 will make it smoother

    private Transform cam; //Main camera
    private Vector3 PrCamPos; //Previous Camera position—position of camera in last frame

    void Awake()//called before Start(), after game object setup. Great for assigning references 
    {
        // set up camera references
        cam = Camera.main.transform;
    }

	// Use this for initialization
	void Start () {
        //Previous frame had the current frame's camera position
        PrCamPos = cam.position;

        parScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length;i++)
        {
            //assigning parallaxing to each backgrounds[]
            parScales[i] = backgrounds[i].position.z * -1;

        }
    }
	
	// Update is called once per frame
	void Update () {
		//for each background
        for(int i = 0; i < backgrounds.Length; i++)
        {
            //parallax is the opposite of the camera movement because the previous frame was multiplied by the scales
            float parallax = (PrCamPos.x - cam.position.x) * parScales[i];
            float parallay = (PrCamPos.y - cam.position.y) * parScales[i];

            //set the parallax to the background as a target position for x axis
            float bgTargetX = backgrounds[i].position.x + parallax;
            float bgTargetY = backgrounds[i].position.y + parallay;

            //then create a vector3 to store the absolute position for the target
            Vector3 bgTarget = new Vector3(bgTargetX,bgTargetY, backgrounds[i].position.z);
            //finally, smoothly transition using  the Lerp method
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, bgTarget,smoother *Time.deltaTime);
        }
        PrCamPos = cam.position;
	}
}
