using System.Collections;
using Photon.Pun;
using StrikeOnline.Core;
using UnityEngine;

namespace StrikeOnline.UpdatedPlayer
{
    public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
    {
        #region Private Fields

        [Header("Player setup")] [SerializeField]
        private float walkSpeed;

        [SerializeField] private float sprintSpeed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private PlayerUIManager playerUIManager;
        [SerializeField] private AudioClip footStepsSound;
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip takeDamageSound;
        private const int MaxHealth = 100;
        private int _health = MaxHealth;
        private bool _grounded;
        private float _horizontalInput;
        private float _verticalInput;
        private float _currentSpeed;
        private float _verticalLookRotation;
        private readonly float _gravity = -9.81f;
        private const string SoundValueKey = "soundValue";
        private AudioSource _audioSource;
        private PlayerManager _playerManager;
        private PhotonMessageInfo _messageInfo;
        private CharacterController _characterController;
        private Vector3 _velocity;
        private Coroutine _audioCoroutine;

        #endregion

        #region Private Properties

        private int Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, 100);
                if(photonView.IsMine) playerUIManager.UpdateHealthBar((float)_health / MaxHealth);
                Debug.Log(_health);
                if (_health == 0)
                {
                    Die();
                }
            }
        }

        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManager>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = PlayerPrefs.HasKey(SoundValueKey) ? PlayerPrefs.GetFloat(SoundValueKey) : 0.5f;
        }

        private void Start()
        {
            if (photonView.IsMine) return;
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(_characterController);
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            if (_playerManager.GetExitMenuBool())
                return;
            MyInput();
            Jump();
            Move();
            Gravity();
            _audioSource.volume =  PlayerPrefs.GetFloat(SoundValueKey);
        }
        

        #endregion

        #region Private Methods

        private void Die()
        {
            _playerManager.Die();
            PlayerManager.Find(_messageInfo.Sender).GetKill();
        }


        private void Jump()
        {
            if (Input.GetKey(KeyCode.Space) && _grounded)
            {
                photonView.RPC(nameof(RPCPlayJumpSound),RpcTarget.All);
                _velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * _gravity);
            }
        }
        private void Move()
        {

            Vector3 moveDirection = transform.right * _horizontalInput + transform.forward * _verticalInput;
            if (_grounded && moveDirection !=Vector3.zero)
            {
                photonView.RPC(nameof(RPCPlayFootStepsSound),RpcTarget.All);
            }
            else if(_grounded&& moveDirection == Vector3.zero)
            {
                _audioSource.Stop();
                _audioCoroutine = null;
            }
            _currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
            _characterController.Move(moveDirection * _currentSpeed * Time.deltaTime);
        }


        private void MyInput()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");
        }
        
        private void Gravity()
        {
            _velocity.y += _gravity * Time.deltaTime;
            _characterController.Move(_velocity * 3.0f * Time.deltaTime);
            if (_grounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
        }

        private IEnumerator WaitForPlayFootSteps()
        {
            _audioSource.PlayOneShot(footStepsSound);
            yield return new WaitForSeconds(footStepsSound.length);
            _audioCoroutine = null;
        }        
        private IEnumerator WaitForPlayJumpSound()
        {
            _audioSource.PlayOneShot(jumpSound);
            yield return new WaitForSeconds(jumpSound.length);
            _audioCoroutine = null;
        }


        #endregion

        #region Public Methods

        public void SetGroundedState(bool grounded)
        {
            _grounded = grounded;
        }

        
        #endregion

        #region IDamagable

        public void TakeDamage(int damage)
        {
            photonView.RPC(nameof(RPCTakeDamage), photonView.Owner, damage);
        }

        #endregion

        #region PunRPC

        [PunRPC]
        void RPCTakeDamage(int damage, PhotonMessageInfo info)
        {
            _audioSource.PlayOneShot(takeDamageSound);
            Health -= damage;
            _messageInfo = info;
        }

        [PunRPC]
        void RPCPlayFootStepsSound()
        {
            _audioCoroutine ??= StartCoroutine(WaitForPlayFootSteps());
        }

        [PunRPC]
        void RPCPlayJumpSound()
        {
            _audioCoroutine = null;
            _audioSource.Stop();
            _audioCoroutine = StartCoroutine(WaitForPlayJumpSound());
        }
        #endregion
    }
}