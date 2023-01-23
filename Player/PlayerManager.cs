using Photon.Pun;
using StrikeOnline.Camera;
using StrikeOnline.Core;
using UnityEngine;

namespace StrikeOnline.Player
{
    public class PlayerManager : MonoBehaviourPunCallbacks,IDamageable, IPunObservable
    {
        #region Private Fields

        [Header("Value")] [SerializeField] private float speed;
        [SerializeField] private float crouchSpeed;
        [SerializeField] private float jumpHeight;

        [Header("Object")] [SerializeField] private LayerMask groundMask;
        [SerializeField] private Transform groundCheckPosition;
        [Tooltip("Camera position on player Head")]
        [SerializeField] private Transform cameraPosition;

        //crouching 
        private bool _isCrouch;

        private float _currentSpeed;

        // value for ground check
        private readonly float _groundCheckRadius = 0.4f;
        private bool _isGrounded;

        private CharacterController _controller;
        private float _horizontalInput;
        private float _verticalInput;

        //gravity value
        private readonly float _gravity = -9.81f;
        private Vector3 _velocity;
        private int _health=100;

        #endregion

        #region Public Fields

        public static GameObject LocalPlayerInstance;

        #endregion

        #region Public Properties

        public int Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, 100);
                if (_health == 0)
                {
                    Debug.Log("I`m Dead");
                }
            }
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            CameraManager _cameraManager = UnityEngine.Camera.main.GetComponent<CameraManager>();
            if (_cameraManager)
            {
                if (photonView.IsMine)
                {
                    Debug.Log("Set Camera");
                    _cameraManager.SetCameraPosition(cameraPosition,transform);
                    _cameraManager.enabled = true;  
                }
            }
            
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                MyInput();
                Move();
                Jump();
                CheckGround();
                Crouch();
                Gravity();
            }
        }

        #endregion

        #region Private Methods

        private void MyInput()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");
        }

        private void Move()
        {
            Vector3 moveDirection = transform.right * _horizontalInput + transform.forward * _verticalInput;
            _currentSpeed = !_isCrouch ? speed : crouchSpeed;
            _controller.Move(moveDirection * _currentSpeed * Time.deltaTime);
        }


        private void Jump()
        {
            if (Input.GetKey(KeyCode.Space) && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * _gravity);
            }
        }

        private void Gravity()
        {
            _velocity.y += _gravity * Time.deltaTime;
            _controller.Move(_velocity * 3.0f * Time.deltaTime);
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
        }

        private void CheckGround() =>
            _isGrounded = Physics.CheckSphere(groundCheckPosition.position, _groundCheckRadius, groundMask);

        private void Crouch() => _isCrouch = Input.GetKey(KeyCode.LeftShift);

        #endregion

        #region IPunObservable

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(this.Health);
            }
            else
            {
                // Network player, receive data
                this.Health = (int)stream.ReceiveNext();
            }
        }

        #endregion

        #region IDamagable

        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

        #endregion
       
    }
}