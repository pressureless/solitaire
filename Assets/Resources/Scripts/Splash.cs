using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("SwitchToGameScene", 3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SwitchToGameScene()
    { 
        SceneManager.LoadScene("game");
    }
}
