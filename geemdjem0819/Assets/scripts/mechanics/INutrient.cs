namespace mechanics
{
    public interface INutrient
    {
        int Consume(int amount);
        int Replenish(int amount);
    }
}