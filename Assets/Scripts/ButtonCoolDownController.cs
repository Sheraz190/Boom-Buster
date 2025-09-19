using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonCoolDownController : MonoBehaviour
{
    #region Variables
    [SerializeField] private Image coolDownImage;
    [SerializeField] private Image attackCoolDownImage;
    [SerializeField] private GameObject shruikenButton;
    [SerializeField] private GameObject AttackButton;
    #endregion
    private void Start()
    {
        SetDefaultColor(shruikenButton);
        SetDefaultColor(AttackButton);
    }

    private void MakeButtonCoolDown(GameObject obj, float time,Image cooldownImage)
    {
        obj.GetComponent<Button>().interactable = false;
        cooldownImage.fillAmount = 1.0f;
        StartCoroutine(CooldownReverse(time,cooldownImage,obj));
    }

    private IEnumerator CooldownReverse(float coolDownTime,Image image,GameObject Obj)
    {
        float timer = coolDownTime;
        image.fillAmount = 1f;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            image.fillAmount = timer / coolDownTime;
            yield return null;
        }
        image.fillAmount = 0f;
        Obj.GetComponent<Button>().interactable = true;
    }

    private void SetDefaultColor(GameObject obj)
    {
        ColorBlock cb = obj.GetComponent<Button>().colors;
        cb.disabledColor = cb.normalColor;
        obj.GetComponent<Button>().colors = cb;
    }

    public void SetAttackButtonToCoolDownState()
    {
        MakeButtonCoolDown(AttackButton, 0.5f,attackCoolDownImage);
    }

    public void SetShruikenButtonToCoolDownState()
    {
        MakeButtonCoolDown(shruikenButton, 2.0f,coolDownImage);
    }
}
