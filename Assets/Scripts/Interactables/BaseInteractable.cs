using UnityEngine;
using Scripts.Interfaces;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] protected string _interactionPrompt = "Press E to interact";
    [SerializeField] protected bool _canInteract = true;
    
    protected Renderer _objectRenderer;
    protected Material _originalMaterial;
    [SerializeField] protected GameObject _highlightObject;
    
    protected virtual void Start()
    {
        _objectRenderer = GetComponent<Renderer>();
        if (_objectRenderer != null)
        {
            _originalMaterial = _objectRenderer.material;
        }
    }
    
    public virtual string InteractionPrompt => _interactionPrompt;
    public virtual bool CanInteract => _canInteract;
    
    public abstract void Interact(IPlayer player);
    
    public virtual void Highlight(bool highlight)
    {
        if (_objectRenderer != null && _highlightObject != null)
        {
            _highlightObject.SetActive(highlight);
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