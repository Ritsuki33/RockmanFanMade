using UnityEngine;
using UnityEngine.Pool;

public class ReusableObject<T> : MonoBehaviour where T : MonoBehaviour
{
    public IObjectPool<T> Pool { get; set; }
}
