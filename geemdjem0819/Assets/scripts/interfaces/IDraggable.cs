namespace interfaces
{
    public interface IDraggable
    {
        bool IsDraggable { get; set; }
        void SetKinematic(bool startDragging);
    }
}
