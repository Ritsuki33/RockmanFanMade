using UnityEngine;
using UnityEngine.Pool;

public class ReusableObject: MonoBehaviour 
{
    public IObjectPool<ReusableObject> Pool { get; set; }
}
