using UnityEngine;
using Scripts.Interfaces;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] protected string interactionPrompt = "Press E to interact";
    [SerializeField] protected bool canInteract = true;
    
    protected Renderer objectRenderer;
    protected Material originalMaterial;
    [SerializeField] protected Material highlightMaterial;
    
    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            originalMaterial = objectRenderer.material;
        }
    }
    
    public virtual string InteractionPrompt => interactionPrompt;
    public virtual bool CanInteract => canInteract;
    
    public abstract void Interact(IPlayer player);
    
    public virtual void Highlight(bool highlight)
    {
        if (objectRenderer != null && highlightMaterial != null)
        {
            objectRenderer.material = highlight ? highlightMaterial : originalMaterial;
        }
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Highlight(true);
        }
    }
    
    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Highlight(false);
        }
    }
} 