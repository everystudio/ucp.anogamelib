using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace anogamelib {

    public class KVSParam : CsvDataParam
    {
        public string key;
        public string value;
    }

    public class KVS : CsvData<KVSParam>
    {
    }
}