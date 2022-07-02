using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZVolume : MonoBehaviour
{
    [SerializeField]
    Player _player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_player.transform.position.x,-2,0);
    }
}
