using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


[DisallowMultipleComponent]
public class SceneLoader : MonoBehaviour{

    //Config

    //State
    float waitTimeBetweenLevels;
    float waitTimeBetweenDeaths;

    //Cached component references
    int currentSceneIndex;

    private void Awake() {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Start() {
        waitTimeBetweenLevels = 2f;
        waitTimeBetweenDeaths = 5f;
    }

    public void LoadNextScene() {
        StartCoroutine(LoadNextSceneOnTimer());
    }

    private IEnumerator LoadNextSceneOnTimer() {
        yield return new WaitForSecondsRealtime(waitTimeBetweenLevels);
        if (!currentSceneIndex.Equals(0) && 
                (SceneManager.sceneCountInBuildSettings - 1) %  currentSceneIndex == 0) {
            SceneManager.LoadScene(0);
        }
        else {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    public void ReloadCurrentScene() {
        StartCoroutine(ReloadCurrentSceneOnTimer());
    }
    private IEnumerator ReloadCurrentSceneOnTimer() {
        yield return new WaitForSecondsRealtime(waitTimeBetweenDeaths);
        SceneManager.LoadScene(currentSceneIndex);
    }
}
