using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace anogamelib
{
	[RequireComponent(typeof(Saveable))]
	public class SaveablePrefabInstanceManager : MonoBehaviour , ISaveable
	{
		[SerializeField]
		private IntReference m_currentSaveSlotIndex;
		private bool m_bSaveable;

		[System.Serializable]
		public class SaveablePrefabData
		{
			public string trimmedguid;
			public string prefabGUID;
			public Dictionary<Saveable, string> saveableGUIDS = new Dictionary<Saveable, string>();
		}
		public Dictionary<SaveablePrefab, SaveablePrefabData> m_saveReferences = new Dictionary<SaveablePrefab, SaveablePrefabData>();
		private bool m_destroyingScene = false;
		private bool m_quittingGame = false;

		private void OnEnable()
		{
			SceneManager.activeSceneChanged += SceneChange;
			SceneManager.sceneUnloaded += SceneUnload;
			Application.quitting += IsQuittingGame;
		}
		private void OnDestroy()
		{
			foreach (SaveablePrefabData data in m_saveReferences.Values)
			{
				data.saveableGUIDS.Clear();
			}
			m_saveReferences.Clear();

			SceneManager.activeSceneChanged -= SceneChange;
			SceneManager.sceneUnloaded -= SceneUnload;
			Application.quitting -= IsQuittingGame;
		}
		private void OnApplicationQuit()
		{
			m_quittingGame = true;
		}
		private void IsQuittingGame()
		{
			m_quittingGame = true;
		}
		private void SceneUnload(Scene arg0)
		{
			if (arg0 == this.gameObject.scene)
			{
				m_destroyingScene = true;
			}
		}
		private void SceneChange(Scene arg0, Scene arg1)
		{
			if (arg0 == this.gameObject.scene)
			{
				m_destroyingScene = true;
			}
		}


		public void AddListener(Saveable _instance, SaveablePrefab _scriptablePrefab, string _identification)
		{
			SaveablePrefabData prefabData;

			if (!m_saveReferences.TryGetValue(_scriptablePrefab, out prefabData))
			{
				prefabData = new SaveablePrefabData();

				prefabData.prefabGUID = _scriptablePrefab.GetGuid();
				prefabData.trimmedguid = $"{this.gameObject.scene.name}{prefabData.prefabGUID.Substring(0, 4)}";

				m_saveReferences.Add(_scriptablePrefab, prefabData);
			}

			if (!prefabData.saveableGUIDS.ContainsKey(_instance))
			{
				string saveableGUID = (string.IsNullOrEmpty(_identification) ?
					$"I{prefabData.trimmedguid}{prefabData.saveableGUIDS.Count}" :
					_identification);

				_instance.saveIdentification.UseConstant = true;
				_instance.saveIdentification.ConstantValue = saveableGUID;

				m_bSaveable = true;

				prefabData.saveableGUIDS.Add(_instance, saveableGUID);
			}
		}

		public void RemoveListener(Saveable _instance, SaveablePrefab _scriptablePrefab)
		{
#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return;
			}
#endif
			if (m_destroyingScene || m_quittingGame)
			{
				return;
			}

			SaveablePrefabData data;
			if (m_saveReferences.TryGetValue(_scriptablePrefab, out data))
			{
				_instance.RemoveFromSaveData();
				if (data.saveableGUIDS.Remove(_instance))
				{
					m_bSaveable = true;
				}
				else
				{
					Debug.Log("Tried to remove listener that was never added.");
				}
			}
		}

		[System.Serializable]
		public class SaveData
		{
			[System.Serializable]
			public struct GuidData
			{
				public string prefabGUID;
				public List<string> saveableGUIDs;
			}
			public List<GuidData> dataList = new List<GuidData>();
			public int spawnedInstancesCount;
		}

		public string OnSave()
		{
			m_bSaveable = false;
			SaveData saveData = new SaveData();
			foreach (SaveablePrefabData cachedData in m_saveReferences.Values)
			{
				SaveData.GuidData guidData;

				List<string> saveables = new List<string>();
				foreach (var item in cachedData.saveableGUIDS.Values)
				{
					saveables.Add(item);
				}
				guidData = new SaveData.GuidData()
				{
					prefabGUID = cachedData.prefabGUID,
					saveableGUIDs = saveables
				};
				saveData.dataList.Add(guidData);
			}
			return JsonUtility.ToJson(saveData);
		}

		public void OnLoad(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return;
			}

			SaveGame currentSaveGame = SaveUtility.LoadSave(m_currentSaveSlotIndex.Value);

			if (currentSaveGame == null)
			{
				Debug.Log("Could not find current save");
				return;
			}
			SaveData saveData = JsonUtility.FromJson<SaveData>(data);

			if (saveData.dataList != null && saveData.dataList.Count > 0)
			{
				for (int i = 0; i < saveData.dataList.Count; i++)
				{
					SaveablePrefab saveablePrefab = ScriptableAssetDatabase.GetAsset(saveData.dataList[i].prefabGUID) as SaveablePrefab;

					if (saveablePrefab == null)
					{
						Debug.Log($"Could not find reference in ScriptableAssetDatabase for Saveable Prefab : {saveData.dataList[i].prefabGUID}");
						continue;
					}

					for (int i2 = 0; i2 < saveData.dataList[i].saveableGUIDs.Count; i2++)
					{
						Saveable getSaveable = saveablePrefab.Retrieve<Saveable>(saveData.dataList[i].saveableGUIDs[i2]);
						getSaveable.OnLoadRequest(currentSaveGame);

#if UNITY_EDITOR
						getSaveable.transform.SetParent(this.transform, true);
#endif
					}
				}
			}
		}
		public bool OnSaveCondition()
		{
			return m_bSaveable;
		}

	}
}



