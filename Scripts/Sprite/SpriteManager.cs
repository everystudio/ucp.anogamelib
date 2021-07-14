using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// プロジェクトごとにセットしてたけど、全然意味無かったので
// レギュラー復帰
namespace anogamelib
{
	public class SpriteManager : SpriteManagerBase<SpriteManager>
	{

		public Sprite Get(string _strSpriteName)
		{
			foreach (KeyValuePair<string, SpriteHolder> pair in dict_sprite_holder)
			{
				Sprite spr = pair.Value.Get(_strSpriteName);
				if (spr != null)
				{
					return spr;
				}
			}
			return null;
		}

		public Sprite Get(string _strHolderName, string _strSpriteName)
		{
			SpriteHolder sprite_holder = null;
			if (dict_sprite_holder.TryGetValue(_strHolderName, out sprite_holder))
			{
				Sprite spr = sprite_holder.Get(_strSpriteName);

				if (spr != null)
				{
					return spr;
				}
				else
				{
					Debug.LogWarning(string.Format("SpriteHolde[{0}]:not contains [{1}]", _strHolderName, _strSpriteName));
				}
			}
			else
			{
				Debug.LogWarning(string.Format("I have no SpriteHolde[{0}]", _strHolderName));
			}

			return Get(_strSpriteName);
		}


		public bool Add(SpriteHolder _sprite_holder)
		{
			if (dict_sprite_holder.ContainsKey(_sprite_holder.holder_name) == true)
			{
				return false;
			}

			dict_sprite_holder.Add(_sprite_holder.holder_name, _sprite_holder);

			return true;
		}





		private Dictionary<string, SpriteHolder> dict_sprite_holder = new Dictionary<string, SpriteHolder>();

		// アセットバンドル系に依存してます
		/// <summary>
		/// アセットバンドルからMultipleスプライトを読み込む
		/// アトラス名はアセット名と同じにしておいてください
		/// </summary>
		/// 


		public IEnumerator LoadSprite(string _strAssetBundleName, string _strAtlasName, string _strSpriteName, Action<Sprite> _callback)
		{
			yield return null;
			/*
			AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(_strAssetBundleName, _strAtlasName, typeof(Sprite));
			if (request == null)
			{
				Debug.LogWarning("There is no asset with name \"" + _strAtlasName + "\" in " + _strAssetBundleName + ".");
			}

			yield return StartCoroutine(request);

			string strError = "";
			LoadedAssetBundle loadedAssetBundle = AssetBundleManager.GetLoadedAssetBundle(_strAssetBundleName, out strError);
			if(loadedAssetBundle == null)
			{
				Debug.LogError(strError);
			}
			Sprite[] arrAtlasSprites = loadedAssetBundle.m_AssetBundle.LoadAssetWithSubAssets<Sprite>(_strAtlasName);
			Sprite sprRet = Array.Find<Sprite>(arrAtlasSprites, item => item.name == _strSpriteName);
			_callback(sprRet);
			*/
		}

		/*
		public void LoadSprite( Sprite _refSprite , string _strAssetBundleName, string _strAtlasName, string _strSpriteName)
		{
			StartCoroutine(LoadSprite(_strAssetBundleName, _strAtlasName, _strSpriteName , (Sprite s)=>{ _refSprite = s; } ));
		}
		*/







	}
}