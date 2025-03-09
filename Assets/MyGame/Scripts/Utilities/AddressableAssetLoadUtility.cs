using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressableAssetLoadUtility
{
    public static Dictionary<int, UnityEngine.Object> prefabCache = new Dictionary<int, UnityEngine.Object>();
    /// <summary>
    /// 同期プレハブロード処理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    /// <param name="parent">生成時の親Transform。nullの場合はシーンのルートに生成されます。</param>
    /// <returns></returns>
    public static (T, int) LoadPrefab<T>(string address, Transform parent = null) where T : UnityEngine.Object
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(address);
        var asset = handle.WaitForCompletion();

        if (asset == null)
        {
            Debug.LogError($"プレハブのロードに失敗しました。");
            return (null, 0);
        }
        else
        {
            var obj = UnityEngine.Object.Instantiate(asset, parent);
            if (obj.TryGetComponent<T>(out T component))
            {
                prefabCache.Add(asset.GetInstanceID(), asset);
                return (component, asset.GetInstanceID());
            }
            else
            {
                Debug.LogError($"コンポーネント({typeof(T).ToString()})の取得に失敗しました。");

                // オブジェクトを破棄   
                UnityEngine.Object.Destroy(obj);
                // アセットを解放
                Addressables.Release(asset);

                return (null, 0);
            }
        }
    }

    /// <summary>
    /// 非同期プレハブロード処理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="address"></param>
    /// <returns></returns>
    public static void LoadPrefabAsync<T>(string address, Action<T, int> successed, Action failed = null) where T : UnityEngine.Object
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(address);
        handle.Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var prefab = handle.Result;
                if (prefab.TryGetComponent<T>(out T component))
                {
                    prefabCache.Add(prefab.GetInstanceID(), prefab);
                    successed.Invoke(component, prefab.GetInstanceID());
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
            Debug.LogError($"アセットのロードに失敗しました。{address}");
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
                var asset = handle.Result;
                successed.Invoke(asset);
            }
            else
            {
                Debug.LogError($"アセットのロードに失敗しました。");
                failed?.Invoke();
            }
        };
    }

    public static void ReleasePrefab(int id)
    {
        if (prefabCache.ContainsKey(id))
        {
            Addressables.Release(prefabCache[id]);
            prefabCache.Remove(id);
        }
        else
        {
            Debug.LogError($"キャッシュに存在しません。");
        }
    }

    public static void ReleaseAsset<TClass>(TClass obj) => Addressables.Release(obj);
}
