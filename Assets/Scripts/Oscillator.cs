using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour{

    //Config
    [SerializeField] Vector3 movementDirection = new Vector3(10f, 10f, 10f);
    [Range(-1,1)][SerializeField] float movementFactor = 0f;
    [SerializeField] float period = 2f;

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
        const float Tao = Mathf.PI * 2f;
        float cycle = Time.time / period;

        float rawSinWave = Mathf.Sin(Tao * cycle);
        movementFactor = rawSinWave;
        Vector3 offset = movementDirection * movementFactor;
        
        transform.position = startingPos + offset;
    }
}
