using System.Collections.Generic;
using System.Linq;
using UnityEngine;




[DefaultExecutionOrder(-100)]
public class ProjectManager : SingletonComponent<ProjectManager>
{
    RuntimeDataHolder rdh = new RuntimeDataHolder();

    public RuntimeDataHolder RDH => rdh;
    private void Start()
    {
        rdh.Initialize();
        DontDestroyOnLoad(gameObject);
    }
}
