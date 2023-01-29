using System;
using System.Collections;
using Photon.Pun;
using StrikeOnline.Core;
using StrikeOnline.UpdatedPlayer;
using UnityEngine;

namespace StrikeOnline.Weapon
{
    public class StandartGun : MonoBehaviourPunCallbacks, IReloadingWeapon
    {
        #region Private Fields

        [SerializeField] private ReloadingWeapon reloadingWeapon;
        private int _currentAmmoStore;
        private int _allAmmo;
        private AudioSource _playAudio;
        private Coroutine _gunCoroutine;
        private ParticleSystem _particleSystem;
        private bool _canShoot;
        private bool _canReload;
        private const string SoundValueKey = "soundValue";
        private Action _gunAction;
        private Rigidbody _rbPhoton;
        private Collider _colPhoton;
        private PlayerUIManager _playerUIManager;
        private Camera _camera;

        #endregion

        #region Private Properties

        private int AllAmmo
        {
            get => _allAmmo;
            set
            {
                _allAmmo = Mathf.Clamp(value, 0, reloadingWeapon.AmmoCapacity);
                if (_allAmmo < reloadingWeapon.StorageCapacity)
                {
                    _canReload = false;
                    print("FUCK");
                }
            }
        }

        private int CurrentAmmo
        {
            get => _currentAmmoStore;
            set
            {
                if (_canShoot)
                {
                    _currentAmmoStore = Mathf.Clamp(value, 0, reloadingWeapon.StorageCapacity);
                }

                if (_currentAmmoStore != reloadingWeapon.StorageCapacity && AllAmmo != 0)
                {
                    _canReload = true;
                }

                if (photonView.IsMine) _playerUIManager.UpdateAmmoBar(_currentAmmoStore, _allAmmo);
            }
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerUIManager = GetComponentInParent<PlayerUIManager>();
            _camera = GetComponentInParent<PlayerCamera>().GetPlayerCamera();
        }

        private void Update()
        {
            if(!photonView.IsMine) return;
            _playAudio.volume =  PlayerPrefs.GetFloat(SoundValueKey);
        }

        private void Start()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _playAudio = GetComponent<AudioSource>();
            _playAudio.maxDistance = reloadingWeapon.Distance;
            _playAudio.volume = PlayerPrefs.HasKey(SoundValueKey) ? PlayerPrefs.GetFloat(SoundValueKey) : 0.5f;
            _canShoot = true;
            AllAmmo = reloadingWeapon.AmmoCapacity;
            CurrentAmmo = reloadingWeapon.StorageCapacity;
            _gunAction += Shoot;
            if (photonView.IsMine) _playerUIManager.UpdateAmmoBar(_currentAmmoStore, _allAmmo);
        }

        private new void OnEnable()
        {
            if (_playAudio) _playAudio.maxDistance = reloadingWeapon.Distance;
            if (photonView.IsMine)
            {
                _playerUIManager.UpdateGunName(reloadingWeapon.Name);
                _playerUIManager.UpdateAmmoBar(_currentAmmoStore, _allAmmo);
            }
            _gunCoroutine = null;
            Debug.Log($"{name} enabled!");
            if (CurrentAmmo < reloadingWeapon.StorageCapacity)
            {
                _canReload = true;
            }

            if (CurrentAmmo > 0)
            {
                _canShoot = true;
                _gunAction = null;
            }

            _gunAction += Shoot;
        }

        private new void OnDisable()
        {
            Debug.Log($"{name} disabled!");
        }

        #endregion

        #region Private Methods

        private void ReloadGun()
        {
            print("Reload Start");
            _gunCoroutine = StartCoroutine(WaitForReload());
        }

