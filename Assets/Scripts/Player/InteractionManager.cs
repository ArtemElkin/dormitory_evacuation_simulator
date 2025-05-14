using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scripts.Interfaces;
public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private Image interactionPrompt;
    
    private Camera mainCamera;
    private IInteractable currentInteractable;
    
    private void Start()
    {
        mainCamera = Camera.main;
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(false);
        }
    }
    
    private void Update()
    {
        CheckForInteractable();
        
        if (currentInteractable != null && currentInteractable.CanInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                var player = GetComponent<IPlayer>();
                if (player != null)
                {
                    currentInteractable.Interact(player);
                }
            }
        }
    }
    
    private void CheckForInteractable()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, interactionDistance, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            
            if (interactable != null && interactable.CanInteract)
            {
                if (currentInteractable != interactable)
                {
                    currentInteractable = interactable;
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
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(show);
            interactionText.text = prompt;
        }
        if (interactionPrompt != null)
        {
            interactionPrompt.gameObject.SetActive(show);
        }
    }
    
    private void ClearInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Highlight(false);
            currentInteractable = null;
            ShowInteractionUI(false);
        }
    }
} 