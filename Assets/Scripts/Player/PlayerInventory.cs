using UnityEngine;
using Scripts.Interfaces;

public class PlayerInventory : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private Transform _rightHandTransform; // Точка крепления предмета в правой руке
    [SerializeField] private float _pickupDistance = 2f;    // Дистанция подбора предмета
    [SerializeField] private LayerMask _pickableLayer;  // Слой интерактивных предметов
    
    private GameObject _currentItem;
    private IPickable _currentPickable;
    
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
        
        // Использование предмета при зажатии левой кнопки мыши
        if (Input.GetMouseButton(0) && _currentItem != null)
        {
            UseItem();
        }
        else if (Input.GetMouseButtonUp(0) && _currentItem != null)
        {
            StopUsingItem();
        }
    }
    
    private void TryPickupItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _pickupDistance, _pickableLayer))
        {
            IPickable pickable = hit.collider.GetComponent<IPickable>();
            if (pickable != null)
            {
                PickupItem(hit.collider.gameObject, pickable);
            }
        }
    }
    
    private void PickupItem(GameObject item, IPickable pickable)
    {
        _currentItem = item;
        _currentPickable = pickable;
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
        pickable.OnPickup();
    }
    
    private void DropItem()
    {
        if (_currentItem != null)
        {
            _currentPickable.OnDrop();
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
            _currentPickable = null;
        }
    }
    
    private void UseItem()
    {
        _currentPickable?.Use();
    }
    
    private void StopUsingItem()
    {
        _currentPickable?.StopUse();
    }
} 