using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    IPlayer srvPlayer;
    void Start()
    {
        srvPlayer = ServicesLocator.GetService<IPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < srvPlayer.GetPos().x - 5)
        {
            Destroy(gameObject);
        }
    }
}
