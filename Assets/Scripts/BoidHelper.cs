using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoidHelper
{
    const int numViewDirections = 15;
    public static readonly Vector2[] directions;

    static BoidHelper()
    {
        directions = new Vector2[BoidHelper.numViewDirections];

        var theta = ((Mathf.PI * 2) / numViewDirections);

        for (int i = 0; i < numViewDirections; i++)
        {
            var angle = (theta * i);

            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            directions[i] = new Vector2(x, y);
        }
    }
}
