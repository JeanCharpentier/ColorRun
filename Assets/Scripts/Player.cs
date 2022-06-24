using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//[DefaultExecutionOrder(-1)]
public class Player : MonoBehaviour, IPlayer
{
    public int _jumpForce;
    public CameraShake _cameraShake;
    bool isOnGround;
    bool isJumping;
    bool isDashing;
    Rigidbody _playerBody;
    int _state; // 0 = Rose, 1 = Bleu


    // Invulnerabilité
    bool isGod;
    float tiDuration;
    float tiTimer;

    GameObject _goMenu;

    IGameManager srvGManager;
    void Awake()
    {
        ServicesLocator.AddService<IPlayer>(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        srvGManager = ServicesLocator.GetService<IGameManager>();

        _playerBody = gameObject.GetComponent<Rigidbody>();
        isOnGround = false;
        isJumping = false;
        isDashing = false;
        _state = 1;


        isGod = false;
        tiTimer = 0.0f;
        tiDuration = 5.0f;

        SwitchColor();

        _goMenu = GameObject.Find("GO_Canvas");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponentsInChildren<MeshFilter>()[0].transform.Rotate(0,0,(-srvGManager.GetSpeed()/Mathf.PI)); // Rotation de la balle

        if(isGod) // Fin de l'invulnerabilité
        {
            if(tiTimer >= tiDuration)
            {
                Invulnerability(false);
                tiTimer = 0.0f;
            }
            tiTimer += Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision pCol){
        
        if (!isOnGround && isJumping)
        {
            if(isDashing)
            {
                StartCoroutine(_cameraShake.Shake(0.1f, 0.3f));
            }else
            {
                StartCoroutine(_cameraShake.Shake(0.1f, 0.07f));
            }
            
        }

        isOnGround = true;
        isJumping = false;
        isDashing = false;

        if (!isGod)
        {
            if (pCol.collider.gameObject.CompareTag("platform"))
            {
                if (pCol.gameObject.tag.Equals("platform"))
                {
                    Platform _platform = pCol.gameObject.GetComponent<Platform>();
                    if (_platform.GetState() != _state)
                    {
                        if (srvGManager.GetLifes() > 0)
                        {
                            srvGManager.SetLifes(srvGManager.GetLifes() - 1, false);
                            Invulnerability(true);
                        }else
                        {
                            _goMenu.GetComponent<Canvas>().enabled = true;
                            Time.timeScale = 0;
                        }
                    }
                }
            }
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
            _playerBody.AddForce(Vector3.up * _jumpForce);
            isJumping = true;
        }
    }

    public void VDash()
    {
        if(!isOnGround)
        {
            _playerBody.AddForce(Vector3.down * _jumpForce * 1.3f);
            isDashing = true;
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

    public void Invulnerability(bool pBool)
    {
        if(pBool)
        {
            transform.GetChild(1).GetComponentInChildren<MeshRenderer>().enabled = true;
        }else
        {
            transform.GetChild(1).GetComponentInChildren<MeshRenderer>().enabled = false;
        }
        isGod = pBool;
    }
}
