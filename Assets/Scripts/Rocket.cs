
using System;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //State
    float waitTimeBetweenLevels;
    float waitTimeBetweenDeaths;

    //Cached component references
    Rigidbody rigidbody;
    AudioSource audioSource;
    int currentSceneIndex;
    
    enum States {
        Alive,
        Dead,
        Transcend
    }

    States state = States.Alive;

    private void Awake(){
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    
    private void Start(){
        mainThrust = 10f;
        reactiveThrust = 250f;

        mainEngineVolume = 1f;
        deathVolume = 1f;
        winVolume = 1f;

        waitTimeBetweenLevels = 2f;
        waitTimeBetweenDeaths = 1f;
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
        StartCoroutine(LoadNextScene());
    }

    private void StartDeathSequence() {
        state = States.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSFX, deathVolume);
        StartCoroutine(ReloadCurrentScene());
    }

    private IEnumerator LoadNextScene() {
        yield return new WaitForSecondsRealtime(waitTimeBetweenLevels);
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private IEnumerator ReloadCurrentScene() {
        yield return new WaitForSecondsRealtime(waitTimeBetweenDeaths);
        SceneManager.LoadScene(currentSceneIndex);
    }
    private void ProcessMovement() {

        if (state.Equals(States.Alive)) {
            HandleThrust();
            HandleRotation();
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
            audioSource.PlayOneShot(mainEngineSFX, mainEngineVolume);
        }
    }

    private void HandleRotation() {
        rigidbody.freezeRotation = true;
        
        float rotationThisFrame = Input.GetAxis("Horizontal") * Time.deltaTime * reactiveThrust;
        transform.Rotate(new Vector3(0f, 0, rotationThisFrame));
        rigidbody.freezeRotation = false;
    }
}
