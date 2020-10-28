using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
/// Coroutine for inactive or static class.
/// </summary>
/////////////////////////////////////////////////////////////////////////////////////////////////
public class GlobalCoroutine : MonoBehaviour
{

    // singleton
    private static GlobalCoroutine instance;

    /////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Run the specified routine.
    /// </summary>
    /// <param name="routine">Routine.</param>
    /////////////////////////////////////////////////////////////////////////////////////////////////
    public static Coroutine Run(IEnumerator routine)
    {
        // check and create GameObject.
        if (instance == null)
        {
            GameObject obj = new GameObject();
            obj.name = "GlobalCoroutine";
            instance = obj.AddComponent<GlobalCoroutine>();
            DontDestroyOnLoad(obj);
        }

        return instance.StartCoroutine(instance.routine(routine));
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// execute routine
    /// </summary>
    /// <param name="src">Source.</param>
    /////////////////////////////////////////////////////////////////////////////////////////////////
    private IEnumerator routine(IEnumerator src)
    {
        yield return StartCoroutine(src);
    }
}