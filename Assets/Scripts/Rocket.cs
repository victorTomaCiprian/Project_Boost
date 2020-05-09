
using UnityEngine;

public class Rocket : MonoBehaviour{

    //Config
    [SerializeField] float mainThrust = 10f;
    [SerializeField] float reactiveThrust = 200f;

    //State

    //Cached component references
    Rigidbody rigidbody;
    AudioSource audioSource;
    


    private void Awake(){
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    
    private void Start(){
        //reactiveThrust = 200f;
    }

    private void Update(){
        HandleThrust();
        HandleRotation();
    }

    private void OnCollisionEnter(Collision collision) {
        HandleCollisionWithTags(collision);
    }

    private void HandleCollisionWithTags(Collision collision) {
        switch (collision.gameObject.tag) {
            case "Friendly":
                Debug.Log("OK");
                break;
            default:
                Debug.Log("Dead");
                break;
        }
    }

    private void HandleThrust() {
        
        if (Input.GetButton("Jump") ) {
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
