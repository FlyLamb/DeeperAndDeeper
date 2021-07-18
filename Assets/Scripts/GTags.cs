using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTags : MonoBehaviour
{
    public static Tag player;

    public Tag m_player;
    void Awake() {
        player = m_player;
    }
}
