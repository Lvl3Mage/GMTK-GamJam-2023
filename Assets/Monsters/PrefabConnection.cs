using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 [CreateAssetMenu(menuName = "PrefabConnection")]
public class PrefabConnection : ScriptableObject
{
    
	[SerializeField] private GameObject prefab;

	public GameObject GetPrefab() {
		return prefab;
	}
}
