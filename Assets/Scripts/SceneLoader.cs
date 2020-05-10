using System.Collections;
using System.Collections.Generic;
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
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void ReloadCurrentScene() {
        StartCoroutine(ReloadCurrentSceneOnTimer());
    }
    private IEnumerator ReloadCurrentSceneOnTimer() {
        yield return new WaitForSecondsRealtime(waitTimeBetweenDeaths);
        SceneManager.LoadScene(currentSceneIndex);
    }
}
