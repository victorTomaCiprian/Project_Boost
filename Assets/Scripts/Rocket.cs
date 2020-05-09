using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour{

    //Config
    [SerializeField] float thrustSpeed;
    [SerializeField] float rotationDegree;
    //State

    //Cached component references
    Rigidbody rigidbody;
    AudioSource audioSource;
    


    private void Awake(){
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    
    private void Start(){

    }

    private void Update(){
        HandleThrust();
        HandleRotation();
    }
   
    private void HandleThrust() {
        if (Input.GetButton("Jump")) {
            rigidbody.AddRelativeForce(new Vector3(0f, thrustSpeed, 0f));
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
        float zRotation = Input.GetAxis("Horizontal") * Time.deltaTime * rotationDegree;
        transform.Rotate(new Vector3(0f, 0, zRotation));
    }
}
