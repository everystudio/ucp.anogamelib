using UnityEngine;
using System.Collections;

namespace anogamelib
{
	[System.Serializable]
	public class CsvDownloadParam : CsvDataParam
	{
		public string type { get; set; }
		public int version { get; set; }
		public string filename { get; set; }
		public string local_path { get; set; }
		public string url { get; set; }
		public string comment { get; set; }
	}

	public class CsvDownload : CsvData<CsvDownloadParam>
	{
		/*
		protected override CsvDownloadParam makeParam (System.Collections.Generic.List<SpreadSheetData> _list, int _iSerial, int _iRow)
		{
			SpreadSheetData type = SpreadSheetData.GetSpreadSheet( _list, _iRow , 1);
			SpreadSheetData version= SpreadSheetData.GetSpreadSheet( _list,_iRow , 2 );
			SpreadSheetData filename= SpreadSheetData.GetSpreadSheet( _list,_iRow , 3 );
			SpreadSheetData local_path= SpreadSheetData.GetSpreadSheet( _list,_iRow , 4 );
			SpreadSheetData url= SpreadSheetData.GetSpreadSheet( _list,_iRow , 5 );
			SpreadSheetData comment= SpreadSheetData.GetSpreadSheet( _list,_iRow , 6 );

			CsvDownloadParam retParam = new CsvDownloadParam ();
			retParam.type = type.param;
			retParam.version = int.Parse(version.param);
			retParam.filename = filename.param;
			retParam.local_path = local_path.param;
			retParam.url = url.param;
			retParam.comment = comment.param;
			return retParam;
		}
		*/

		public bool HasItem(CsvDownloadParam _param)
		{
			bool bRet = false;
			foreach (CsvDownloadParam param in list)
			{
				if (_param.local_path == param.local_path && _param.filename == param.filename && _param.version == param.version)
				{
					bRet = true;
					break;
				}
			}
			return bRet;
		}

		public void UpdateFile(CsvDownloadParam _param)
		{
			bool bHit = false;
			foreach (CsvDownloadParam param in list)
			{
				if (_param.local_path == param.local_path && _param.filename == param.filename && _param.version != param.version)
				{
					bHit = true;
					param.version = _param.version;
				}
			}
			if (bHit == false)
			{
				list.Add(_param);
			}
			Save();
		}


	}
}