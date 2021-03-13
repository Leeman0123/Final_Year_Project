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

    public Sprite enableImage, disableImage, selectedImage;

    // Start is called before the first frame update
    void Start()
    {
        itemInstance = this;

        itemOwned[0] = PlayerPrefs.GetInt("spaceshooterInvincibleOwned", 0);
        itemOwned[1] = PlayerPrefs.GetInt("spaceshooterSpecialShootOwned", 5);
        itemOwned[2] = PlayerPrefs.GetInt("spaceshooterDoublecoinOwned", 5);

        for (int i = 0; i <= 2; i++) {
            itemText[i].text = "Owned : " + itemOwned[i];
            itemEnable[i] = false;
            if (itemOwned[i] <= 0) {
                itemButton[i].enabled = false;
                itemButton[i].GetComponent<Image>().sprite = disableImage;
            }
        }
    }

    public void ChangeColor(int itemselected) {
        if (!itemEnable[itemselected]) {
            itemEnable[itemselected] = true;
            itemButton[itemselected].GetComponent<Image>().sprite = selectedImage;
            Debug.Log(itemButton[itemselected].GetComponentInChildren<Text>().text + " Enable");
        } else {
            itemEnable[itemselected] = false;
            itemButton[itemselected].GetComponent<Image>().sprite = enableImage;
            Debug.Log(itemButton[itemselected].GetComponentInChildren<Text>().text + " Disable");
        }
    }
}
