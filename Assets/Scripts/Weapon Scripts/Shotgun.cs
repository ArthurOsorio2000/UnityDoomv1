using UnityEngine;

public class Shotgun : PlayerFirearm
{
    public int shotgunID = 2;

    private void Awake()
    {
        fireFX = (AudioClip) Resources.Load("Sounds/Weapon Sounds/DoomShotgun", typeof(AudioClip));
        damage = 75f;
        range = 12f;
        rateOfFire = 1.05f;
        spreadRadius = 0.2f;
        audibleRange = 20f;
        bulletsPerShot = 12;
    }

    protected override void UpdateWeapon()
    {
        if (inputManager.PlayerSingleShot())
        {
            Shoot(fireFX, damage, range, spreadRadius ,rateOfFire, audibleRange, bulletsPerShot);
        }
    }

    // public override void Shoot(AudioClip fireFX, float damage, float range, float rateOfFire, float spread)
    // {
        
    // }
}
