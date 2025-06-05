using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private NPC[] _npcsEditor;
    [SerializeField] private Transform _playerTransform;

    public event Action OnAllCompleted;

    private Queue<NPC> _npcs = new();

    private void OnEnable()
    {
        foreach (NPC npc in _npcsEditor)
        {
            npc.OnComplete += MoveNextNPC;
        }
    }

    private void OnDisable()
    {
        foreach (NPC npc in _npcsEditor)
        {
            npc.OnComplete -= MoveNextNPC;
        }
    }

    private void Awake()
    {
        int counter = 0;
        foreach (NPC npc in _npcsEditor)
        {
            counter++;
            npc.Construct(counter, _playerTransform);
            _npcs.Enqueue(npc);

            npc.gameObject.SetActive(false);
        }
    }

    public void MoveNextNPC()
    {
        if (_npcs.Count == 0)
        {
            OnAllCompleted?.Invoke();
            return;
        }

        NPC npc = _npcs.Dequeue();
        npc.gameObject.SetActive(true);
        npc.Init();
    }
}
