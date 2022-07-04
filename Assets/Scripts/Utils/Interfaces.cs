using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformManager
{
    void ResetPlatform(Platform pPlatform);
    void ReplayGame();
}

public interface IBackgroundManager
{
    void ResetPlatform(Background pBG);
    void ReplayGame();
}


public interface IPlatform
{
    int GetState();
    void ChangeColor();
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

    void ChangeBloom(float pBloom);
}

public interface IPlayer
{
    void Jump();
    void VDash();
    void SwitchColor();
    void ResetPlayer();
    void ChangeSpeed(float pSpeed);

    Vector3 GetPos();
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