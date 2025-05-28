using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ItemManipulator _itemManipulator;
    [SerializeField] private InteractionHighlighter _interactionHighlighter;
    [SerializeField] private PlayerInteractions _playerInteractions;

    public void Construct(ItemInfo itemInfo)
    {
        _playerInteractions.Construct();
        _interactionHighlighter.Construct(itemInfo, _itemManipulator);
    }
}
