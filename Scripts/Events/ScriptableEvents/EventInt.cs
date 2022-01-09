using UnityEngine;
using System.Collections;

namespace anogamelib
{
	[CreateAssetMenu(menuName = "Events/Int Event")]
	public class EventInt : ScriptableEvent<int>
	{
	}
	[CreateAssetMenu(menuName = "Events/GameObject Event")]
	public class EventGameObject : ScriptableEvent<GameObject>
	{
	}
}
