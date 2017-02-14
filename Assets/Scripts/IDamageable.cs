using UnityEngine;

public interface IDamageable
{
    void TakeHit(float dmg, RaycastHit hit);

    void TakeDmg(float dmg);
}