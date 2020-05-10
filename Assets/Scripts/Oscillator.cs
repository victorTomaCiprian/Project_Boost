using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour{

    //Config
    [SerializeField] Vector3 movementDirection;
    [Range(-1,1)][SerializeField] float movementFactor = 0f;

    Vector3 startingPos;
    //State

    //Cached component references

    private void Awake(){

    }
    
    private void Start(){
        startingPos = transform.position;
    }

    private void Update(){
        Move();
    }

    private void Move() {
        Vector3 offset = movementDirection * movementFactor;
        transform.position = startingPos + offset;
    }
}
