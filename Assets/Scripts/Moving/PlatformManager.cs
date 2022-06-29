using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour, IPlatformManager,IMovingManager
{
    [SerializeField]
    List<Platform> _platformList;
    public List<Platform> _platformBag;
    Platform _tmpPlatform;

    
    float _curSpeed;

    // Services
    IGameManager srvGManager;

    private void Awake()
    {
        ServicesLocator.AddService<IPlatformManager>(this);
        ServicesLocator.AddService<IMovingManager>(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        srvGManager = ServicesLocator.GetService<IGameManager>();
        
        _curSpeed = srvGManager.GetSpeed();

        float offset = 0;
        _platformList = new List<Platform>();
        do{
            int index = Random.Range(0,_platformBag.Count-1);
            _tmpPlatform = Instantiate<Platform>(_platformBag[index],new Vector3(offset,0,0),Quaternion.identity);
            _tmpPlatform._speed = _curSpeed;
            _tmpPlatform._state = 0;
            _tmpPlatform.GetComponentsInChildren<MeshFilter>()[0].GetComponent<MeshRenderer>().sharedMaterials[1].color = CF._colList[0];
            _tmpPlatform.GetComponentsInChildren<MeshFilter>()[0].GetComponent<MeshRenderer>().sharedMaterials[0].color = Color.grey;
            _platformList.Add(_tmpPlatform);

            _platformBag.RemoveAt(index);
            
            offset = _tmpPlatform._length+_tmpPlatform.transform.position.x;
            
        } while(_platformBag.Count > 0);
    }

    public void ReplayGame()
    {
        foreach(Platform p in _platformList)
        {
            _platformBag.Add(p);
        }

        _platformList.Clear();

        float offset = 0;

        do
        {
            int index = Random.Range(0, _platformBag.Count - 1);

            _tmpPlatform = _platformBag[index];
            _tmpPlatform.transform.position = new Vector3(offset, 0, 0);
            _tmpPlatform.ResetThisPlatform();

            _platformList.Add(_tmpPlatform);

            _platformBag.RemoveAt(index);

            offset = _tmpPlatform._length + _tmpPlatform.transform.position.x;

        } while (_platformBag.Count > 0);
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void ResetPlatform(Platform pPlatform) // Ajoute la plateforme "détruite" au sac
    {
        _platformBag.Add(pPlatform);
        _platformList.Remove(pPlatform);
        if(_platformBag.Count > 2)
        {
            MovePlatform();
        }
    }

    void MovePlatform() {
        int index = Random.Range(0,_platformBag.Count-1);
        Platform prevPlatform = _platformList[_platformList.Count-1];
        Transform nextPlatformPos = prevPlatform.transform.Find("NextPlatform"); // On récupère le slot pour coller la nouvelle plateforme
        nextPlatformPos.position = new Vector3(nextPlatformPos.position.x,Random.Range(-1.5f,1.5f)*0.5f,0);

        _platformBag[index].transform.position = nextPlatformPos.position;
        _platformBag[index].isTP = false;

        // Change la couleur des plateformes avant de les retirer du sac
        int color = Random.Range(0, CF._colList.Length);
        _platformBag[index]._state = color; // On met l'état correspondant à la couleur
        for(int i = 0;i <= _platformBag[index]._length - 1; i++)
        {
            _platformBag[index].GetComponentsInChildren<MeshFilter>()[i*2].GetComponent<MeshRenderer>().materials[1].color = CF._colList[color];
        }

        _platformList.Add(_platformBag[index]);
        _platformBag.Remove(_platformBag[index]);
    }

    public void ChangeSpeed(float pSpeed)
    {
        foreach (Platform p in _platformList)
        {
            p._speed = pSpeed;
        }
        foreach (Platform p in _platformBag)
        {
            p._speed = pSpeed;
        }
    }
}
