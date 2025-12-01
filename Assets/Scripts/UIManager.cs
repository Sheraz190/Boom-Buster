using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Variables
    public static UIManager Instance;
    [SerializeField] private GameObject _enterDoorButton;
    [SerializeField] private GameObject buttons;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    public void OpenDoorButton()
    {
        _enterDoorButton.gameObject.SetActive(true);
    }

    public void SetAlllTrue()
    {
        buttons.SetActive(true);
    }

    public void SetAllFalse()
    {
        buttons.SetActive(false);
    }
}
