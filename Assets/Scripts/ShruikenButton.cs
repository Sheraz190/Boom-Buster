using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShruikenButton : MonoBehaviour
{
    #region Variables
    [SerializeField] private Image coolDownImage;
    [SerializeField] private GameObject shruikenButton;
    #endregion
    private void Start()
    {
        ColorBlock cb = shruikenButton.GetComponent<Button>().colors;
        cb.disabledColor = cb.normalColor;
        shruikenButton.GetComponent<Button>().colors = cb;
    }

    public void OnShruikenButtonClick()
    {
        shruikenButton.GetComponent<Button>().interactable = false;
        coolDownImage.fillAmount = 1.0f;
        StartCoroutine(CooldownReverse());
    }

    private IEnumerator CooldownReverse()
    {
        float timer = 2.0f;
        coolDownImage.fillAmount = 1f;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            coolDownImage.fillAmount = timer / 2.0f;
            yield return null;
        }
        coolDownImage.fillAmount = 0f;
        shruikenButton.GetComponent<Button>().interactable = true;
    }
}
