using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour, IPlatformManager
{

    List<Platform> _platformList;
    public List<Platform> _platformBag;
    Platform _tmpPlatform;
    

    float curSpeed;

    //Timer
    float tTimer;
    [SerializeField]
    float tTimerRate;

    // Services
    IGameManager srvGManager;

    // Start is called before the first frame update
    void Start()
    {
        ServicesLocator.AddService<IPlatformManager>(this);
        srvGManager = ServicesLocator.GetService<IGameManager>();
        curSpeed = srvGManager.GetSpeed();

        tTimer = 0;

        float offset = 0;
        _platformList = new List<Platform>();
        do{
            int index = Random.Range(0,_platformBag.Count-1);
            _tmpPlatform = Instantiate<Platform>(_platformBag[index],new Vector3(offset,0,0),Quaternion.identity);
            _tmpPlatform._speed = curSpeed;
            _tmpPlatform._state = 0;
            _tmpPlatform.GetComponentsInChildren<MeshFilter>()[0].GetComponent<MeshRenderer>().sharedMaterials[1].color = CF._colList[0];
            _tmpPlatform.GetComponentsInChildren<MeshFilter>()[0].GetComponent<MeshRenderer>().sharedMaterials[0].color = Color.grey;
            _platformList.Add(_tmpPlatform);
            
            _platformBag.RemoveAt(index);
            
            offset = _tmpPlatform._length+_tmpPlatform.transform.position.x;
            
        } while(_platformBag.Count > 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(tTimer >= tTimerRate) {
            curSpeed = srvGManager.IncreaseSpeed();
            foreach(Platform p in _platformList){
                p._speed = curSpeed;
            }
            foreach(Platform p in _platformBag){
                p._speed = curSpeed;
            }
            tTimer = 0;
            Debug.LogWarning("Speed = "+curSpeed);
        }
        tTimer += Time.deltaTime; 
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
        Transform nextPlatformPos = prevPlatform.transform.Find("NextPlatform");
        nextPlatformPos.position = new Vector3(nextPlatformPos.position.x,Random.Range(-1.5f,1.5f)*0.5f,0);

        _platformBag[index].transform.position = nextPlatformPos.position;
        _platformBag[index]._isTP = false;

        // Change la couleur des plateformes avant de lesretirer du sac
        int color = Random.Range(0, CF._colList.Length);
        _platformBag[index]._state = color; // On met l'état correspondant à la couleur
        for(int i = 0;i <= _platformBag[index]._length - 1; i++)
        {
            _platformBag[index].GetComponentsInChildren<MeshFilter>()[i*2].GetComponent<MeshRenderer>().materials[1].color = CF._colList[color];
        }

        _platformList.Add(_platformBag[index]);
        _platformBag.Remove(_platformBag[index]);
    }
}
