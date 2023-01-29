using StrikeOnline.Core;
using UnityEngine;

namespace StrikeOnline.Weapon
{
    public class GunItem: MonoBehaviour
    {
        public StandartGun gunInfo;
        public IReloadingWeapon GunReloadingWeapon;
        public GameObject gunGameObject;

        private void Awake()
        {
            GunReloadingWeapon = gunInfo.GetComponent<IReloadingWeapon>();
        }
    }
}