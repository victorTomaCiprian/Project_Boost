using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour{

    //Config
    float mainThrust;
    float reactiveThrust;
    //State

    //Cached component references
    Rigidbody rigidbody;
    AudioSource audioSource;
    


    private void Awake(){
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    
    private void Start(){
        mainThrust = 10f;
        reactiveThrust = 200f;
    }

    private void Update(){
        HandleThrust();
        HandleRotation();
    }
   
    private void HandleThrust() {
        if (Input.GetButton("Jump")) {
            rigidbody.AddRelativeForce(new Vector3(0f, mainThrust, 0f));
            PlayEngineSFX();
        }
        else {
            audioSource.Stop();
        }
    }

    private void PlayEngineSFX() {
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }

    private void HandleRotation() {
        rigidbody.freezeRotation = true;

        float rotationThisFrame = Input.GetAxis("Horizontal") * Time.deltaTime * reactiveThrust;
        transform.Rotate(new Vector3(0f, 0, rotationThisFrame));

        rigidbody.freezeRotation = false;
    }
}
