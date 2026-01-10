using UnityEngine;

public class AssaultRifle : PlayerFirearm
{
    public int AssaultRifleID = 1;

    private void Awake()
    {
        fireFX = (AudioClip) Resources.Load("Sounds/Weapon Sounds/DoomPistol", typeof(AudioClip));
        damage = 10f;
        range = 30f;
        isAutomatic = false;
        rateOfFire = 0.12f;
    }

    protected override void UpdateWeapon()
    {
        if (inputManager.PlayerHoldShot())
        {
            Shoot(fireFX, damage, range, rateOfFire);
        }
    }
}
