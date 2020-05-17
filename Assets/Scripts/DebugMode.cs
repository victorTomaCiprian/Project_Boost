using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMode : MonoBehaviour{

    //Config
    KeyCode nextLevelKey;
    KeyCode disableCollisionKey;

    //Cached component references
    SceneLoader sceneLoader;

    bool isCollisionEnabled = true;
    public bool IsCollisionEnabled {
        get { return isCollisionEnabled; }
    }

    private void Awake(){
        sceneLoader = FindObjectOfType<SceneLoader>();
    }
    
    private void Start(){
        nextLevelKey = KeyCode.L;
        disableCollisionKey = KeyCode.C;
    }

    private void Update(){
        if (Debug.isDebugBuild) {
            RespondToDebugKeys();
        }
        
    }

    private void RespondToDebugKeys() {
        LoadNextLevelOnKeyPress();
        DisableCollisionDetection();
    }

    private void LoadNextLevelOnKeyPress() {
        if (Input.GetKeyDown(nextLevelKey)) {
            sceneLoader.LoadNextScene();
        }
    }

    private void DisableCollisionDetection() {
        if (Input.GetKeyDown(disableCollisionKey) && isCollisionEnabled) {
            isCollisionEnabled = false;
        }
        else if (Input.GetKeyDown(disableCollisionKey)) {
            isCollisionEnabled = true;
        }
        
    }
}
