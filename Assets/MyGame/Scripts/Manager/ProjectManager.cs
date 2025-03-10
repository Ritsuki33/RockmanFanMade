using System.Collections.Generic;
using System.Linq;
using UnityEngine;




[DefaultExecutionOrder(-100)]
public class ProjectManager : SingletonComponent<ProjectManager>
{
    RuntimeDataHolder rdh = new RuntimeDataHolder();
    [SerializeField] private FooterUI footerUi;

    public RuntimeDataHolder RDH => rdh;
    public FooterUI FooterUi => footerUi;

    private void Start()
    {
        footerUi.gameObject.SetActive(false);
        rdh.Initialize();
        DontDestroyOnLoad(gameObject);
    }
}
