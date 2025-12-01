using UnityEngine;
using DG.Tweening;
using System.Collections;

public class DoorController : MonoBehaviour
{
    #region Variables
    public static DoorController Instance;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _door;
    #endregion

    void Start()
    {
        OpenDoor();
    }

    private void OpenDoor()
    {
       



        //Sequence gameStartseq = DOTween.Sequence();
        //gameStartseq.Append(_door.transform.DORotate(new Vector3(0, 2, 0), 2.0f));
        //gameStartseq.Append(_door.transform.DOMoveZ(0, 0f));
        //PlayerController.Instance.SetWalkTrue();
        //gameStartseq.Append(_player.transform.DOMoveX(-10, 1.5f));
        //gameStartseq.Join(_door.transform.DORotate(new Vector3(0, 0, 0), 2.0f));
        //gameStartseq.OnComplete(() =>
        //{
        //    PlayerController.Instance.BackToIdleState();
        //});
        //gameStartseq.Play();
    }

    public void CloseDoor()
    {
        Sequence closeDoorseq = DOTween.Sequence();
        closeDoorseq.Append(_door.transform.DORotate(new Vector3(0, 2, 0), 2.0f));
        closeDoorseq.Append(_door.transform.DOMoveZ(-0.05f, 0));
        closeDoorseq.Append(_player.transform.DOMoveX(100, 1.5f));
        closeDoorseq.Append(_door.transform.DORotate(new Vector3(0, 2, 0), 2.0f));
        closeDoorseq.Append(_door.transform.DOMoveZ(0, 0));
        closeDoorseq.Play();
    }
}
