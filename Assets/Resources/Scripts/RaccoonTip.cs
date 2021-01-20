using UnityEngine;

public class RaccoonTip : MonoBehaviour
{
    private UIController uiCont;

    public string dialogue;
    public float timer;

    private void Awake()
    {
        uiCont = FindObjectOfType<UIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiCont.GiveTip(dialogue, timer);
            gameObject.SetActive(false);
        }
    }
}
