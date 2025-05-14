using UnityEngine;

namespace Scripts.Interfaces
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        bool CanInteract { get; }
        
        void Interact(IPlayer player);
        void Highlight(bool highlight);
    }
} 