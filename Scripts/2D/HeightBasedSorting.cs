using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace anogamelib
{
	[RequireComponent(typeof(SortingGroup)), DisallowMultipleComponent]
	public class HeightBasedSorting : MonoBehaviour,IMove2D
	{
		[SerializeField]
		private SortingGroup m_sortingGroup;

		[SerializeField]
		private float m_fPositionScaling = -100;

		private void OnValidate()
		{
			if (m_sortingGroup == null)
			{
				m_sortingGroup = GetComponent<SortingGroup>();
			}
			updateOrder();
		}

		private void Start()
		{
			updateOrder();
		}

		public void OnMove(Vector2 direction, float velocity)
		{
			updateOrder();
		}

		private void updateOrder()
		{
			if (m_sortingGroup != null)
			{
				m_sortingGroup.sortingOrder = (int)(transform.position.y * m_fPositionScaling);
			}
		}

	}
}