using UnityEngine;
public class Interactable : MonoBehaviour

{
    public bool isInteracting = false;
    public float interactDistance = 1;
    public virtual void Interact() { isInteracting = true; }

    private void Update()
    {
        SetInteractionFalse();
    }

    public void SetInteractionFalse()
    {
        if (PlayerInteraction.instance.target != this.gameObject ||
        Vector3.Distance(PlayerInteraction.instance.transform.position, transform.position) > interactDistance)
            isInteracting = false;
    }

}
