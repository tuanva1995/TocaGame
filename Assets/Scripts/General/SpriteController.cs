using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController
{
    public static Sprite GetUISprite(string spriteName)
    {
        return Resources.Load<Sprite>("Sprites/UI/" + spriteName);
    }

    public static Sprite GetGameplaySprite(string spriteName)
    {
        return Resources.Load<Sprite>("Sprites/Gameplay/" + spriteName);
    }
}
