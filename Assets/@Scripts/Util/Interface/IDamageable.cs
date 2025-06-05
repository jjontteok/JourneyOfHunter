public interface IDamageable
{
    public void GetDamaged(float damage);

    public float CalculateFinalDamage(float damage, float def);
}
