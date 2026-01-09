using UnityEngine;
public class Handgun : PlayerFirearm
{
    [SerializeField] private AudioClip handgunFireFX;
    [SerializeField] private float handgunDamage = 10f;
    [SerializeField] private float handgunRange = 100f;
    [SerializeField] private bool handgunIsAutomatic = true;
    [SerializeField] private float handgunRateOfFire = 0.3f;

    protected override AudioClip fireFX => handgunFireFX;
    protected override float damage => handgunDamage;
    protected override float range => handgunRange;
    protected override bool isAutomatic => handgunIsAutomatic;
    protected override float rateOfFire => handgunRateOfFire;
}