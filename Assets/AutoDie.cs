using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDie : MonoBehaviour
{
    public float tm;

    void Start() {
        StartCoroutine(count());
    }

    IEnumerator count() {
        yield return new WaitForSeconds(tm);
        Destroy(gameObject);
    }
}
