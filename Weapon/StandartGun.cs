using System;
using System.Collections;
using Photon.Pun;
using StrikeOnline.Core;
using UnityEngine;

namespace StrikeOnline.Weapon
{
    public class StandartGun : MonoBehaviour, IReloadingWeapon, IPunObservable
    {
        #region Private Fields

        [SerializeField] private ReloadingWeapon reloadingWeapon;
        private Transform _cameraDirection;
        private int _currentAmmoStore;
        private int _allAmmo;
        private AudioSource _playAudio;
        private Coroutine _gunCoroutine;
        private ParticleSystem _particleSystem;
        private bool _canShoot;
        private bool _canReload;
        private Action _gunAction;
        private Rigidbody _rbPhoton;
        private Collider _colPhoton;

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
                    print($"{CurrentAmmo}/{AllAmmo}");
                }

                if (_currentAmmoStore != reloadingWeapon.StorageCapacity && AllAmmo != 0)
                {
                    _canReload = true;
                }
            }
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            _playAudio = GetComponent<AudioSource>();
            _canShoot = true;
            AllAmmo = reloadingWeapon.AmmoCapacity;
            CurrentAmmo = reloadingWeapon.StorageCapacity;
            _gunAction += Shoot;
        }

        private void OnEnable()
        {
            Debug.Log($"{this.name} enabled!");
        }

        private void OnDisable()
        {
            Debug.Log($"{this.name} disabled!");
        }

        #endregion
        
        #region Private Methods

        private void BetweenShoot()
        {
            if (_gunCoroutine == null)
            {
                _gunCoroutine = StartCoroutine(WaitBetweenShot());
            }
        }

        private IEnumerator WaitBetweenShot()
        {
            _playAudio.PlayOneShot(reloadingWeapon.Shoot);
            _canShoot = false;
            yield return new WaitForSeconds(reloadingWeapon.TimeBetweenShoots);
            if (CurrentAmmo != 0)
            {
                _canShoot = true;
            }

            _gunCoroutine = null;
        }

        private void ReloadGun()
        {
            print("Reload Start");
            _gunCoroutine = StartCoroutine(WaitForReload());
        }

        private IEnumerator WaitForReload()
        {
            _gunAction -= Shoot;
            _canReload = false;
            _canShoot = false;
            yield return new WaitForSeconds(reloadingWeapon.ReloadTime);
            print("COCK");
            _canShoot = true;
            AllAmmo -= reloadingWeapon.StorageCapacity - CurrentAmmo;
            if (AllAmmo <= reloadingWeapon.StorageCapacity)
            {
                CurrentAmmo = AllAmmo;
                AllAmmo = 0;
            }
            else
            {
                CurrentAmmo = reloadingWeapon.StorageCapacity;
            }

            print("Reload End");
            _gunCoroutine = null;
            _gunAction += Shoot;
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
            _playAudio.PlayOneShot(reloadingWeapon.EmptyShoot);
            yield return new WaitForSeconds(reloadingWeapon.TimeBetweenShoots);
            _gunCoroutine = null;
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

        private void Shoot()
        {
            if (_canShoot)
            {
                ParticlePlay();
                RaycastHit hit;
                if (Physics.Raycast(_cameraDirection.transform.position, _cameraDirection.transform.forward,
                        out hit, reloadingWeapon.Distance))
                {
                    IDamageable damageable = hit.collider.GetComponentInParent<IDamageable>();
                    damageable?.TakeDamage(reloadingWeapon.Damage);
                }

                CurrentAmmo -= 1;
                BetweenShoot();
            }

            BetweenEmptyShoot();
        }

        #endregion

        #region IReloadingWeapon

        public void Damage()
        {
            _gunAction?.Invoke();
        }

        public void TakeWeaponDirection(Transform position) => _cameraDirection = position;

        public void Reload()
        {
            if (_canReload)
            {
                _playAudio.PlayOneShot(reloadingWeapon.Reload);
                ReloadGun();
            }
        }

        #endregion

        #region IPunObservable

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_rbPhoton);
                stream.SendNext(_colPhoton);
                stream.SendNext(_currentAmmoStore);
                stream.SendNext(_allAmmo);
            }
            else if(stream.IsReading)
            {
                _rbPhoton = (Rigidbody)stream.ReceiveNext();
                _colPhoton = (Collider)stream.ReceiveNext();
                _currentAmmoStore = (int)stream.ReceiveNext();
                _allAmmo = (int)stream.ReceiveNext();
            }
        }

        #endregion
        
    }
}