using UnityEngine;

public class AssaultRifle : PlayerFirearm
{
    public int AssaultRifleID = 1;
    [SerializeField] private AudioClip assaultRifleFireFX;
    [SerializeField] private float assaultRifleDamage = 10f;
    [SerializeField] private float assaultRifleRange = 100f;
    [SerializeField] private bool assaultRifleIsAutomatic = true;
    [SerializeField] private float assaultRifleRateOfFire = 0.12f;

    protected override AudioClip fireFX => assaultRifleFireFX;
    protected override float damage => assaultRifleDamage;
    protected override float range => assaultRifleRange;
    protected override bool isAutomatic => assaultRifleIsAutomatic;
    protected override float rateOfFire => assaultRifleRateOfFire;

    protected override void UpdateWeapon()
    {
        if (inputManager.PlayerHoldShot())
        {
            Shoot(fireFX, damage, range, rateOfFire);
        }
    }
}
