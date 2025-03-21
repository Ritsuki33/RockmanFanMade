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

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using System.Linq;
using UnityEditor.AddressableAssets;
using System.IO;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
using UnityEditor.Experimental;
#endif

namespace CriWare.Assets
{
	internal class CriAddressablesEditor : IPreprocessBuildWithReport
	{
#if !CRI_ADDRESSABLES_DISABLE_ANCHOR_ASSET
		internal static event System.Action OnAfterImportCriAsset;

		static bool shouldDestroyUnusedAnchors = false;
		static void ReserveImportCriAssetEvent()
		{
			shouldDestroyUnusedAnchors = true;
			EditorApplication.delayCall += () => {
				if (!shouldDestroyUnusedAnchors) return;
				OnAfterImportCriAsset?.Invoke();
				shouldDestroyUnusedAnchors = false;
			};
		}

		[InitializeOnLoadMethod]
		static void RegisterCallback() =>
			CriAssetImporter.OnCreateImpl += path => ReserveImportCriAssetEvent();

		class ModificationHook : UnityEditor.AssetModificationProcessor
		{
			private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options)
			{
				var impl = AssetDatabase.LoadAssetAtPath<CriAssetBase>(assetPath)?.Implementation;
				if (impl == null) return AssetDeleteResult.DidNotDelete;
				if (!(impl is CriAddressableAssetImpl)) return AssetDeleteResult.DidNotDelete;

				ReserveImportCriAssetEvent();

				return AssetDeleteResult.DidNotDelete;
			}
		}

		[MenuItem("CRIWARE/Reimport All CRI Addressable Assets")]
		public static void RegenerateAllAnchors()
		{
			DeleteAllAnchors();
			ReimportAllCriAddressables();
		}

		static void DeleteAllAnchors()
		{
			var paths = AssetDatabase.FindAssets($"t:{nameof(CriAddressablesAnchor)}")
				.Where(guid => guid != AssetDatabase.FindAssets($"t:{nameof(CriLocalAddressablesAnchor)}").FirstOrDefault())
				.Select(guid => AssetDatabase.GUIDToAssetPath(guid));
			foreach (var path in paths)
				AssetDatabase.DeleteAsset(path);
			AssetDatabase.Refresh();
		}

		static void ReimportAllCriAddressables()
		{
			var assets = AssetDatabase.FindAssets($"t:{nameof(CriAssetBase)}")
				.Select(guid => AssetDatabase.LoadAssetAtPath<CriAssetBase>(AssetDatabase.GUIDToAssetPath(guid)))
				.Where(asset => !(asset is ICriReferenceAsset) && (asset.Implementation is CriAddressableAssetImpl));

			foreach (var asset in assets)
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(asset));

			AssetDatabase.Refresh();
		}
#endif

		[MenuItem("CRIWARE/Validate Builtin Assets")]
		public static void ValidateBuiltinAsset()
		{
			EditorUtility.DisplayProgressBar("[CRIWARE][Addressables] Collectiong dependencies int Built-in assets.", "", 0);

			try
			{
				var assets = EditorUtility.CollectDependencies(EditorBuildSettings.scenes.Select(scene => AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path)).Distinct().ToArray())
					.Where(asset => asset is CriAssetBase).Select(asset => asset as CriAssetBase).Where(asset => !(asset is ICriReferenceAsset) && (asset.Implementation is CriAddressableAssetImpl));
				var resources = EditorUtility.CollectDependencies(UnityEngine.Resources.LoadAll(string.Empty))
					.Where(asset => asset is CriAssetBase).Select(asset => asset as CriAssetBase).Where(asset => !(asset is ICriReferenceAsset) && (asset.Implementation is CriAddressableAssetImpl));
				assets = assets.Concat(resources).Distinct().ToArray();
				foreach (var asset in assets)
					UnityEngine.Debug.LogWarning($"[CRIWARE][Addressables] {asset} is referenced from built-in scene or Resources folder. This is not allowed for CRI Addressable Assets.");
			}
			catch(System.Exception e)
			{
				throw e;
			}
			finally
			{
				EditorUtility.ClearProgressBar();
			}
		}

		public static void DeployData(string srcPath, string dstPath)
		{
			if (!Directory.Exists(Path.GetDirectoryName(dstPath)))
				Directory.CreateDirectory(Path.GetDirectoryName(dstPath));
			if (File.Exists(dstPath))
				File.Delete(dstPath);
			File.Copy(srcPath, dstPath);
			// remove read-only attribute
			var att = File.GetAttributes(dstPath);
			if ((att & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				File.SetAttributes(dstPath, att & ~FileAttributes.ReadOnly);
		}

#if !CRI_ADDRESSABLES_DISABLE_ANCHOR_ASSET
		public static IEnumerable<CriAssetBase> CollectDependentAssets<T>() where T : ICriAssetImpl {
			string[] allCriAssetBasePaths = AssetDatabase.FindAssets($"t:{nameof(CriAssetBase)}")
				.Select(guid => AssetDatabase.GUIDToAssetPath(guid))
				.ToArray();
			string[] allTargetAssetPaths = AddressableAssetSettingsDefaultObject.Settings.groups
				.SelectMany(group => group.entries)
				.SelectMany(entry =>
				{
					var ret = new List<AddressableAssetEntry>();
					entry.GatherAllAssets(ret, true, true, true);
					return ret;
				})
				.Select(entry => entry.AssetPath)
				.ToArray();
			for (int i = 0; i < allTargetAssetPaths.Length; i++) {
				string[] dependencies = AssetDatabase.GetDependencies(allTargetAssetPaths[i], true);
				IEnumerable<CriAssetBase> criAssetBases = dependencies
					.Where(p => allCriAssetBasePaths.Contains(p))
					.Select(p => AssetDatabase.LoadAssetAtPath<CriAssetBase>(p))
					.Where(obj => obj != null)
					.Where(obj => !(obj is ICriReferenceAsset) && (obj.Implementation is T));
				foreach (var obj in criAssetBases) yield return obj;
			}
		}

		public static string CalclateBundleName(AddressableAssetEntry entry) =>
			CalclateBundleName(entry.parentGroup.Guid, entry.address);

		public static string CalclateBundleName(string groupId, string address) =>
			UnityEditor.Build.Pipeline.Utilities.HashingMethods.Calculate(
				(groupId + "_assets_" + address + ".bundle")
				.ToLower().Replace(" ", "").Replace('\\', '/').Replace("//", "/")
			).ToString();
#endif

		public int callbackOrder => 0;
		public void OnPreprocessBuild(BuildReport report)
		{
			ValidateBuiltinAsset();
		}

		static CriLocalAddressablesAnchor _localAnchor = null;
        internal static CriLocalAddressablesAnchor GetLocalAnchor() => _localAnchor ?? (_localAnchor = AssetDatabase.LoadAssetAtPath<CriLocalAddressablesAnchor>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"t:{nameof(CriLocalAddressablesAnchor)} LocalAnchor").FirstOrDefault())));
        [UnityEditor.Callbacks.DidReloadScripts]
        static void ForceLoadLocalAnchor() => GetLocalAnchor();
	}

	internal static class CriAddressablesVersionDependency
	{
		const string dependencyKey = "CriAddressablesVersionKey";
		public static void DependsOnCriAddressablesVersion(this AssetImportContext ctx) =>
			ctx.DependsOnCustomDependency(dependencyKey);

		[UnityEditor.Callbacks.DidReloadScripts]
		static void RegisterDependency()
		{
#if UNITY_2020_3_OR_NEWER
			AssetDatabase.RegisterCustomDependency(dependencyKey, UnityEngine.Hash128.Compute(CriAddressables.VersionString));
#else
			AssetDatabaseExperimental.RegisterCustomDependency(dependencyKey, UnityEngine.Hash128.Compute(CriAddressables.VersionString));
#endif
#if UNITY_2021_3_OR_NEWER
			AssetDatabase.Refresh();
#endif
		}
	}

