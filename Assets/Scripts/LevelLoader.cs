using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;

    void Awake () {instance = this;}


    public int currentLevel = -1;

    public int[] levels;

    public int levelOffset;

    public CanvasGroup loading;
    public TextMeshProUGUI tipText;


    public string[] tips;

    void Start() {
        StartCoroutine(LoadLevel(true));

    }

    public AsyncOperation NextLevel(bool inc) {
        if(currentLevel != -1)
            SceneManager.UnloadSceneAsync(levels[currentLevel] + levelOffset);
        if(inc)
            currentLevel++;
        var ao = SceneManager.LoadSceneAsync(levels[currentLevel] + levelOffset, LoadSceneMode.Additive);
        return ao;
    }

    public IEnumerator LoadLevel(bool inc = true) {
        tipText.text = tips[Random.Range(0,tips.Length)];
        LeanTween.alphaCanvas(loading, 1, 0.2f / Time.timeScale);
        Time.timeScale = 1;
        yield return new WaitForSeconds(1);
        
        var ao = NextLevel(inc);
        ao.allowSceneActivation = false;
        yield return new WaitForSeconds(2f);
        ao.allowSceneActivation = true;
        LeanTween.alphaCanvas(loading, 0, 0.2f);
        
    }
}
