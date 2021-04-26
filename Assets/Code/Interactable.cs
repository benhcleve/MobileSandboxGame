using UnityEngine;
public class Interactable : MonoBehaviour

{
    public bool isInteracting = false;
    public float interactDistance = 1;
    public virtual void Interact() { isInteracting = true; }

    private void Update()
    {
        if (PlayerInteraction.instance.target != this.gameObject)
            isInteracting = false;
    }
}
