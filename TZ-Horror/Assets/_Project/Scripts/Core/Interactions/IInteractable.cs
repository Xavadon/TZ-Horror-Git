using System;

public interface IInteractable : IOutlinable
{
    void Interact();
    string Name { get; }
    bool IsInteractable { get; }
}
