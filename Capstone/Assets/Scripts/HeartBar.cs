using UnityEngine;
using System.Collections.Generic;

public class HeartBar : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartContainer;

    private List<GameObject> hearts = new List<GameObject>();

    public static HeartBar Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void SetHearts(int current, int max)
    {
        // Add hearts if needed
        while (hearts.Count < max)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            hearts.Add(heart);
        }
        // Remove extra hearts
        while (hearts.Count > max)
        {
            Destroy(hearts[hearts.Count - 1]);
            hearts.RemoveAt(hearts.Count - 1);
        }
        // Set active/inactive
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < current);
        }
    }
}