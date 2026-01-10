using UnityEngine;

public class AssaultRifle : PlayerFirearm
{
    public int AssaultRifleID = 1;
    protected override AudioClip fireFX => (AudioClip) Resources.Load("Sounds/Weapon Sounds/DoomPistol", typeof(AudioClip));
    protected override float damage => 10f;
    protected override float range => 30f;
    protected override float rateOfFire => 0.12f;
    protected override bool isAutomatic => true;

    protected override void UpdateWeapon()
    {
        if (inputManager.PlayerHoldShot())
        {
            Shoot(fireFX, damage, range, rateOfFire);
        }
    }
}
