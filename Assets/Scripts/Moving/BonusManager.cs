using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [SerializeField]
    Bonus _bonus;

    float tBonus;
    [SerializeField]
    float tBonusRate;

    IPlayer srvPlayer;
    private void Start()
    {
        srvPlayer = ServicesLocator.GetService<IPlayer>();
    }
    void Update()
    {
        if(tBonus >= tBonusRate)
        {
            tBonus = 0;
            Bonus _newInstance = Instantiate<Bonus>(_bonus, new Vector3(srvPlayer.GetPos().x + 10, transform.position.y, transform.position.z), Quaternion.identity);
        }
        tBonus += Time.deltaTime;
    }
}
