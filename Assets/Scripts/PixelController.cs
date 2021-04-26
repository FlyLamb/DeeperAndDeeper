using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelController : MonoBehaviour
{
    public GameObject player;


    public void Update() {
        if(player != null)
        GetComponent<Pixelate>().pixelDensity = Mathf.RoundToInt((256 / 120) * Mathf.Pow(Vector3.Distance(transform.position,player.transform.position),1.75f));
        else player = GameObject.Find("Player");
    }
}
