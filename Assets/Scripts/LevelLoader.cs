using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;

    void Awake () {instance = this;}

    
    


    public CanvasGroup loading;
    public TextMeshProUGUI tipText;

    public string[] levels;
    public string finalSceneName;
    public string[] tips;

    public bool debugMode = false;
    public int debugModeScene = -1;
    

    private bool isLoading = false;
    [HideInInspector]
    public int currentLevel = -1;
    

    void Start() {
#if !UNITY_EDITOR
        debugMode = false;
#endif

        if(debugMode) {
            currentLevel = debugModeScene - 1;
        }
        StartCoroutine(LoadLevel(true));
    }

    public AsyncOperation NextLevel(bool increment) {
        
        if(SceneManager.sceneCount > 1) // Unload only if there is anything to unload
            SceneManager.UnloadSceneAsync($"Levels/{levels[currentLevel]}/{levels[currentLevel]}");

        if(increment)
            currentLevel++;

        if(currentLevel >= levels.Length) { // End the game
            return SceneManager.LoadSceneAsync(finalSceneName, LoadSceneMode.Single);
        }
        
        return SceneManager.LoadSceneAsync(levels[currentLevel], LoadSceneMode.Additive);
    }

    public IEnumerator LoadLevel(bool increment = true) {
        if(LevelDebugger.isDebugMode) {
            if(increment) {
                Debug.Log("Level completed! Debug mode is enabled so we are not loading another level.");
                
            }
            else Debug.Log("Level failed... Reloading!"); {
                Time.timeScale = 1;
                var asyncLoading = NextLevel(false);

                asyncLoading.allowSceneActivation = true;
            }
        } else
        if(!isLoading){
            isLoading = true;
            tipText.text = tips[Random.Range(0,tips.Length)];
            LeanTween.alphaCanvas(loading, 1, 0.2f / Time.timeScale);
            Time.timeScale = 1;
            yield return new WaitForSeconds(1);

            var winPanel = GameObject.Find("Win Panel"); // TODO: Rewrite ??????
            LeanTween.scaleX(winPanel,0,0);

            LeanTween.alphaCanvas(Messenger.instance.panel,0,0.1f);
            
            var asyncLoading = NextLevel(increment);

            asyncLoading.allowSceneActivation = false;

            yield return new WaitForSeconds(2f);
            asyncLoading.allowSceneActivation = true;

            LeanTween.alphaCanvas(loading, 0, 0.2f);

            isLoading = false;
        }
        
    }
}
