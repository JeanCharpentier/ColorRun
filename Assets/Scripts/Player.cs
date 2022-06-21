using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//[DefaultExecutionOrder(-1)]
public class Player : MonoBehaviour, IPlayer
{
    public int _jumpForce;
    bool isOnGround;
    Rigidbody pBody;
    int _state; // 0 = Rose, 1 = Bleu

    IGameManager srvGManager;
    IPlatform srvPlatform;

    void Awake()
    {
        ServicesLocator.AddService<IPlayer>(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        srvGManager = ServicesLocator.GetService<IGameManager>();
        srvPlatform = ServicesLocator.GetService<IPlatform>();

        pBody = gameObject.GetComponent<Rigidbody>();
        isOnGround = false;
        _state = 1;
        SwitchColor();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponentsInChildren<MeshFilter>()[0].transform.Rotate(0,0,(-srvGManager.GetSpeed()/Mathf.PI));
    }

    void OnCollisionEnter(Collision pCol){
        if(pCol.collider.gameObject.CompareTag("platform"))
        {
            if(pCol.gameObject.tag.Equals("platform"))
            {
                Platform _platform = pCol.gameObject.GetComponent<Platform>();
                if(_platform.GetState() != _state)
                {
                    Debug.LogWarning($"Perte de vie !!!");
                }
            }
            isOnGround = true;
        }
    }

    void OnCollisionExit(Collision pCol)
    {
        if (pCol.collider.gameObject.CompareTag("platform"))
        {
            isOnGround = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "KillZ")
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void Jump()
    {
        if(isOnGround)
        {
            pBody.AddForce(Vector3.up * _jumpForce);
        }
    }

    public void VDash()
    {
        if(!isOnGround)
        {
            pBody.AddForce(Vector3.down * _jumpForce * 1.3f);
        }
    }

    public void SwitchColor()
    {
        if (_state == 0)
        {
            _state = 1;
        }
        else
        {
            _state = 0;
        }
        GetComponentsInChildren<MeshFilter>()[0].GetComponent<MeshRenderer>().sharedMaterials[1].color = CF._colList[_state];
    }
}
