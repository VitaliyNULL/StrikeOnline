using StrikeOnline.Core;
using UnityEngine;

namespace StrikeOnline.TestObjects
{
    public class UnitForTakeDamage : MonoBehaviour, IDamageable
    {
        [SerializeField] private AudioClip clip;
        private AudioSource _audioSource;
        private int _health = 100;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private int Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, 100);
                if (_health == 0)
                {
                    Death();
                }
            }
        }

        private void Death()
        {
            _audioSource.Stop();
            Debug.Log("You kill me");
            Destroy(gameObject);
        }

        public void TakeDamage(int damage)
        {
            _audioSource.PlayOneShot(clip);
            Health -= damage;
            Debug.Log($"You hit me, my health is {Health} now ");
        }
    }
}