using UnityEngine;
using Scripts.Interfaces;

public class PlayerInventory : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private Transform _rightHandTransform; // Точка крепления предмета в правой руке
    [SerializeField] private float _pickupDistance = 2f;    // Дистанция подбора предмета
    [SerializeField] private LayerMask _interactableLayer;  // Слой интерактивных предметов
    
    private GameObject _currentItem;                        // Текущий предмет в руках
    private IInteractable _currentInteractable;            // Интерфейс текущего предмета
    
    private void Update()
    {
        // Подбор предмета правой кнопкой мыши
        if (Input.GetMouseButtonDown(1))
        {
            if (_currentItem == null)
            {
                TryPickupItem();
            }
            else
            {
                DropItem();
            }
        }
        
        // Использование предмета правой левой кнопкой мыши
        if (Input.GetMouseButton(0) && _currentItem != null)
        {
            UseItem();
        }
    }
    
    private void TryPickupItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, _pickupDistance, _interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                PickupItem(hit.collider.gameObject, interactable);
            }
        }
    }
    
    private void PickupItem(GameObject item, IInteractable interactable)
    {
        _currentItem = item;
        _currentInteractable = interactable;
        
        // Отключаем физику и коллайдеры
        Rigidbody rb = _currentItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        Collider col = _currentItem.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        
        // Перемещаем предмет в руку
        _currentItem.transform.SetParent(_rightHandTransform);
        _currentItem.transform.localPosition = Vector3.zero;
        _currentItem.transform.localRotation = Quaternion.identity;
        
        // Вызываем метод OnPickup, если он есть
        FireExtinguisher extinguisher = _currentItem.GetComponent<FireExtinguisher>();
        if (extinguisher != null)
        {
            extinguisher.OnPickup();
        }
    }
    
    private void DropItem()
    {
        if (_currentItem != null)
        {
            // Вызываем метод OnDrop, если он есть
            FireExtinguisher extinguisher = _currentItem.GetComponent<FireExtinguisher>();
            if (extinguisher != null)
            {
                extinguisher.OnDrop();
            }
            
            // Включаем физику и коллайдеры
            Rigidbody rb = _currentItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            
            Collider col = _currentItem.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = true;
            }
            
            // Открепляем предмет от руки
            _currentItem.transform.SetParent(null);
            
            // Бросаем предмет перед игроком
            _currentItem.transform.position = transform.position + transform.forward * 1f;
            
            _currentItem = null;
            _currentInteractable = null;
        }
    }
    
    private void UseItem()
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.Interact(GetComponent<IPlayer>());
        }
    }
} 