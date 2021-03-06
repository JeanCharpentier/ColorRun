using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IPlayer
{

    public CameraShake _cameraShake;

    [SerializeField]
    int _jumpStrenth;
    int _jumpForce;
    float _speed;
    float _pos;
    bool isOnGround;
    bool isJumping;
    bool isDashing;
    Rigidbody _playerBody;
    Transform _playerTransform;

    int _state; // 0 = Rose, 1 = Bleu
    MeshRenderer _meshPlayer;
    MeshRenderer _meshInvul;

    Vector3 _basePos;
    Vector3 _move;

    bool canVibrate;
    AudioSource _sndDash;

    [SerializeField]
    AudioClip _audDash;
    [SerializeField]
    AudioClip _audPickup;

    ParticleSystem _psDash;

    PlayerInput _controls;


    // Invulnerabilité
    bool isGod;
    float tiDuration;
    float tiTimer;

    GameObject _goMenu;

    IGameManager srvGManager;
    IScoreManager srvSManager;
    //IPlatformManager srvPManager;
    void Awake()
    {
        ServicesLocator.AddService<IPlayer>(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        _controls = GetComponent<PlayerInput>();
#if UNITY_ANDROID
        _jumpForce = _jumpStrenth;
        _controls.defaultActionMap = "Touch";
#endif

#if UNITY_STANDALONE_WIN
        _jumpForce = _jumpStrenth / 2;
        _controls.defaultActionMap = "Keyboard";
#endif


        srvGManager = ServicesLocator.GetService<IGameManager>();
        srvSManager = ServicesLocator.GetService<IScoreManager>();
        //srvPManager = ServicesLocator.GetService<IPlatformManager>();

        _playerBody = gameObject.GetComponent<Rigidbody>();
        isOnGround = false;
        isJumping = true;
        isDashing = false;
        _state = 0;
        _basePos = transform.position;

        _playerTransform = gameObject.GetComponentsInChildren<MeshFilter>()[0].transform;
        _meshPlayer = GetComponentsInChildren<MeshFilter>()[0].GetComponent<MeshRenderer>();
        _meshInvul = transform.GetChild(1).GetComponentInChildren<MeshRenderer>();

        _sndDash = GetComponent<AudioSource>();
        if(PlayerPrefs.GetFloat("volume") <= 0)
        {
            _sndDash.enabled = false;
        }else
        {
            _sndDash.volume = PlayerPrefs.GetFloat("volume") / 3;
        }

        _psDash = transform.GetChild(0).GetComponent<ParticleSystem>();

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

        _goMenu = GameObject.Find("GO_Canvas");
    }
    void Update()
    {

        _move = Vector3.right * _speed * Time.deltaTime;
        _playerBody.MovePosition(_playerBody.position + _move);

        _playerTransform.Rotate(0,0,(-_speed/Mathf.PI)*Time.deltaTime*160); // Rotation de la balle

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

                _sndDash.clip = _audDash;
                _sndDash.Play();
                _psDash.Emit(50);

#if UNITY_ANDROID
                if (canVibrate)
                {
                    Handheld.Vibrate();
                }
#endif
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

        if (other.gameObject.CompareTag("bonus"))
        {
            Destroy(other.gameObject);
            _psDash.Emit(50);
            _sndDash.clip = _audPickup;
            _sndDash.Play();
            srvSManager.BonusScore(500);
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
            _meshPlayer.sharedMaterials[1].color = CF._colList[_state];
        }
    }

    public void Invulnerability(bool pBool)
    {
        if(pBool)
        {
            _meshInvul.enabled = true;
        }else
        {
            _meshInvul.enabled = false;
        }
        isGod = pBool;
    }

    public void ResetPlayer()
    {
        transform.position = _basePos;
        _state = 0;
        isOnGround = false;
        _meshPlayer.sharedMaterials[1].color = CF._colList[_state];
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
