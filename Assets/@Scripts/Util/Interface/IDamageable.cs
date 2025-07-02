public interface IDamageable
{
    public void GetDamage(float damage);

    public float CalculateFinalDamage(float damage, float def);
}
