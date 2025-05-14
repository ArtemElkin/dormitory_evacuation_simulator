using UnityEngine;
using Scripts.Interfaces;

public class FireExtinguisher : BaseInteractable
{
    [SerializeField] private float extinguishingPower = 10f;
    [SerializeField] private float useDuration = 10f;
    [SerializeField] private ParticleSystem extinguisherEffect;
    
    private bool isBeingUsed = false;
    private float currentUseTime = 0f;
    
    protected override void Start()
    {
        base.Start();
        if (extinguisherEffect != null)
        {
            extinguisherEffect.Stop();
        }
    }
    
    public override void Interact(IPlayer player)
    {
        if (!isBeingUsed)
        {
            StartExtinguishing();
        }
    }
    
    private void Update()
    {
        if (isBeingUsed)
        {
            currentUseTime += Time.deltaTime;
            
            if (currentUseTime >= useDuration)
            {
                StopExtinguishing();
            }
            
            // Проверяем, есть ли огонь перед игроком
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 5f))
            {
                IFire fire = hit.collider.GetComponent<IFire>();
                if (fire != null)
                {
                    fire.Extinguish(extinguishingPower * Time.deltaTime);
                }
            }
        }
    }
    
    private void StartExtinguishing()
    {
        isBeingUsed = true;
        currentUseTime = 0f;
        if (extinguisherEffect != null)
        {
            extinguisherEffect.Play();
        }
    }
    
    private void StopExtinguishing()
    {
        isBeingUsed = false;
        if (extinguisherEffect != null)
        {
            extinguisherEffect.Stop();
        }
    }
} 