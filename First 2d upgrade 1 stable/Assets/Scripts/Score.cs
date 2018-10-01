using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Text scoreRecord;
    public GameMaster gm;
	
	// Update is called once per frame
	void Update () {
        scoreRecord.text = "\t " + (gm.enemyKillCount).ToString() + "\n\n\t " + (GameMaster.RemainingLives); ;
	}
}
