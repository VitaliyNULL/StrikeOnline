using System.Collections;
using StrikeOnline.Core;
using UnityEngine;

namespace StrikeOnline.Weapon
{
    public class StandartGrenade : MonoBehaviour, IThrowingWeapon
    {
        #region Private Fields

        [SerializeField] private ThrowingWeapon throwingWeapon;
        [SerializeField] private new ParticleSystem particleSystem;
        private Transform _cameraPosition;
        private Rigidbody _rigidbody;
        private Collider _sphereCollider;

        #endregion

        #region MonoBehaviour CallBacks

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _sphereCollider = GetComponentInChildren<Collider>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 5f);
        }

        #endregion

        #region Private Methods

        private void GrenadeExplosion() => StartCoroutine(WaitForExplosion());

        private IEnumerator WaitForExplosion()
        {
            yield return new WaitForSeconds(5f);
            Explosion();
            Instantiate(particleSystem, transform.position, transform.rotation);
            AudioSource.PlayClipAtPoint(throwingWeapon.ExplodeSound, transform.position);
            Destroy(gameObject);
        }

        private void Explosion()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
            foreach (var varCollider in colliders)
            {
                IDamageable damageable = varCollider.GetComponentInParent<IDamageable>();
                damageable?.TakeDamage(throwingWeapon.Damage);
            }
        }

        #endregion

        #region IThrowingWeapon
        public void Damage()
        {
            _rigidbody.useGravity = true;
            _sphereCollider.isTrigger = false;
            print("Excellent throw");
            GrenadeExplosion();
        }


        public void Throw()
        {
            _rigidbody.AddForce(
                _cameraPosition.transform.forward * throwingWeapon.Distance + _cameraPosition.transform.up,
                ForceMode.Impulse);
            _rigidbody.AddTorque(_rigidbody.transform.right + _rigidbody.transform.forward, ForceMode.Impulse);
            Damage();
        }
        

        #endregion

    }
}