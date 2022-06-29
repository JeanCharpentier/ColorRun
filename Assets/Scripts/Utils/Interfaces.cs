using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformManager
{
    void ResetPlatform(Platform pPlatform);
    void ReplayGame();
}
public interface IMovingManager
{
    void ChangeSpeed(float pSpeed);
}


public interface IPlatform
{
    int GetState();
}

public interface IGameManager
{
    void SetSpeed(float pSpeed, bool pReset);
    float GetSpeed();
    float IncreaseSpeed();
    float DecreaseSpeed();

    int GetLifes();
    void SetLifes(int pLifes, bool pReset);

    void ResetGame();
    void FillContinues();

    int GetScore();
    void SaveScore();
}

public interface IPlayer
{
    void Jump();
    void VDash();
    void SwitchColor();
    void ResetPlayer();
}


public interface IHUD
{
    void ChangeSprite(int pLifes);
    void UpdateScore(int pScore);
    void UpdateSpeed(float pSpeed);
}

public interface IGOMenu
{
    void ChangeContinues(int pContinues);
}

public interface IScoreManager
{
    void SetHighscore(int pScore,string pPlayerName);
}