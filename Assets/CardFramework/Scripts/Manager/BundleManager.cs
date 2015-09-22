﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BundleManager : Manager<BundleManager>
{
	private readonly List<AssetBundle> AssetBundleList = new List<AssetBundle>();
	
	private void Awake ()
	{
		if (_currentLevelAssetBundle != null)
		{
			_currentLevelAssetBundle.Unload(false);
			_currentLevelAssetBundle = null;
		}	
	}
	
	public void OnDestroy()
	{
		UnloadAllBundles();
	}	

	private AssetBundle GetBundle(string name)
	{
		for (int i = 0; i < AssetBundleList.Count; ++i)
		{
			if (name == AssetBundleList[i].name)
			{
				return AssetBundleList[i];
			}
		}
		return null;
	}

	public AssetBundle LoadBundle(string path)
	{
		string name = System.IO.Path.GetFileNameWithoutExtension(path);
		AssetBundle assetBundle = GetBundle(name);
		if (assetBundle == null)
		{
			assetBundle = new AssetBundle();
			assetBundle = AssetBundle.CreateFromFile(path);
			assetBundle.name = name;
			AssetBundleList.Add(assetBundle);
			return assetBundle;
		}
		else
		{
			return assetBundle;
		}
	}

	private void UnloadAllBundles()
	{
		for (int i = 0; i < AssetBundleList.Count; ++i)
		{
			AssetBundleList[i].Unload(false);
		}
		AssetBundleList.Clear();
	}
	
	public void LoadLevelAssetBundle(string level)
	{
		string path = DirectoryUtility.ExternalAssets() + level + ".assetBundle";
		Debug.Log("LoadLevelAssetBundle: " + path);
		_currentLevelAssetBundle = AssetBundle.CreateFromFile(path);
		if (_currentLevelAssetBundle != null && Application.CanStreamedLevelBeLoaded(level))
		{
			BundleManager.SharedManager.UnloadAllBundles();
			Application.LoadLevel(level);	
		}
		else
		{
			Debug.Log("AssetBundle Not Found: " + path);
		}
	}
	static private AssetBundle _currentLevelAssetBundle;
}

