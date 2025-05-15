using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scripts.Interfaces;
public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float _interactionDistance = 2f;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private TextMeshProUGUI _interactionText;
    [SerializeField] private Image _interactionPrompt;
    
    private Camera _mainCamera;
    private IInteractable _currentInteractable;
    
    private void Start()
    {
        _mainCamera = Camera.main;
        if (_interactionText != null)
        {
            _interactionText.gameObject.SetActive(false);
        }
        if (_interactionPrompt != null)
        {
            _interactionPrompt.gameObject.SetActive(false);
        }
    }
    
    private void Update()
    {
        CheckForInteractable();
        
        if (_currentInteractable != null && _currentInteractable.CanInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var player = GetComponent<IPlayer>();
                if (player != null)
                {
                    _currentInteractable.Interact(player);
                }
            }
        }
    }
    
    private void CheckForInteractable()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, _interactionDistance, _interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            
            if (interactable != null && interactable.CanInteract)
            {
                if (_currentInteractable != interactable)
                {
                    _currentInteractable = interactable;
                    ShowInteractionUI(true, interactable.InteractionPrompt);
                }
            }
            else
            {
                ClearInteraction();
            }
        }
        else
        {
            ClearInteraction();
        }
    }
    
    private void ShowInteractionUI(bool show, string prompt = "")
    {
        if (_interactionText != null)
        {
            _interactionText.gameObject.SetActive(show);
            _interactionText.text = prompt;
        }
        if (_interactionPrompt != null)
        {
            _interactionPrompt.gameObject.SetActive(show);
        }
    }
    
    private void ClearInteraction()
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.Highlight(false);
            _currentInteractable = null;
            ShowInteractionUI(false);
        }
    }
} 