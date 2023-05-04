public interface IGameCollider
{
    int GetColliderType();
    void OnColliderEnter(IGameCollider other);
}
