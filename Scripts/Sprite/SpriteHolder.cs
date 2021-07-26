using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace anogamelib
{
	public class SpriteHolder : MonoBehaviour
	{

		public string holder_name;

		[SerializeField]
		private List<Sprite> sprite_list;

		[System.Serializable]
		public class Data
		{
			public string name;
			public Sprite sprite;

			public Data()
			{

			}
			public Data(string _strName, Sprite _sprite)
			{
				name = _strName;
				sprite = _sprite;
			}
		}

		[SerializeField]
		public List<Data> data = new List<Data>();

		public Sprite Get(string _strName)
		{
			foreach (Data d in data)
			{
				if (d.name.Equals(_strName) == true)
				{
					return d.sprite;
				}
			}
			return null;
		}




		private void Start()
		{
			foreach (Sprite sprite in sprite_list)
			{
				data.Add(new Data(sprite.name, sprite));
			}
			Debug.Log(SpriteManager.Instance);
			SpriteManager.Instance.Add(this);
			gameObject.transform.SetParent(SpriteManager.Instance.gameObject.transform);
			gameObject.name = holder_name;
		}


	}
}