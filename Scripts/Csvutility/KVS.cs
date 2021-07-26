using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace anogamelib {

    public class KVSParam : CsvDataParam
    {
        public KVSParam() { }
        public KVSParam(string _strKey,string _strValue)
        {
            key = _strKey;
            value = _strValue;
        }
        public string key;
        public string value;
    }

    public class KVS : CsvData<KVSParam>
    {
        public bool HasKey(string _strKey )
        {
            return list.Find(p => p.key == _strKey) != null;
        }
        public KVSParam GetParam(string _strKey)
        {
            return list.Find(p => p.key == _strKey);
        }
        public string GetValue(string _strKey)
        {
            return list.Find(p => p.key == _strKey).value;
        }
        public bool SetValue(string _strKey, string _strValue)
        {
            KVSParam param = GetParam(_strKey);
            if( param == null)
            {
                param = new KVSParam() { key = _strKey };
                list.Add(param);
            }
            param.value = _strValue;
            return true;
        }
        public bool Add(string _strKey , string _strValue)
        {
            if (HasKey(_strKey))
            {
                return false;
            }
            KVSParam param = new KVSParam(_strKey, _strValue);
            list.Add(param);
            return true;
        }

        public int GetInt(string _strKey)
        {
            return int.Parse(GetValue(_strKey));
        }
        public bool SetInt(string _strKey , int _iValue)
        {
            return SetValue(_strKey, _iValue.ToString());
        }
        public int AddInt(string _strKey, int _iValue)
        {
            KVSParam param = GetParam(_strKey);
            if( param == null)
            {
                param = new KVSParam() { key = _strKey, value = "0" };
                list.Add(param);
            }
            int temp = int.Parse(param.value);
            temp += _iValue;

            param.value = temp.ToString();
            return temp;
        }
    }
}