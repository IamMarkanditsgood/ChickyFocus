using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lose : BasickScreen
{
    public Button home;

    public override void Subscribe()
    {
        base.Subscribe();
        home.onClick.AddListener(Home);
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        home.onClick.RemoveListener(Home);
    }

    private void Home()
    {
        Hide();
    }
}
