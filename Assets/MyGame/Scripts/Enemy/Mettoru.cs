using UnityEngine;

public class Mettoru : MonoBehaviour
{

    [SerializeField] MettoruController mettoruController;

    public bool IsDeath => !this.gameObject.activeSelf;
    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        mettoruController.Init();
    }

    public void Disapper()
    {
        this.gameObject.SetActive(false);
    }

    public void Revive()
    {
        this.gameObject.SetActive(true);
    }
}
