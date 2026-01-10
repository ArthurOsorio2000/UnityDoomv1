using System.Collections;
using UnityEngine;
public class Handgun : PlayerFirearm
{
    public int handgunID = 0;
    protected override AudioClip fireFX => (AudioClip) Resources.Load("Sounds/Weapon Sounds/DoomPistol", typeof(AudioClip));
    protected override float damage => 25f;
    protected override float range => 100f;
    protected override bool isAutomatic => false;
    protected override float rateOfFire => 0.3f;

    protected override void UpdateWeapon()
    {
        if (inputManager.PlayerSingleShot())
        {
            Shoot(fireFX, damage, range, rateOfFire);
        }
    }
}