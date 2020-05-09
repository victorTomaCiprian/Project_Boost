
using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour{

    //Config
    [SerializeField] float mainThrust = 10f;
    [SerializeField] float reactiveThrust = 200f;

    //State
    float waitTimeBetweenLevels = 2f;
    float waitTimeBetweenDeaths = 1f;

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
        //reactiveThrust = 200f;
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
                state = States.Transcend;
                StartCoroutine(LoadNextScene());
                break;
            default:
                state = States.Dead;
                StartCoroutine(ReloadCurrentScene());
                audioSource.Stop();
                break;
        }
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
