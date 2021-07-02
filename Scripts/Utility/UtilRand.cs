using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class UtilRand : MonoBehaviour {

	public static int GetIndex( int [] _intParamArr ){

		int intRet;

		int intParam = 0;
		for( int i = 0 ; i < _intParamArr.Length ; i++ ){
			intParam += _intParamArr[i];
		}
		int intRand = UnityEngine.Random.Range(0, intParam);

		for( intRet = 0 ; intRet < _intParamArr.Length ; intRet++ ){
			int intProb = _intParamArr[intRet];
			if( intRand < intProb ){
				break;
			}
			else {
				intRand -= intProb;
			}
		}
		return intRet;
	}

	public static T GetParam<T>(ref List<T> _paramList , string _strProbField)
	{
		int[] probArr = new int[_paramList.Count];
		for( int i=0; i < _paramList.Count; i++)
		{
			FieldInfo field_info = _paramList[i].GetType().GetField(_strProbField);
			probArr[i] = (int)field_info.GetValue(_paramList[i]);
		}
		int index = UtilRand.GetIndex(probArr);
		return _paramList[index];
		//return  UtilRand.GetIndex(probArr);
		//return UtilRand.GetIndex(probArr);
	}

	public static int GetRand( int _iMax , int _iMin = 0 ){

		if (_iMax < _iMin) {
			return 0;
		}

		int iRet = UnityEngine.Random.Range(_iMin,_iMax);

		return iRet;
	}

	public static float GetRange( float _fMax , float _fMin = 0.0f ){

		float fSeido = 1000.0f;

		int iMax = (int)(_fMax * fSeido);
		int iMin = (int)(_fMin * fSeido);

		int iRand = GetRand ( iMax, iMin);

		float fRet = (float)iRand / fSeido;

		return fRet;

	}

}
