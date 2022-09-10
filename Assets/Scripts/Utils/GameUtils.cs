using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;

public static class GameUtils
{
    public static string GetTranslateText(string key)
    {
        return LeanLocalization.GetTranslationText(key, key);
    }
}
