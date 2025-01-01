using UnityEngine;
using UnityEngine.Pool;

interface IResuable
{
    void OnGet();
    void OnRelease();
    void OnDispose();
}
public class Reusable : MonoBehaviour, IResuable
{
    public IObjectPool<Reusable> Pool { get; set; }

    void IResuable.OnGet() { this.gameObject.SetActive(true); OnGet(); }

    void IResuable.OnRelease() { this.gameObject.SetActive(false); OnRelease(); }

    void IResuable.OnDispose() { OnDispose(); Destroy(this.gameObject); }

    protected virtual void OnGet() { }
    protected virtual void OnRelease() { }
    protected virtual void OnDispose() { }

}
