using System.Collections.Generic;
using UnityEditor;

public static class Utilities
{
    /// <summary>
    /// Enumerates the child properties of the given SerializedProperty without recursion.
    /// You don't have to dispose the current property of the enumerator; it will be disposed at the end of the enumeration. However, you should dispose the enumerator itself.
    /// </summary>
    /// <param name="property">The SerializedProperty to enumerate.</param>
    /// <returns>An IEnumerator that iterates through the child SerializedProperties.</returns>
    public static IEnumerator<SerializedProperty> GetNonRecursiveChildEnumerator(this SerializedProperty property)
    {
        // To retain the original value of the property
        using var copied = property.Copy();
        var end = copied.GetEndProperty();

        // Specially handle the first next, because it should enter the child.
        if (copied.NextVisible(true) && !SerializedProperty.EqualContents(copied, end))
            yield return copied;
        else
            yield break;

        while (copied.NextVisible(false) && !SerializedProperty.EqualContents(copied, end))
        {
            yield return copied;
        }
    }
}
