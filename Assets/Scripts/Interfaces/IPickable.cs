namespace Scripts.Interfaces
{
    public interface IPickable
    {
        void OnPickup();
        void OnDrop();
        void Use();
        void StopUse();
    }
} 