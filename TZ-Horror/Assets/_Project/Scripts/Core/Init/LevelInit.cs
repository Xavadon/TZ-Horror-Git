using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelInit : MonoBehaviour
{
    [SerializeField] private ItemInfo _itemInfo;
    [SerializeField] private PlayerController _player;

    [Header("Level")]
    [SerializeField] private LevelController _levelController;
    [SerializeField] private TMP_Text _questText;
    [SerializeField] private InteractableDoor _barOpening;
    [SerializeField] private NPCSpawner _spawner;

    private void Awake()
    {
        _itemInfo.Construct();
        _player.Construct(_itemInfo);

        _levelController.Construct(_questText, _barOpening, _spawner);
    }
}
