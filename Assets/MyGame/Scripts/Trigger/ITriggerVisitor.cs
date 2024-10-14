public interface ITriggerVisitor 
{
    // オブジェクト系統
    void Take(PlayerTrigger damage) { }

    // ダメージ系統
    void Take(DamageBase damage) { }
    void Take(RockBusterDamage damage) { Take(damage as DamageBase); }
}

public interface ITriggerVisitable
{
    void Accept(ITriggerVisitor visitor);
}
