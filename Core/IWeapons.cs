namespace StrikeOnline.Core
{
    public interface IWeapon
    {
        void Damage();
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