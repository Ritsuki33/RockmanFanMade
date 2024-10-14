using UnityEngine;

public class PlayerTrigger : MonoBehaviour, ITriggerVisitable
{
    [SerializeField] PlayerController controller;

    public PlayerController PlayerController => controller;

    public void Accept(ITriggerVisitor visitor)
    {
        visitor.Take(this);
    }
}
