using UnityEngine;

namespace StrikeOnline.Weapon
{
    [CreateAssetMenu(menuName = "Weapon/ReloadingWeapon", fileName = "ReloadingWeapon")]
    public class ReloadingWeapon : ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private int damage;
        [SerializeField] private float reloadTime;
        [SerializeField] private int ammoCapacity;
        [SerializeField] private int storageCapacity;
        [SerializeField] private float distance;
        [SerializeField] private float timeBetweenShots;
        [SerializeField] private AudioClip shoot;
        [SerializeField] private AudioClip reload;
        [SerializeField] private AudioClip emptyShoot;
        public AudioClip EmptyShoot => emptyShoot;
        public AudioClip Shoot => shoot;
        public AudioClip Reload => reload;
        public float Distance => distance;
        public int Damage => damage;
        public string Name => name;
        public float ReloadTime => reloadTime;
        public int AmmoCapacity => ammoCapacity;
        public int StorageCapacity => storageCapacity;
        public float TimeBetweenShoots => timeBetweenShots;
    }
}