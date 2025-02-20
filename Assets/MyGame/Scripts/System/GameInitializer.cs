using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(StartCo());

        IEnumerator StartCo()
        {
            yield return AudioManager.Instance.Configure();

            SceneManager.Instance.ChangeManager(ManagerType.Title);
        }
    }
}
