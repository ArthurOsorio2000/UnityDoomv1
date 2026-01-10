using System.Collections;
using UnityEngine;
public class Handgun : PlayerFirearm
{
    public int handgunID = 0;

    private void Awake()
    {
        fireFX = (AudioClip) Resources.Load("Sounds/Weapon Sounds/DoomPistol", typeof(AudioClip));
        damage = 25f;
        range = 100f;
        isAutomatic = false;
        rateOfFire = 0.3f;
    }

    protected override void UpdateWeapon()
    {
        if (inputManager.PlayerSingleShot())
        {
            Shoot(fireFX, damage, range, rateOfFire);
        }
    }
}