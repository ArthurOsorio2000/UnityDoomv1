using System.Collections;
using UnityEngine;
public class Handgun : PlayerFirearm
{
    public int handgunID = 0;
    private void Awake()
    {
        
        fireFX = (AudioClip) Resources.Load("Sounds/Weapon Sounds/DoomPistol", typeof(AudioClip));
        damage = 50f;
        range = 100f;
        isAutomatic = false;
        rateOfFire = 0.3f;
        bulletsPerShot = 1f;
        spreadRadius = 0.07f;
    }

    protected override void UpdateWeapon()
    {
        if (inputManager.PlayerSingleShot())
        {
            Shoot(fireFX, damage, range, rateOfFire, spreadRadius);
        }
    }
}