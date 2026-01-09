using UnityEngine;


//I could make a generic weapon class as a parent, and inherit it for each individual weapon?
public class Handgun : PlayerFirearm
{   
    [SerializeField] private AudioClip handgunFireFX;
    protected override AudioClip fireFX => handgunFireFX;
    [SerializeField] private float handgunDamage = 10f;
    protected override float damage => handgunDamage;
    [SerializeField] private float handgunRange = 100f;
    protected override float range => handgunRange;
}
