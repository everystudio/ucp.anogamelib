using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace anogamelib
{

	public class FileDownload : MonoBehaviour
	{

		public IEnumerator Download(CsvDownloadParam _downloadParam, Action<float> _onProgress, Action<bool> _onFinished)
		{
			yield return new WaitForEndOfFrame();
			yield return StartCoroutine(Download(_downloadParam.url, _downloadParam.local_path, _downloadParam.filename, _onProgress, _onFinished));
		}


		public static IEnumerator Download(string _strUrl, string _strLocalPath, string _strFilename, Action<float> _onProgress, Action<bool> _onFinished)
		{
			string server_file_path = System.IO.Path.Combine(_strUrl, _strLocalPath);
			server_file_path = System.IO.Path.Combine(server_file_path, _strFilename);

			if (_strLocalPath.Equals("") == false)
			{
				// 拡張子なしファイルがあるので、無理やり拡張子をつける
				EditDirectory.MakeDirectory(_strLocalPath + "/" + _strFilename + ".dummy");
				server_file_path = string.Format("{0}/{1}/{2}", _strUrl, _strLocalPath, _strFilename);
			}
			else
			{
				// ファイル名に階層が入ることもあるので・・・
				EditDirectory.MakeDirectory(_strFilename + ".dummy");
				server_file_path = string.Format("{0}/{1}", _strUrl, _strFilename);
			}

			string persisten_file_path = System.IO.Path.Combine(Application.persistentDataPath, _strLocalPath);
			persisten_file_path = System.IO.Path.Combine(persisten_file_path, _strFilename);

			Debug.Log(server_file_path);
			WWW www = new WWW(server_file_path);

			bool bResult = false;
#if !UNITY_WEBPLAYER
			while (!www.isDone)
			{
				_onProgress(www.progress);
				yield return new WaitForEndOfFrame();
			}
			if (www.error == null)
			{
				bResult = true;
				File.WriteAllBytes(persisten_file_path, www.bytes);
			}
			else
			{
				Debug.LogError(www.error);
				Debug.LogError(_strUrl);
				Debug.LogError(_strLocalPath);
				Debug.LogError(_strFilename);
			}
#endif

			_onFinished.Invoke(bResult);
			yield return 0;
		}
	}

}













