
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class Rocket : MonoBehaviour{

    //Config
    float mainThrust;
    float reactiveThrust;
    
    [SerializeField] AudioClip mainEngineSFX;
    [Range(0f,1f)][SerializeField] float mainEngineVolume;
    [Space(10)]
    [SerializeField] AudioClip deathSFX;
    [Range(0f, 1f)][SerializeField] float deathVolume;
    [Space(10)]
    [SerializeField] AudioClip winSFX;
    [Range(0f, 1f)][SerializeField] float winVolume;

    [Space(15)]
    [SerializeField] GameObject deathVFX;
    [SerializeField] ParticleSystem mainEngineVFX;
    [SerializeField] GameObject winVFX;
    //State

    //Cached component references
    Rigidbody rigidbody;
    AudioSource audioSource;
    int currentSceneIndex;
    SceneLoader sceneLoader;
    
    enum States {
        Alive,
        Dead,
        Transcend
    }

    States state = States.Alive;

    private void Awake(){
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        sceneLoader = FindObjectOfType<SceneLoader>();
    }
    
    private void Start(){
        mainThrust = 3500f;
        reactiveThrust = 350f;
        Debug.Log(mainThrust);
        mainEngineVolume = 1f;
        deathVolume = 1f;
        winVolume = 1f;
    }

    private void Update() {
        ProcessMovement();
    }

    private void OnCollisionEnter(Collision collision) {
        HandleCollisionWithTags(collision);
    }

    private void HandleCollisionWithTags(Collision collision) {
        if (!state.Equals(States.Alive)) {
            return;
        }

        switch (collision.gameObject.tag) {
            case "Friendly":
                Debug.Log("OK");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence() {
        state = States.Transcend;
        audioSource.Stop();
        audioSource.PlayOneShot(winSFX, winVolume);
        ProcessWinVFX();
        sceneLoader.LoadNextScene();
    }

    private void ProcessWinVFX() {
        mainEngineVFX.Stop();
        Instantiate(winVFX, gameObject.transform.position,Quaternion.identity);
    }

    private void StartDeathSequence() {
        state = States.Dead;
        ProcessDeathSFX();
        ProcessDeathVFX();
        sceneLoader.ReloadCurrentScene();
    }

    private void ProcessDeathSFX() {
        audioSource.Stop();
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVolume);
    }

    private void ProcessDeathVFX() {
        Instantiate(deathVFX, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void ProcessMovement() {

        if (state.Equals(States.Alive)) {
            HandleThrust();
            HandleRotation();
        }

    }
    private void HandleThrust() {
        if (Input.GetButton("Jump")) {
            rigidbody.AddRelativeForce(new Vector3(0f, mainThrust * Time.deltaTime, 0f));
            PlayEngineEffects();
        }
        else {
            audioSource.Stop();
            mainEngineVFX.Stop();
        }
    }

    private void PlayEngineEffects() {
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(mainEngineSFX, mainEngineVolume);
        }
        mainEngineVFX.Play();
        
    }

    private void HandleRotation() {
        rigidbody.freezeRotation = true;
        
        float rotationThisFrame = Input.GetAxis("Horizontal") * Time.deltaTime * reactiveThrust;
        transform.Rotate(new Vector3(0f, 0, rotationThisFrame));
        rigidbody.freezeRotation = false;
    }
}
