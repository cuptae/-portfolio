using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_JusulsoReward : UI_Popup
{
    private UI_Jusulso jusulso;
    public Sprite[] itemSprite;
    public Image itemImage;
    private void Start()
    {
        jusulso = FindObjectOfType<UI_Jusulso>();
        transform.SetParent(jusulso.transform);
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        Vector3 position = rectTransform.localPosition;
        position.z = 0f; // z 축 값을 0으로 설정합니다.
        rectTransform.localPosition = position;
    }

    public void Close()
    {
        Destroy(gameObject);
    }
    public void SetItemImage(string item)
    {
        if(item.Contains("key"))
        {
            itemImage.sprite = itemSprite[1];
        }
        else if(item.Contains("box"))
        {
            itemImage.sprite = itemSprite[0];
        }
        else
        {
            DebugManager.Instance.PrintDebug("아이템을 찾을 수 없습니다");
        }
    }
}
