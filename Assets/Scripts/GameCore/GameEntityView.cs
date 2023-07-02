using UnityEngine;

public abstract class GameEntityView : MonoBehaviour
{
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public Transform ViewTransform => this.transform;


    public virtual void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    public virtual void SetRotation(Vector3 rotation)
    {
        this.transform.rotation = Quaternion.Euler(rotation);
    }

    public virtual void SetScale(Vector3 scale)
    {
        this.transform.localScale = scale;
    }

    public virtual void OnUpdate(float deltaTime)
    {

    }

    public virtual void OnLateUpdate(float deltaTime)
    {

    }

    public virtual void Dispose()
    {
        
    }
}