        private IEnumerator WaitForReload()
        {
            _playAudio.maxDistance = 20f;
            _gunAction -= Shoot;
            _canReload = false;
            _canShoot = false;
            yield return new WaitForSeconds(reloadingWeapon.ReloadTime);
            _canShoot = true;
            if (AllAmmo <= reloadingWeapon.StorageCapacity)
            {
                CurrentAmmo = AllAmmo;
                AllAmmo = 0;
            }
            else
            {
                AllAmmo -= reloadingWeapon.StorageCapacity - CurrentAmmo;
                CurrentAmmo = reloadingWeapon.StorageCapacity;
            }

            print("Reload End");
            _gunCoroutine = null;
            _gunAction += Shoot;
            _playAudio.maxDistance = reloadingWeapon.Distance;
        }

        private void ParticlePlay()
        {
            if (CurrentAmmo != 0)
            {
                StartCoroutine(ParticleWait());
            }
        }

        private IEnumerator ParticleWait()
        {
            _particleSystem.Play();
            yield return new WaitForSeconds(0.1f);
            _particleSystem.Stop();
        }


        private void BetweenEmptyShoot()
        {
            if (CurrentAmmo == 0 && _gunCoroutine == null)
            {
                _gunCoroutine = StartCoroutine(WaitBetweenEmptyShoot());
            }
        }

        private IEnumerator WaitBetweenEmptyShoot()
        {
            _playAudio.maxDistance = 20f;
            photonView.RPC(nameof(RpcPlayBetweenEmptyShot), RpcTarget.All);
            yield return new WaitForSeconds(reloadingWeapon.TimeBetweenShoots);
            _gunCoroutine = null;
        }

        private void BetweenShoot()
        {
            _gunCoroutine ??= StartCoroutine(WaitBetweenShot());
        }

        private IEnumerator WaitBetweenShot()
        {
            photonView.RPC(nameof(RpcPlayBetweenShot), RpcTarget.All);
            _canShoot = false;
            yield return new WaitForSeconds(reloadingWeapon.TimeBetweenShoots);
            if (CurrentAmmo != 0)
            {
                _canShoot = true;
            }

            _gunCoroutine = null;
        }

        private void Shoot()
        {
            if (_canShoot)
            {
                Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                ray.origin = _camera.transform.position;
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, reloadingWeapon.Distance))
                {
                    hit.collider.GetComponentInParent<IDamageable>()?.TakeDamage(reloadingWeapon.Damage);
                    photonView.RPC(nameof(RPCShoot), RpcTarget.All, hit.point, hit.normal);
                }

                photonView.RPC(nameof(RpcParticlePlay), RpcTarget.All);
                CurrentAmmo -= 1;
                BetweenShoot();
            }


            BetweenEmptyShoot();
        }

        #endregion

        #region PunRPC

        [PunRPC]
        public void RpcPlayBetweenShot()
        {
            _playAudio.PlayOneShot(reloadingWeapon.Shoot);
        }

        [PunRPC]
        public void RpcPlayBetweenEmptyShot()
        {
            _playAudio.PlayOneShot(reloadingWeapon.EmptyShoot, _playAudio.volume*0.5f);
        }

        [PunRPC]
        public void RpcPlayReload()
        {
            _playAudio.PlayOneShot(reloadingWeapon.Reload,_playAudio.volume*0.5f);
        }

        [PunRPC]
        public void RpcParticlePlay()
        {
            ParticlePlay();
        }

        [PunRPC]
        public void RPCShoot(Vector3 hitPosition, Vector3 hitNormal)
        {
            Debug.Log("Yes,RPC SHOOT");
            Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
            if (colliders.Length != 0)
            {
                GameObject bulletImpact = Instantiate(reloadingWeapon.BulletImpact, hitPosition + hitNormal * 0.001f,
                    Quaternion.LookRotation(hitNormal, Vector3.up) * reloadingWeapon.BulletImpact.transform.rotation);
                Destroy(bulletImpact, 10f);
                bulletImpact.transform.SetParent(colliders[0].transform);
            }
        }

        #endregion


        #region IReloadingWeapon

        public void Damage()
        {
            _gunAction?.Invoke();
        }

        public void Reload()
        {
            if (_canReload)
            {
                photonView.RPC(nameof(RpcPlayReload), RpcTarget.All);
                ReloadGun();
            }
        }

        #endregion
    }
}