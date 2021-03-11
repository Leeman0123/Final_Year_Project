using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelector : MonoBehaviour
{

    public Button[] itemButton;
    public Text[] itemText;

    public static ItemSelector itemInstance;

    public int[] itemOwned;

    public bool[] itemEnable = new bool[3];

    private ColorBlock theColor;

    // Start is called before the first frame update
    void Start()
    {
        itemInstance = this;

        itemOwned[0] = PlayerPrefs.GetInt("spaceshooterInvincibleOwned", 5);
        itemOwned[1] = PlayerPrefs.GetInt("spaceshooterSpecialShootOwned", 5);
        itemOwned[2] = PlayerPrefs.GetInt("spaceshooterDoublecoinOwned", 5);

        for (int i = 0; i <= 2; i++) {
            itemText[i].text = "Owned : " + itemOwned[i];
            itemEnable[i] = false;
            if (itemOwned[i] <= 0) {
                itemButton[i].enabled = false;
            }
        }
    }

    public void ChangeColor(int itemselected) {
        if (!itemEnable[itemselected]) {
            itemEnable[itemselected] = true;
            theColor.normalColor = Color.green;
            theColor.highlightedColor = new Color32(245, 245, 245, 255);
            theColor.pressedColor = new Color32(200, 200, 200, 255);
            theColor.selectedColor = Color.green;
            theColor.disabledColor = new Color32(200, 200, 200, 128);
            theColor.colorMultiplier = 1;
            Debug.Log(itemButton[itemselected].GetComponentInChildren<Text>().text + " Enable");
        } else {
            itemEnable[itemselected] = false;
            theColor.normalColor = new Color32(255, 255, 255, 255);
            theColor.highlightedColor = new Color32(245, 245, 245, 255);
            theColor.pressedColor = new Color32(200, 200, 200, 255);
            theColor.selectedColor = new Color32(255, 255, 255, 255);
            theColor.disabledColor = new Color32(200, 200, 200, 128);
            theColor.colorMultiplier = 1;
            Debug.Log(itemButton[itemselected].GetComponentInChildren<Text>().text + " Disable");
        }
        itemButton[itemselected].colors = theColor;
    }
}
