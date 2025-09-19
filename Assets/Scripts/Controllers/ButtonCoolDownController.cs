using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonCoolDownController : MonoBehaviour
{
    #region Variables
    public static ButtonCoolDownController Instance;
    [SerializeField] private Image shruikenCoolDownImage;
    [SerializeField] private Image attackCoolDownImage;
    [SerializeField] private Image greenCoolDownImage;
    [SerializeField] private Image vanishCoolDownImage;
    [SerializeField] private GameObject shruikenButton;
    [SerializeField] private GameObject attackButton;
    [SerializeField] private GameObject vanishButton;
    #endregion

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
    }
    
    private void Start()
    {
        SetDefaultColor(shruikenButton);
        SetDefaultColor(attackButton);
        SetDefaultColor(vanishButton);
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
        MakeButtonCoolDown(attackButton, 0.5f,attackCoolDownImage);
    }

    public void SetShruikenButtonToCoolDownState()
    {
        MakeButtonCoolDown(shruikenButton, 2.0f,shruikenCoolDownImage);
    }

    public void VanishButtonCoolDown()
    {
        StartCoroutine(OnVanishButton());
    }

    private IEnumerator OnVanishButton()
    {
        MakeButtonCoolDown(vanishButton, 5.0f, greenCoolDownImage);
        Button bt = vanishButton.GetComponent<Button>();
        bt.interactable = false;
        yield return new WaitForSeconds(5.0f);
        MakeButtonCoolDown(vanishButton, 3.0f, vanishCoolDownImage);
    }
}
