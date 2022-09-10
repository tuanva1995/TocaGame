using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(ScrollRect))]
public class ScrollViewHelper : MonoBehaviour
{
    [SerializeField] bool horizontal;
    [SerializeField] bool vertical;
    [SerializeField] float timeToVisible = 0.5f;
    private ScrollRect scroll;

    private void Awake()
    {
        scroll = GetComponent<ScrollRect>();
    }

    /// <summary>
    /// Make sure the whole rect transform of <paramref name="element"/> fit inside viewport mask
    /// </summary>
    /// <param name="element"></param>
    /// <returns> Amount of time for this action to complete </returns>
    public float MakeElementVisible(RectTransform element)
    {
        StartCoroutine(MakeElementVisibleCoroutine(element));
        return timeToVisible;
    }

    // NOTE: RectTransform local position is calculated from top left corner of its parent transform with rect coordinates
    private IEnumerator MakeElementVisibleCoroutine(RectTransform element)
    {
        if (!element.IsChildOf(scroll.content))
        {
            Debug.LogWarning("This transform is not a child of content!");
            yield break;
        }

        //The returned array of 4 vertices is clockwise.
        //It starts bottom left and rotates to top left, then top right, and finally bottom right

        // 1            2



        // 0            3

        Vector3[] viewportCorners = new Vector3[4];
        scroll.viewport.GetWorldCorners(viewportCorners);

        Vector3[] elementCorners = new Vector3[4];
        element.GetWorldCorners(elementCorners);

        if (horizontal)
        {
            // Too far to the left
            while (elementCorners[0].x < viewportCorners[0].x)
            {
                scroll.horizontalNormalizedPosition += 1/ timeToVisible * Time.deltaTime;
                element.GetWorldCorners(elementCorners);
                yield return null;
            }

            // Too far to the right
            while (elementCorners[2].x > viewportCorners[2].x)
            {
                scroll.horizontalNormalizedPosition -= 1 / timeToVisible * Time.deltaTime;
                element.GetWorldCorners(elementCorners);
                yield return null;
            }
        }
        else if (vertical)
        {
            // Too far to the top
            while (elementCorners[2].y > viewportCorners[2].y)
            {
                scroll.verticalNormalizedPosition += 1 / timeToVisible * Time.deltaTime;
                element.GetWorldCorners(elementCorners);
                yield return null;
            }

            // Too far to the bottom
            while (elementCorners[0].y < viewportCorners[0].y)
            {
                scroll.verticalNormalizedPosition -= 1 / timeToVisible * Time.deltaTime;
                element.GetWorldCorners(elementCorners);
                yield return null;
            }
        }
    }
}
