using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace anogamelib
{
	public class SpriteHolderRoot : MonoBehaviour
	{

		public SpriteHolder[] array;

		void Start()
		{
			foreach (SpriteHolder holder in array)
			{
				Instantiate(holder.gameObject, gameObject.transform);
			}
		}



	}
}
