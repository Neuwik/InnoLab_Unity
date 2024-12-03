using System.Text.RegularExpressions;
using UnityEngine;

public enum EDirection2D { Up = 0, Down = 1, Left = 2, Right = 3 };

public static class Converters // Converters
{
    public static string SplitCamelCase(this string str)
    {
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

    // EDirectionToVector3
    public static Vector3 EDirection2DToVector3(EDirection2D direction)
    {
        switch (direction)
        {
            case EDirection2D.Up:
                return Vector3.forward;

            case EDirection2D.Down:
                return Vector3.back;

            case EDirection2D.Left:
                return Vector3.left;

            case EDirection2D.Right:
                return Vector3.right;

            default:
                return Vector3.zero;
        }
    }
}
