using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;


public static class TextHelper 
{


    public static string FormatStat(StatName stat, float value) {

        //StringBuilder builder = new StringBuilder();
        string bonusColor = ColorUtility.ToHtmlStringRGB(new Color(.439f, .839f, 0.11f));
        string penaltyColor = ColorUtility.ToHtmlStringRGB(new Color(0.839f, 0.235f, 0.11f));

        string result = stat switch {
            StatName.Health => $"<color=#{bonusColor}>+" + value + "</color>",
            StatName.MoveSpeed when value < 0 => $"<color=#{penaltyColor}>" + (Mathf.Abs(value) * 100) + "% </color>",
            StatName.MoveSpeed when value > 0 => $"<color=#{bonusColor}>" + (Mathf.Abs(value) * 100) + "% </color>",
            StatName.RotationSpeed => throw new System.NotImplementedException(),
          
            StatName.DetectionRange => throw new System.NotImplementedException(),
            StatName.Knockback => throw new System.NotImplementedException(),
            StatName.ProjectileLifetime => $"<color=#{bonusColor}>" + (value * 100) + "% </color>",
            StatName.Cooldown when value < 0 => $"<color=#{bonusColor}>" + (value * 100) + "% </color>",
            StatName.Cooldown when value >= 0 => $"<color=#{penaltyColor}>+" + (value * 100) + "% </color>",
            StatName.ShotCount => $"<color=#{bonusColor}>" + value + "</color>",
            StatName.FireDelay => $"<color=#{bonusColor}>" + (value * 100) + "% </color>",
            StatName.Accuracy when value >= 0 => $"<color=#{bonusColor}>" + (value * 100) + "% </color>",
            StatName.Accuracy when value < 0 => $"<color=#{penaltyColor}>" + (value * 100) + "% </color>",
            StatName.StatModifierValue => $"<color=#{bonusColor}>" + value + "</color>",
            StatName.GlobalProjectileSizeModifier when value > 0 => $"<color=#{bonusColor}>+" + (Mathf.Abs(value) * 100) + "% </color>",
            StatName.GlobalProjectileSizeModifier when value < 0 => $"<color=#{penaltyColor}>-" + (Mathf.Abs(value) * 100) + "% </color>",
            StatName.EffectSize when value > 0 => $"<color=#{bonusColor}>" + (Mathf.Abs(value) * 100) + "% </color>",
            StatName.EffectSize when value < 0 => $"<color=#{penaltyColor}>" + (Mathf.Abs(value) * 100) + "% </color>",
            StatName.ProjectileSize when value >= 0 => $"<color=#{bonusColor}>" + (Mathf.Abs(value) * 100) + "% </color>",
            StatName.ProjectileSize when value < 0 => $"<color=#{penaltyColor}>" + (Mathf.Abs(value) * 100) + "% </color>",
            StatName.GlobalProjectileSizeModifier when value >= 0 => $"<color=#{bonusColor}>" + (value) * 100 + "%</color>",
            StatName.GlobalProjectileSizeModifier when value < 0 => $"<color=#{penaltyColor}>" + (value) * 100 + "%</color>",
            StatName.Armor when value >= 0 => $"<color=#{bonusColor}>" + (value) * 100 + "%</color>",
            StatName.Armor when value < 0 => $"<color=#{penaltyColor}>" + (value) * 100 + "%</color>",
            StatName.EffectRange when value >= 0 => $"<color=#{bonusColor}>" + (value) * 100 + "%</color>",
            StatName.EffectRange when value < 0 => $"<color=#{penaltyColor}>" + (value) * 100 + "%</color>",
           
            _ => "No Entry For: " + stat,
        };

        //string rarityText = $"<color=#{bonusColor}>{result}</color>";


        return "<b>" + result + "</b>";

    }




    public static string PretifyStatName(StatName stat) {
        string result = stat switch {
            StatName.Health => stat.ToString(),
            StatName.MoveSpeed => stat.ToString().SplitCamelCase(),
            StatName.RotationSpeed => stat.ToString().SplitCamelCase(),
            StatName.DetectionRange => stat.ToString().SplitCamelCase(),
            StatName.Knockback => stat.ToString(),
            StatName.ProjectileLifetime => stat.ToString().SplitCamelCase(),
            StatName.Cooldown => stat.ToString(),
            StatName.ShotCount => stat.ToString().SplitCamelCase(),
            StatName.FireDelay => stat.ToString().SplitCamelCase(),
            StatName.EffectSize => "Effect Size",
            StatName.EffectRange => "Range",
            StatName.GlobalProjectileSizeModifier => "Projectile Size Modifier",
            StatName.ProjectileSize => "Projectile Size",
            _ => stat.ToString().SplitCamelCase(),
        };

        return result;
    }

    public static string ColorizeText(string text, Color color, float size = 0f) {
        string hexColor = ColorUtility.ToHtmlStringRGB(color);

        string colorizedText = $"<color=#{hexColor}><b>" + text + "</b></color>";

        if(size <= 0f) {
            return colorizedText;
        }
        else {
            return SizeText(colorizedText, size);
        }

        //return $"<color=#{hexColor}><b>" + text + "</b></color>";
    }

    public static string SizeText(string text, float size) {

        return $"<size=#{size}>" + text + "</size>";
    }


    public static string RoundTimeToPlaces(float time, int places) {
        float result = (float)System.Math.Round(time,places);

        return result.ToString();
    }


    public static string SplitCamelCase(this string str) {
        return Regex.Replace(
            Regex.Replace(
                str,
                @"(\P{Ll})(\P{Ll}\p{Ll})",
                "$1 $2"
            ),
            @"(\p{Ll})(\P{Ll})",
            "$1 $2"
        );
    }





}
