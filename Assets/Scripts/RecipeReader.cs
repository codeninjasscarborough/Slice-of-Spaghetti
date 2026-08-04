using System.Collections;
using System.Collections.Generic;
using CardGame;
using TMPro;
using UnityEngine;

public class RecipeReader : MonoBehaviour
{
    public RecipeCatalog recipeReader;
    public TMP_Text odoo;

    // Start is called before the first frame update
    void Start()
    { 

        odoo.text = "";

        foreach (var recipe in recipeReader.Entries)
        {
            odoo.text += recipe.recipeName + "\n";
            odoo.text += recipe.guideText + "\n\n";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
