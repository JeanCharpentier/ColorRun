using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour, IBackgroundManager
{
    [SerializeField]
    List<Background> _bgList;
    [SerializeField]
    List<Background> _bgBag;
    Background _tmpPlatform;


    float _curSpeed;

    // Services
    IGameManager srvGManager;

    private void Awake()
    {
        ServicesLocator.AddService<IBackgroundManager>(this);
    }
    void Start()
    {
        srvGManager = ServicesLocator.GetService<IGameManager>();

        _curSpeed = srvGManager.GetSpeed();

        float offset = transform.position.x;
        _bgList = new List<Background>();
        do
        {
            int index = Random.Range(0, _bgBag.Count - 1);
            _tmpPlatform = Instantiate<Background>(_bgBag[index], new Vector3(offset, transform.position.y, Random.Range(transform.position.z - 1, transform.position.z + 2)), Quaternion.identity);
            _tmpPlatform._speed = _curSpeed;

            _bgList.Add(_tmpPlatform);

            _bgBag.RemoveAt(index);

            offset = _tmpPlatform._length + _tmpPlatform.transform.position.x;

        } while (_bgBag.Count > 0);
    }
    public void ReplayGame()
    {
        foreach (Background p in _bgList)
        {
            _bgBag.Add(p);
        }

        _bgList.Clear();

        float offset = 0;

        do
        {
            int index = Random.Range(0, _bgBag.Count - 1);

            _tmpPlatform = _bgBag[index];
            _tmpPlatform.transform.position = new Vector3(offset, transform.position.y, Random.Range(transform.position.z-1,transform.position.z+2));
            _tmpPlatform.ResetThisPlatform();

            _bgList.Add(_tmpPlatform);

            _bgBag.RemoveAt(index);

            offset = _tmpPlatform._length + _tmpPlatform.transform.position.x;

        } while (_bgBag.Count > 0);
    }

    public void ResetPlatform(Background pPlatform) // Ajoute la plateforme "détruite" au sac
    {
        _bgBag.Add(pPlatform);
        _bgList.Remove(pPlatform);

        if (_bgBag.Count > 2)
        {
            MovePlatform();
        }
    }

    void MovePlatform()
    {
        int index = Random.Range(0, _bgBag.Count - 1);
        Background prevPlatform = _bgList[_bgList.Count - 1];
        Transform nextPlatformPos = prevPlatform.transform.Find("NextPlatform"); // On récupère le slot pour coller la nouvelle plateforme
        nextPlatformPos.position = new Vector3(nextPlatformPos.position.x, transform.position.y, Random.Range(transform.position.z - 1, transform.position.z + 2));

        _bgBag[index].transform.position = nextPlatformPos.position;
        _bgBag[index].isTP = false;

        // Change la couleur des plateformes avant de les retirer du sac
        int color = Random.Range(0, CF._colList.Length);

        _bgList.Add(_bgBag[index]);
        _bgBag.Remove(_bgBag[index]);
    }
}
