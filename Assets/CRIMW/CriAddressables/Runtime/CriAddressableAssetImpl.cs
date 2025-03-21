﻿/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ADDRESSABLES_INTEGRATION
 * @{
 */

#if CRI_USE_ADDRESSABLES

using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace CriWare.Assets
{
	/**
	 * <summary>Addressables 配置の実データ格納先</summary>
	 */
	public class CriAddressableAssetImpl : ICriFileAssetImpl
	{
		[SerializeField]
		string fileName;
		[SerializeField]
		internal CriAddressablesAnchor anchor;
		[SerializeField]
		string _originalId;
		[SerializeField]
		internal string _bundleName;

		public CriAddressableAssetImpl(string fileName, CriAddressablesAnchor anchor, string originalId, string bundleName)
		{
			this.fileName = fileName;
			this.anchor = anchor;
			this._originalId = originalId;
			this._bundleName = bundleName;
		}

		string CachePath { get {
				var path = CriAddressables.Filename2CachePath(_bundleName ?? fileName);
				if (string.IsNullOrEmpty(path))
					throw new System.Exception($"[CRIWARE] cache path not found. This resource may have to rebuild. ({_bundleName ?? fileName})");
#if UNITY_ANDROID && !UNITY_EDITOR
				if (path.Contains(Application.streamingAssetsPath))
				{
					path = path.Replace(Application.streamingAssetsPath, string.Empty);
					while (path[0] == '/')
						path = path.Remove(0, 1);
				}
#endif
				return path;
			}
		}

		public string Path =>
#if UNITY_EDITOR
		anchor == null ?
			CachePath :
			System.IO.Path.GetFullPath(AssetDatabase.GUIDToAssetPath(_originalId));
#else
		anchor == null ?
			CachePath :
			throw new System.Exception($"[CRIWARE] CRI Addressables is being referenced from built-in resources. ({fileName})\nPlease access it using Addressables API.");
#endif

		public bool HasCache => File.Exists(CachePath);

		public void ClearCache()
		{
			if (HasCache)
				File.Delete(CachePath);
		}

		public void OnEnable() { }

		public void OnDisable() { }

		public ulong Offset => 0;

		public long Size =>
#if UNITY_EDITOR
		anchor == null ?
			CriAddressables.Filename2CahceSize(_bundleName ?? fileName) :
			new FileInfo(Path).Length;
		
#else
		CriAddressables.Filename2CahceSize(_bundleName ?? fileName);
#endif

		public bool IsReady => true;
	}


}

#endif

/** @} */
