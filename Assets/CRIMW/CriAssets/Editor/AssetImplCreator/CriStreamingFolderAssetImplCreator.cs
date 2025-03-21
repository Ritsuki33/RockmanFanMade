﻿/****************************************************************************
 *
 * Copyright (c) 2022 CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

/**
 * \addtogroup CRIADDON_ASSETS_INTEGRATION
 * @{
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using System.IO;
using System.Linq;
using UnityEditor.Build.Reporting;
using System;
using System.Security.Cryptography;
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace CriWare.Assets
{
	/**
	 * <summary>"StreamingAssets" DeployType実装クラス</summary>
	 * <remarks>
	 * <para header='説明'>
	 * "StreamingAssets"DeployTypeでの実データインポート処理を定義するクラスです。<br/>
	 * <see cref="CriAssetImporter.implementation"/>に指定することでスクリプトからDeployTypeを指定できます。<br/>
	 * </para>
	 * </remarks>
	 */
	[CriDisplayName("StreamingAssets")]
	public class CriStreamingFolderAssetImplCreator : ICriAssetImplCreator
	{
		public string Description =>
	@"The actual data is loaded from the StreamingAssets folder.
The files are copied to the StreamingAssets folder during the pre-build process.
The original file will be loaded when the Unity Editor is in play mode.
This deploy type cannot be used for assets used as part of additional content distribution.";

		public ICriAssetImpl CreateAssetImpl(AssetImportContext ctx)
		{
			var md5 = MD5.Create();
			var hash = md5.ComputeHash(File.ReadAllBytes(ctx.assetPath));
			md5.Clear();
			var hashStr = BitConverter.ToString(hash).ToLower().Replace("-", "");
			return new CriStreamingFolderAssetImpl(Path.Combine(DirectoryName, $"{Path.GetFileName(ctx.assetPath)}{hashStr}"), new System.IO.FileInfo(ctx.assetPath).Length, AssetDatabase.AssetPathToGUID(ctx.assetPath));
		}

		public static string DirectoryName => CriStreamingAssetsSetting.Instance.deployFolderPath;
	}

	public class CriStreamingFolderAssetsDeployer : IPreprocessBuildWithReport, IPostprocessBuildWithReport
	{
		public int callbackOrder => 0;

		public static void DeployAssets(IEnumerable<CriAssetBase> assets)
		{
			try
			{
				var count = 0;
				foreach (var asset in assets)
				{
					count++;
					EditorUtility.DisplayProgressBar("[CRIWARE] Deploy data for StreamingAssets data", $"{count} / {assets.Count()} assets", (float)count / assets.Count());

	#if UNITY_WEBGL
					if (asset is CriAtomAcfAsset)
						Debug.LogWarning("[CRIWARE] \"StreamingAssets\" Deploy for ACF is not supported in WebGL.");
	#endif

					var srcPath = AssetDatabase.GetAssetPath(asset);
					var dstPath = Path.Combine(Application.streamingAssetsPath, (asset.Implementation as CriStreamingFolderAssetImpl).InternalPath);

					if (File.Exists(dstPath)) continue;

					var dirPath = Path.GetDirectoryName(dstPath);
					if (!Directory.Exists(dirPath))
						Directory.CreateDirectory(dirPath);

					File.Copy(srcPath, dstPath);
					// remove read-only attribute
					var att = File.GetAttributes(dstPath);
					if ((att & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
						File.SetAttributes(dstPath, att & ~FileAttributes.ReadOnly);
				}
			}
			catch
			{
				throw;
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}
		}

		static IEnumerable<CriAssetBase> CollectTargetAssets()
		{
			var pathList = 
				EditorBuildSettings.scenes.Select(s => s.path)
				.Concat(Resources.LoadAll(string.Empty).Select(asset => AssetDatabase.GetAssetPath(asset)))
				.Concat(PlayerSettings.GetPreloadedAssets().Select(asset => AssetDatabase.GetAssetPath(asset)));
			foreach(var path in pathList)
			{
				var assets = AssetDatabase.GetDependencies(path)
					.Select(p => AssetDatabase.LoadAssetAtPath<CriAssetBase>(p))
					.Where(asset => asset != null)
					.Where(asset => !(asset is ICriReferenceAsset) && asset.Implementation is CriStreamingFolderAssetImpl);
				foreach (var asset in assets)
					yield return asset;
			}
		}

		public void OnPreprocessBuild(BuildReport report)
		{
			var assets = CollectTargetAssets().Distinct().ToArray();

			DeployAssets(assets);

			Debug.Log($"[CRIWARE] CriStreamingFolderAssetsDeployer copied {assets.Count()} files to StreamingAssets/{CriStreamingFolderAssetImplCreator.DirectoryName}.");
		}

		public void OnPostprocessBuild(BuildReport report)
		{
			var dirPath = Path.Combine(Application.streamingAssetsPath, CriStreamingFolderAssetImplCreator.DirectoryName);
			if (!Directory.Exists(dirPath)) return;
			Directory.Delete(dirPath, true);
			Debug.Log($"[CRIWARE] CriStreamingFolderAssetsDeployer deleted all contents in {dirPath}.");
		}
	}
}

/** @} */
