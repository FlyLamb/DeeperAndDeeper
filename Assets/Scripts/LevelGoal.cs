using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{
    public string requireTag;
    public Tag playerTag;
    private GameObject winPanel;

    void Start () {
        winPanel = GameObject.Find("Win Panel");
        LeanTween.scaleX(winPanel,0,0);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.HasTag(GTags.player)) {
            StartCoroutine("Win");
        }
    }

    IEnumerator Win() {
        LeanTween.scaleX(winPanel,1,0.2f);
        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetTrigger("Shake");
        yield return new WaitForSeconds(2f);
        LevelLoader.instance.StartCoroutine(LevelLoader.instance.LoadLevel(true));
    }
}
