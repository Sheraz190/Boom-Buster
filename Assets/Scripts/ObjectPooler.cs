using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    #region Variables
    public static ObjectPooler Instance;
    [SerializeField] private GameObject shruikenPrefab;
    [SerializeField] private Transform shruikenContainer;
    private List<GameObject> _pooledShruikens;
    private int _poolCount = 20;
    #endregion

    private void Awake()
    {
        Instance = this;
        _pooledShruikens = new List<GameObject>();
        PoolObjects();
    }

    private void PoolObjects()
    {
        for (int i = 0; i < _poolCount; i++)
        {
            GameObject obj = Instantiate(shruikenPrefab, shruikenContainer);
            obj.SetActive(false);
            _pooledShruikens.Add(obj);
        }
    }

    public GameObject GetShruiken()
    {
        foreach (GameObject obj in _pooledShruikens)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
    }
}
