using UnityEngine;

namespace StrikeOnline.Weapon
{
    [CreateAssetMenu(menuName = "Weapon/ThrowingWeapon", fileName = "ThrowingWeapon")]
    public class ThrowingWeapon: ScriptableObject
    {
        [SerializeField] private new string name;
        [SerializeField] private int damage;
        [SerializeField] private float distance;
        [SerializeField] private float timeToThrow;
        [SerializeField] private AudioClip throwGrenade;
        [SerializeField] private AudioClip beforeThrowGrenade;
        [SerializeField] private AudioClip explodeSound;
        
        public float Distance => distance;
        public float TimeToThrow => timeToThrow;
        public int Damage => damage;
        public string Name => name;
        public AudioClip ThrowGrenade => throwGrenade;
        public AudioClip ExplodeSound => explodeSound;
        public AudioClip BeforeThrowGrenade => beforeThrowGrenade;


    }
}