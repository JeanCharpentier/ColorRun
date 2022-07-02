using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IPlayer
{

    public CameraShake _cameraShake;

    public int _jumpForce;
    float _speed;
    Vector3 _pos;
    bool isOnGround;
    bool isJumping;
    bool isDashing;
    Rigidbody _playerBody;
    int _state; // 0 = Rose, 1 = Bleu

    Vector3 _basePos;
    Vector3 _move;

    bool canVibrate;


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
        isJumping = true;
        isDashing = false;
        _state = 0;
        _basePos = transform.position;

        isGod = false;
        tiTimer = 0.0f;
        tiDuration = 5.0f;

        if(PlayerPrefs.GetInt("vibration") == 1)
        {
            canVibrate = true;
        }else
        {
            canVibrate = false;
        }

        GetComponentsInChildren<MeshFilter>()[0].GetComponent<MeshRenderer>().sharedMaterials[1].color = CF._colList[_state];

        _goMenu = GameObject.Find("GO_Canvas");
    }
    void Update()
    {

        _move = Vector3.right * _speed * Time.deltaTime;
        //_move = new Vector3(1 * _speed * Time.deltaTime, _playerBody.position.y, 0);
        _playerBody.MovePosition(transform.position + _move);

        gameObject.GetComponentsInChildren<MeshFilter>()[0].transform.Rotate(0,0,(-_speed/Mathf.PI)*Time.deltaTime*160); // Rotation de la balle

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

    void OnCollisionEnter(Collision pCol)
    {
        if (!isOnGround && isJumping)
        {
            if(isDashing) // Si c'est un Dash, on shake plus fort !
            {
                StartCoroutine(_cameraShake.Shake(0.1f, 0.3f));
                if (canVibrate)
                {
                    Handheld.Vibrate();
                }
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

    void OnCollisionExit(Collision pCol)
    {
        if (pCol.collider.gameObject.CompareTag("platform") && isJumping)
        {
            isOnGround = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "KillZVolume")
        {
            _goMenu.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0;
        }
    }

    public void Jump()
    {
        if (isOnGround)
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
        if(!isOnGround)
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

    public void ResetPlayer()
    {
        transform.position = _basePos;
        _state = 0;
        GetComponentsInChildren<MeshFilter>()[0].GetComponent<MeshRenderer>().sharedMaterials[1].color = CF._colList[_state];
        Invulnerability(true);
    }

    public void ChangeSpeed(float pSpeed)
    {
        _speed = pSpeed;
    }

    public Vector3 GetPos()
    {
        return _playerBody.position;
    }
}
