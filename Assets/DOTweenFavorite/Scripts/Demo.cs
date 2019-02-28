using UnityEngine;

public class Demo : MonoBehaviour
{
    public void OpenDialog()
    {
        Dialog.Create().SetMessage("君の夢が").Open(() => Debug.Log("完了しまうま"));
    }
}