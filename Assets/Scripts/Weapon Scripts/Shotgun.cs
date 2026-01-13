using UnityEngine;

public class Shotgun : PlayerFirearm
{
    public int shotgunID = 2;

    private void Awake()
    {
        fireFX = (AudioClip) Resources.Load("Sounds/Weapon Sounds/DoomShotgun", typeof(AudioClip));
        damage = 75f;
        range = 12f;
        isAutomatic = false;
        rateOfFire = 1.05f;
        bulletsPerShot = 5;
        spreadRadius = 0.2f;
    }

    protected override void UpdateWeapon()
    {
        if (inputManager.PlayerSingleShot())
        {
            Shoot(fireFX, damage, range, rateOfFire, spreadRadius);
        }
    }

    // public override void Shoot(AudioClip fireFX, float damage, float range, float rateOfFire, float spread)
    // {
        
    // }
}
