using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressableAssetLoadUtility
{
    /// <summary>
    /// 同期プレハブロード処理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    /// <returns></returns>
    public static T LoadPrefab<T>(string address) where T : MonoBehaviour
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(address);
        var prefab = handle.WaitForCompletion();

        if (prefab == null)
        {
            Debug.LogError($"プレハブのロードに失敗しました。");
            return null;
        }
        else if (prefab.TryGetComponent<T>(out T component))
        {
            return component;
        }
        else
        {
            Debug.LogError($"コンポーネント({typeof(T).ToString()})の取得に失敗しました。");
            return null;
        }
    }

    /// <summary>
    /// 非同期プレハブロード処理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    /// <returns></returns>
    public static void LoadPrefabAsync<T>(string address, Action<T> successed, Action failed = null) where T : MonoBehaviour
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(address);
        handle.Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var prefab = handle.Result;
                if (prefab.TryGetComponent<T>(out T component))
                {
                    successed.Invoke(component);
                }
                else
                {
                    Debug.LogError($"コンポーネント({typeof(T).ToString()})の取得に失敗しました。");
                    failed?.Invoke();
                }
            }
            else
            {
                Debug.LogError($"プレハブのロードに失敗しました。");
                failed?.Invoke();
            }
        };
    }

    /// <summary>
    /// 同期アセットロード処理 (※ Tはコンポーネント以外)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    /// <returns></returns>
    public static T LoadAsset<T>(string address) where T : UnityEngine.Object
    {
        var handle = Addressables.LoadAssetAsync<T>(address);
        var asset = handle.WaitForCompletion();

        if (asset == null)
        {
            Debug.LogError($"アセットのロードに失敗しました。");
        }

        return asset;
    }

    /// <summary>
    /// 非同期プレハブロード処理 (※ Tはコンポーネント以外)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    /// <returns></returns>
    public static void LoadAssetsAsync<T>(string address, Action<T> successed, Action failed = null) where T : UnityEngine.Object
    {
        var handle = Addressables.LoadAssetAsync<T>(address);
        handle.Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var prefab = handle.Result;
                successed.Invoke(prefab);
            }
            else
            {
                Debug.LogError($"アセットのロードに失敗しました。");
                failed?.Invoke();
            }
        };
    }
}
