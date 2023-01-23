using UnityEngine;

namespace StrikeOnline.Core
{
    public interface IWeapon
    {
        void Damage();
        void TakeWeaponDirection(Transform position);
    }

    public interface IReloadingWeapon : IWeapon
    {
        void Reload();
    }

    public interface IThrowingWeapon : IWeapon
    {
        void Throw();
    }
    
}