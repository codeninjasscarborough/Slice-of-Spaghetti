using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardGame;
using TMPro;

public class RecipeBook : MonoBehaviour
{
    public GameObject panel;
    public Transform content;
    public TextMeshProUGUI rowPrefab;

    List<GameObject> rows = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);  
    }

    public void OpenBook()
    {
        ClearOldRows();

        Hand myHand = GameManager.Instance.LocalPlayer.Hand;
        List<GuideGroup> groups = GameManager.Instance.GuideManager.GetGuideGroups(myHand);

        foreach (GuideGroup group in groups) 
        {
            MakeRow("---" + group.Tier + ("---"));

            foreach (GuideEntry entry in group.Entries)
            {
                if(entry.Recipe == null) continue;

                string line;

                if(entry.IsDiscovered)
                {
                    line = entry.Recipe.DisplayName;
                }
                else
                {
                    line = "???";
                }

                if (entry.IsCraftable)
                {
                    line = line + " (You can make this now!)";
                }
                MakeRow(line);
            }
        }
        panel.SetActive(true);
    }

    public void CloseBook()
    {
        panel.SetActive(false);
    }

    void MakeRow(string words)
    {
        TextMeshProUGUI row = Instantiate(rowPrefab, content);
        row.text = words;
        rows.Add(row.gameObject);
    }

    void ClearOldRows()
    {
        foreach (GameObject old in rows)
        {
            Destroy(old);
        }
        rows.Clear();
    }
}
