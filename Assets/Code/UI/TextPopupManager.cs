using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopupManager : MonoBehaviour
{
    public GameObject textPopup;
    private static TextPopupManager _instance;
    public static TextPopupManager instance { get { return _instance; } }

    public void Awake() => CreateInstance();
    void CreateInstance() //Make this an instance (Or destroy if already exists)
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }

    public void GeneratePopUp(string popupText, Color32 color, Vector3 size, TextPopup.Effect effect, float duration, Transform target, Vector3 targetOffset = default(Vector3))
    {
        GameObject popup = Instantiate(textPopup, transform.position, transform.rotation);
        popup.transform.SetParent(transform);
        TextPopup txt = popup.GetComponent<TextPopup>();
        txt.popupText = popupText;
        txt.textColor = color;
        txt.size = size;
        txt.duration = duration;
        txt.effect = effect;
        txt.duration = duration;
        txt.target = target;
        txt.targetOffset = targetOffset;
    }
}
