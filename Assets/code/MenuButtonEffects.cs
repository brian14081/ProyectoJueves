using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject shimmerObject;
    public Vector3 hoverScale = new Vector3(1.05f, 1.05f, 1f);
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        if (shimmerObject != null) shimmerObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = hoverScale;
        if (shimmerObject != null) shimmerObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        if (shimmerObject != null) shimmerObject.SetActive(false);
    }
}