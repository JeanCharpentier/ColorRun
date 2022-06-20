using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformManager
{
    void ResetPlatform(Platform pPlatform);
}

public interface IPlatform
{
    int GetState();
}

public interface IGameManager
{
    void SetSpeed(float pSpeed);
    float GetSpeed();
    float IncreaseSpeed();
    float DecreaseSpeed();
}

public interface IPlayer
{
    void Jump();
    void VDash();
    void SwitchColor();
}
