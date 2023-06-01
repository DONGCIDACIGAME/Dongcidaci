using UnityEngine;

public abstract class GameEntityView : MonoBehaviour
{
    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    public void SetRotation(Vector3 rotation)
    {
        this.transform.rotation = Quaternion.Euler(rotation);
    }
}

