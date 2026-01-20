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
        rateOfFire = 0.3f;
        spreadRadius = 0.07f;
        audibleRange = 15f;
    }

    protected override void UpdateWeapon()
    {
        if (inputManager.PlayerSingleShot())
        {
            Shoot(fireFX, damage, range, spreadRadius, rateOfFire, audibleRange);
        }
    }
}