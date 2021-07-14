using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace anogamelib
{
	//public class SpriteManagerBase : Singleton<SpriteManager> 
	public abstract class SpriteManagerBase<T> : Singleton<T> where T : Singleton<T>
	{

		public override void Initialize()
		{
			//SetDontDestroy(true);
			base.Initialize();
			m_eStep = STEP.IDLE;
			m_eStepPre = STEP.MAX;
			m_spriteAtlasList.Clear();
			return;
		}

		[System.Serializable]
		public class TTexture2DPair
		{
			public string strSpriteName;
			public Texture2D texture2D;
		}
		public Dictionary<string, Texture2D> m_LoadedTexture2DList = new Dictionary<string, Texture2D>();
		protected bool AddTexture2d(string _strName, Texture2D _texture2d)
		{
			_strName = _strName.Replace(".png", "").Replace(".jpg", "");
			//Debug.LogError (_strName);
			m_LoadedTexture2DList.Add(_strName, _texture2d);
			return true;
		}

		[System.Serializable]
		public class TSpritePair
		{
			public string strSpriteName;
			public Sprite sprSprite;
		}
		public Dictionary<string, Sprite> m_LoadedSpriteList = new Dictionary<string, Sprite>();

		[System.Serializable]
		public class TSpriteAtlas
		{
			public string strAtlasName;
			public Sprite[] sprites;
		}
		/*
		public Sprite SpriteCreate( string _strTexture2d , Rect _rect , Vector2 _v2Pivot ){
			Texture2D t2;
			GetTexture( _strTexture2d , out t2 );
			return Sprite.Create (t2, _rect, _v2Pivot);
		}
		*/
		public string server_url;
		public List<Sprite[]> m_spriteAtlasList = new List<Sprite[]>();
		public bool LoadAtlas(string _strFile)
		{
			bool bRet = false;

			string[] strArr = _strFile.Split('/');
			string strFileName = "";
			if (0 < strArr.Length)
			{
				strFileName = strArr[strArr.Length - 1];
			}

			Sprite[] sprites = Resources.LoadAll<Sprite>(_strFile);
			TSpriteAtlas sprite_atlas = new TSpriteAtlas();
			sprite_atlas.strAtlasName = strFileName;
			sprite_atlas.sprites = sprites;
			m_LoadedAtlasList.Add(sprite_atlas);
			m_spriteAtlasList.Add(sprites);
			return bRet;
		}

		public Sprite GetSprite(string _strName)
		{
			//_strName = _strName.ToLower ();

			Sprite sprite;

			string[] strArr = _strName.Split('/');
			string strFileName = "";
			if (0 < strArr.Length)
			{
				strFileName = strArr[strArr.Length - 1];
			}
			strFileName = strFileName.Replace(".png", "").Replace(".jpg", "");
			//Debug.LogError(strFileName);
			m_LoadedSpriteList.TryGetValue(strFileName, out sprite);

			if (sprite == null)
			{
				m_LoadedSpriteList.TryGetValue(_strName, out sprite);
			}
			/*
			foreach (TSpritePair data in m_LoadedSpriteList) {
				//Debug.Log (data.strSpriteName);
				if (data.strSpriteName.Equals (_strName) == true) {
					return data.sprSprite;
				}
			}
			*/
			return sprite;
		}

		public bool Unload(string _strName)
		{
			if (GetSprite(_strName) != null)
			{

				int iCount = 0;
				/*
				foreach (TSpritePair param in m_LoadedSpriteList) {
					if (param.strSpriteName.Equals (_strName) == true) {
						break;
					} else {
						iCount += 1;
					}
				}
				m_LoadedSpriteList.RemoveAt (iCount);
				*/
				m_LoadedSpriteList.Remove(_strName);
				iCount = 0;

				m_LoadedTexture2DList.Remove(_strName);
				/*
				foreach (TTexture2DPair param in m_LoadedTexture2DList) {
					if (param.strSpriteName.Equals (_strName) == true) {
						break;
					} else {
						iCount += 1;
					}
				}
				m_LoadedTexture2DList.RemoveAt (iCount);
				*/
				iCount = 0;
				foreach (string param in m_strLoadedFilenameList)
				{
					if (param.Equals(_strName) == true)
					{
						break;
					}
					else
					{
						iCount += 1;
					}
				}
				m_strLoadedFilenameList.RemoveAt(iCount);
				return true;
			}
			return false;
		}
		public bool LoadFromFile(out Sprite _sprite, string _strSpriteName)
		{
			bool bRet = false;
			_sprite = null;

			string filename = "";
			//Debug.LogError(_strSpriteName);
			if (System.IO.File.Exists(_strSpriteName))
			{
				bRet = true;
				filename = _strSpriteName;
			}
			if (bRet == false)
			{
				string strPersistent = Application.persistentDataPath + "/" + _strSpriteName;
				//Debug.LogError(strPersistent);
				if (System.IO.File.Exists(strPersistent))
				{
					bRet = true;
					filename = strPersistent;
				}
			}

			if (bRet)
			{
#if !UNITY_WEBPLAYER
				//Debug.LogError ("exist");
				byte[] image = File.ReadAllBytes(filename);
				// Texture2D を作成して読み込み
				Texture2D tex2d = new Texture2D(0, 0);
				tex2d.LoadImage(image);
				_sprite = AddSprite(_strSpriteName, tex2d, Vector2.one * 0.5f);
				AddLoadedFilenameList(_strSpriteName);
				AddTexture2d(_strSpriteName, tex2d);
#endif
			}
			return bRet;
		}

		public IEnumerator LoadTexture2D(string _strFilename, Action<bool> _onFinished)
		{

			// string.Format("texture/atlas/chara/chara{0:D3}_main.png", _iCharaId);
			//bool bRet = true;
			string strPersistent = Application.persistentDataPath + "/" + _strFilename;
			if (System.IO.File.Exists(strPersistent))
			{
				//Debug.LogError ("exist");
#if !UNITY_WEBPLAYER
				//Debug.LogError ("exist");
				byte[] image = File.ReadAllBytes(strPersistent);
				// Texture2D を作成して読み込み
				Texture2D tex2d = new Texture2D(0, 0);
				tex2d.LoadImage(image);

				// 登録ファイル名は拡張子をはずす（かなり雑）
				AddTexture2d(_strFilename, tex2d);
				SpriteManager.Instance.AddLoadedFilenameList(_strFilename);

				_onFinished.Invoke(true);
#endif

			}
			else
			{

				bool bDownloadResult = false;
				yield return FileDownload.Download(server_url, "", _strFilename, (float _progress) =>
				{
				}, (bool _bResult) =>
				{
					Debug.Log(_bResult);
					bDownloadResult = _bResult;
				});

				if (bDownloadResult && System.IO.File.Exists(strPersistent))
				{
					Debug.LogWarning("from server");
					yield return StartCoroutine(LoadTexture2D(_strFilename, _onFinished));
				}
				else
				{
					_onFinished.Invoke(false);
				}


				/*
				// Resorucesのファイルを詠みに行く場合は.pngとかを消す
				_strFilename = _strFilename.Replace (".png", "").Replace (".jpg", "");
				Texture2D tex2d = Resources.Load (_strFilename) as Texture2D;
				if (tex2d != null) {
					AddTexture2d( _strFilename , tex2d );

					SpriteManager.Instance.AddLoadedFilenameList (_strFilename);
				} else {
					bRet = false;
				}
				*/
			}
		}

		public bool LoadFromAtlas(out Sprite _sprite, string _strSpriteName)
		{
			bool bRet = false;
			_sprite = null;

			string[] strArr = _strSpriteName.Split('/');
			string strFileName = "";
			if (0 < strArr.Length)
			{
				strFileName = strArr[strArr.Length - 1];
			}
			//Debug.Log (strFileName);
			//Debug.LogError(m_spriteAtlasList.Count);
			foreach (Sprite[] sprite_list in m_spriteAtlasList)
			{
				_sprite = System.Array.Find<Sprite>(sprite_list, (sprite) => sprite.name.Equals(strFileName));
				if (_sprite != null)
				{
					AddSprite(_strSpriteName, _sprite);
					bRet = true;
					break;
				}
			}
			return bRet;
		}

		public bool LoadFromResources(out Sprite _sprite, string _strSpriteName)
		{
			bool bRet = false;
			string strResources = _strSpriteName.Replace(".png", "").Replace(".jpg", "");
			//Debug.LogError (strResources);
			_sprite = Resources.Load<Sprite>(strResources);
			if (_sprite != null)
			{
				AddSprite(_strSpriteName, _sprite);
				//SpriteManager.Instance.AddLoadedFilenameList(_strSpriteName);
				bRet = true;
			}
			return bRet;
		}

		public Sprite LoadSprite(string _strName)
		{
			Sprite sprRet = null;
			sprRet = GetSprite(_strName);
			if (null != sprRet)
			{
				return sprRet;
			}
			if (LoadFromFile(out sprRet, _strName))
			{
			}
			else if (LoadFromAtlas(out sprRet, _strName))
			{
			}
			else if (LoadFromResources(out sprRet, _strName))
			{
			}

			return sprRet;
		}
		/*
		//private bool texture_loading_wait = false;
		public AssetBundleDownloadProgress download_progress
		{
			private get;
			set;
		}

		public IEnumerator GetTextureAssetbundle(string _strAssetBundleName, string _strTextureName, Action<Texture2D> _onLoadTexture)
		{
			yield return StartCoroutine(AssetBundleManager.Instance.LoadAssetBundle(_strAssetBundleName, ((bool isSuccess, string error) =>
			{
				if (isSuccess)
				{
				//GameObject pref = AssetBundleManager.Instance.GetAssetBundle(strAssetName).LoadAsset(strAssetName + ".prefab", typeof(GameObject)) as GameObject;
				//Debug.Log(_str);
				// アセットバンドル化するプレファブにアンダーバーを入れるとまずいと言うことが判明しました
				// キャッシュクリアができないだけで影響出すぎだろ
				Texture2D tex = AssetBundleManager.Instance.GetAsset<Texture2D>(_strAssetBundleName, _strTextureName);
				//texture_loading_wait = false;
				_onLoadTexture.Invoke(tex);

				}
				else
				{
					Debug.LogError(string.Format("error[assetbundlename:{0} assetname(prefabname):{1}]", _strAssetBundleName, _strTextureName));
				}
			}), (float progress, int fileNum, int fileIndex, bool isComplete, string error) =>
			{
			//Debug.LogError(progress);
			if (download_progress != null)
				{
					download_progress.gameObject.SetActive(true);
					download_progress.Show("データ準備中", progress, fileNum, fileIndex);
				}
			}));





		}
		*/

		/*
		// 読み込めたんなら大丈夫でしょと言う感じの再起呼び出し
		public IEnumerator GetTexture( string _strName , Action<Texture2D> _onLoadTexture ){
			//_strName = _strName.ToLower ();
			//Debug.Log (m_LoadedSpriteList.Count);

			Texture2D tex = null;
			string strUseName = _strName.Replace (".png", "").Replace (".jpg", "");
			bool bRet = m_LoadedTexture2DList.TryGetValue (strUseName, out tex);

			if (bRet)
			{
				_onLoadTexture.Invoke(tex);
			}
			else {
				bool bResult = false;
				yield return StartCoroutine(LoadTexture2D(_strName, (bool _bResult) =>
				{
					bResult = _bResult;
				}));

				if (bResult)
				{
					yield return StartCoroutine(GetTexture(strUseName, _onLoadTexture));
				}
				else
				{
					_onLoadTexture.Invoke(null);
				}
			}
		}
		*/

		// 小文字にするのよくない気がしてきた
		public bool IsExistSprite(string _strName)
		{
			//_strName = _strName.ToLower ();

			string[] strArr = _strName.Split('/');
			string strFileName = "";
			if (0 < strArr.Length)
			{
				strFileName = strArr[strArr.Length - 1];
			}
			strFileName = strFileName.Replace(".png", "").Replace(".jpg", "");
			if (m_LoadedSpriteList.ContainsKey(strFileName))
			{
				return true;
			}
			return m_LoadedSpriteList.ContainsKey(_strName);
			/*
			foreach (TSpritePair data in m_LoadedSpriteList) {
				if (data.strSpriteName.Equals (_strName) == true) {
					return true;
				}
			}
			return false;
			*/
		}

		// 閲覧用
		//public List<TSpritePair> m_LoadedSpriteList = new List<TSpritePair>();
		//public List<TTexture2DPair> m_LoadedTexture2DList = new List<TTexture2DPair>();
		public List<TSpriteAtlas> m_LoadedAtlasList = new List<TSpriteAtlas>();

		public Sprite AddSprite(string _strTextureName, Texture2D _texture2D, Vector2 _pivot)
		{
			/*
			foreach (TTexture2DPair data in m_LoadedTexture2DList) {
				if (data.strSpriteName.Equals (_strTextureName) == true) {
					return null;
				}
			}
			TTexture2DPair insertData = new TTexture2DPair ();
			insertData.strSpriteName = _strTextureName;
			insertData.texture2D = _texture2D;
			m_LoadedTexture2DList.Add (insertData);
			*/
			//Debug.LogError (_strTextureName);

			// texture2dはよそから入れる
			//AddTexture2d (_strTextureName.Replace (".png", "").Replace (".jpg", ""), _texture2D);

			Sprite tempSprite = Sprite.Create(_texture2D, new Rect(0, 0, _texture2D.width, _texture2D.height), _pivot);
			tempSprite.name = string.Format("{0}(fromTexture2D)", _strTextureName);
			AddSprite(_strTextureName, tempSprite);

			return tempSprite;
		}

		public bool AddSprite(string _strAssetBundleName, Sprite _sprite)
		{

			//Debug.LogError (_strAssetBundleName);
			bool bRet = false;
			try
			{
				m_LoadedSpriteList.Add(_strAssetBundleName, _sprite);
			}
			catch
			{
				//catch (System.Exception ex)
				//Debug.LogError(ex);
				//Debug.LogError(_strAssetBundleName);
			}
			bRet = true;
			/*
			foreach (TSpritePair data in m_LoadedSpriteList) {
				if (data.strSpriteName.Equals (_strAssetBundleName) == true) {
					return false;
				}
			}
			//Debug.Log ("insert sprite:" + _strAssetBundleName);
			bRet = true;

			TSpritePair insertData = new TSpritePair ();
			insertData.strSpriteName = _strAssetBundleName;
			insertData.sprSprite = _sprite;

			m_LoadedSpriteList.Add (insertData);
			*/
			return bRet;
		}

		public static void AdjustSquareSize(ref Image _image, Sprite _sprLoaded, float _fOriginalSize)
		{
			if (_image.sprite == null)
			{
				_image.sprite = _sprLoaded;
			}
			float fOriginSize = 100.0f;
			float fScale = 1.0f;
			if (_sprLoaded.textureRect.width < _sprLoaded.textureRect.height)
			{
				fScale = fOriginSize / _sprLoaded.textureRect.height;
			}
			else
			{
				fScale = fOriginSize / _sprLoaded.textureRect.width;
			}
			_image.rectTransform.sizeDelta = new Vector2(_sprLoaded.textureRect.width * fScale, _sprLoaded.textureRect.height * fScale);
		}

		public static void AdjustSquareSize(ref Image _image, float _fOriginalSize)
		{
			AdjustSquareSize(ref _image, _image.sprite, _fOriginalSize);
		}

		public Queue<string> m_LoadFileName = new Queue<string>();
		public string m_strLoadingFileName;
		public List<string> m_strLoadedFilenameList = new List<string>();
		public bool AddLoadedFilenameList(string _strFilename)
		{
			m_strLoadedFilenameList.Add(_strFilename);
			return true;
		}

		public bool LoadAssetBundleQueue(string _strAssetName)
		{
			_strAssetName = _strAssetName.ToLower();
			if (IsExistSprite(_strAssetName) == true)
			{
				return false;
			}
			Debug.LogError(_strAssetName);
			m_LoadFileName.Enqueue(_strAssetName);
			return true;
		}
		public bool IsIdle()
		{
			return true;
			// おそらく待つような仕組みはもう入れないんじゃないかな？
			// ここでもアセットバンドルをロード待ちするようならひつようかもだけど、
			// システム的にどうなの？って気もしてます。
			// dead codeなのでコメントアウト
			/*
			if (0 < m_LoadFileName.Count) {
				return false;
			}
			return (m_eStep == STEP.IDLE);
			*/
		}

		public bool IsLoaded(string _strFilename)
		{
			foreach (string strLoadedFilename in m_strLoadedFilenameList)
			{
				if (_strFilename.Equals(strLoadedFilename))
				{
					return true;
				}
			}
			return false;
		}

		public enum STEP
		{
			NONE = 0,
			IDLE,
			LOAD_START,
			LOADING,
			LOAD_END,
			MAX,
		}
		public STEP m_eStep;
		public STEP m_eStepPre;

		/*
		void Update(){
			bool bInit = false;

			if (m_eStepPre != m_eStep) {
				m_eStepPre  = m_eStep;
				bInit = true;
			}
			switch (m_eStep) {
			case STEP.NONE:
			case STEP.IDLE:
				if (0 < m_LoadFileName.Count) {
					m_eStep = STEP.LOADING;
				}
				break;
			case STEP.LOADING:
				if( bInit ){
					m_strLoadingFileName = m_LoadFileName.Dequeue ();

					m_csUtilAssetBundleSprite.Load (m_strLoadingFileName );
				}
				if( m_csUtilAssetBundleSprite.IsLoaded()){
					m_strLoadedFilenameList.Add(m_strLoadingFileName);
					m_eStep = STEP.IDLE;
				}
				break;

			case STEP.MAX:
			default:
				break;

			}
		}
		*/





	}



}









