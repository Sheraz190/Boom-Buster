using UnityEngine;

public class AddLogger : MonoBehaviour
{
    public static void DisplayLog(string text)
    {
        Debug.Log(text);
    }

    public static void DisplayErrorLog(string text)
    {
        Debug.LogError(text);
    }
}
