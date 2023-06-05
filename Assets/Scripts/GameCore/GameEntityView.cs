using UnityEngine;

public abstract class GameEntityView : MonoBehaviour
{
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    public void SetRotation(Vector3 rotation)
    {
        this.transform.rotation = Quaternion.Euler(rotation);
    }

    public virtual void Dispose()
    {
        
    }
}