#if !CRI_ADDRESSABLES_DISABLE_ANCHOR_ASSET
	internal class CriAddressableGroup
	{
		string groupName;
		string templateName;
		CriAddressablesPathPair pathPair;

		public CriAddressableGroup(string name, string temp, CriAddressablesPathPair pathPair)
		{
			if(Settings == null)
			{
				UnityEngine.Debug.LogError("[CRIWARE] Create Addressables Settings before using CRI Addressables.");
				return;
			}
			groupName = name;
			templateName = temp;
			this.pathPair = pathPair;
		}

		public string Name => groupName;

		AddressableAssetGroupTemplate _groupTemplate = null;
		AddressableAssetGroupTemplate GroupTemplate
		{
			get
			{
				if (_groupTemplate == null)
					_groupTemplate = AssetDatabase.LoadAssetAtPath<AddressableAssetGroupTemplate>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"t:AddressableAssetGroupTemplate {templateName}").FirstOrDefault()));
				return _groupTemplate;
			}
		}

		AddressableAssetGroup _criAddressableGroup;

		public AddressableAssetGroup AddressableGroup
		{
			get
			{
				if (_criAddressableGroup == null)
					for (int i = Settings.groups.Count - 1; i >= 0; i--)
						if (Settings.groups[i].name == groupName)
							_criAddressableGroup = Settings.groups[i];
				if (_criAddressableGroup == null) return null;
				_criAddressableGroup.GetSchema<BundledAssetGroupSchema>().BuildPath.SetVariableById(Settings, pathPair.buildPath.Id);
				_criAddressableGroup.GetSchema<BundledAssetGroupSchema>().LoadPath.SetVariableById(Settings, pathPair.loadPath.Id);
				return _criAddressableGroup;
			}
		}

		public AddressableAssetGroup GetOrCrate()
		{
			if(AddressableGroup == null)
				_criAddressableGroup = Settings.CreateGroup(groupName, false, true, true, GroupTemplate.SchemaObjects);
			_criAddressableGroup.GetSchema<BundledAssetGroupSchema>().BuildPath.SetVariableById(Settings, pathPair.buildPath.Id);
			_criAddressableGroup.GetSchema<BundledAssetGroupSchema>().LoadPath.SetVariableById(Settings, pathPair.loadPath.Id);
			return _criAddressableGroup;
		}

		static AddressableAssetSettings Settings => AddressableAssetSettingsDefaultObject.Settings;

		public void ClearGroup()
		{
			for (int i = Settings.groups.Count - 1; i >= 0; i--)
				if (Settings.groups[i].name.Contains(groupName))
					Settings.RemoveGroup(Settings.groups[i]);
			AssetDatabase.SaveAssets();
			_criAddressableGroup = null;
		}
	}
#endif
}

#endif

/** @} */
