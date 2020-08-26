using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlights : MonoBehaviour
{
    public static BoardHighlights Instance { set; get; }

    public GameObject highlightPrefab;
    private List<GameObject> _highlights;

    private void Start()
    {
        Instance = this;
        _highlights = new List<GameObject>();
    }

    private GameObject GetHighlightObject()
    {
        var go = _highlights.Find(g => !g.activeSelf);
        if (go != null) return go;

        go = Instantiate(highlightPrefab);
        _highlights.Add(go);

        return go;
    }

    public void HighlightAllowedMoves(bool[,] moves)
    {
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                if (moves[i, j])
                {
                    var go = GetHighlightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
                }
            }
        }
    }

    public void HideHightlights()
    {
        foreach (var go in _highlights)
            go.SetActive(false);
    }
}