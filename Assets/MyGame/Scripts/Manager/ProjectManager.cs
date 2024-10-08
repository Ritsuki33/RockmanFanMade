using System.Collections.Generic;
using System.Linq;
using UnityEngine;




[DefaultExecutionOrder(-100)]
public class ProjectManager : SingletonComponent<ProjectManager>
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
