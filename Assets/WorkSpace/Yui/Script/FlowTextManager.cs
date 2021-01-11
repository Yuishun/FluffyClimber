using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowTextManager : MonoBehaviour
{
    public static FlowTextManager instance;

    [SerializeField]
    GameObject flowtextPrefab;

    List<FlowText> FlowTexts = new List<FlowText>();

    public Vector2 basisPos = Vector2.zero;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ResetTexts()
    {
        foreach(var ft in FlowTexts)
        {
            ft.gameObject.SetActive(false);
        }
    }

    public void FlowingText(string str)
    {
        var ftext = GetFlowText();
        ftext.Domove = true;
        ftext.text.text = str;
        ftext.text.color = Color.black;
        ftext.speed = SetSpeed(3, 6);
        ftext.maxtime = 8;
        ftext.rtrans.position = new Vector2(GetRandomX(basisPos.x), GetRandomY(basisPos.y));
        ftext.rtrans.localScale = GetRandomScale(0, 2);
    }

    public void SetBasisX(float x) { basisPos.x = x; }
    float GetRandomX(float x)
    {
        return Random.Range(x, x + 3);
    }
    public void SetBasisY(float y) { basisPos.y = y; }
    float GetRandomY(float n)
    {
        return Random.Range((int)n - 3, (int)n + 4);
    }

    float SetSpeed(float min, float max)
    {
        return Random.Range(min, max);
    }

    Vector2 GetRandomScale(int min,int max)
    {
        if (min < 0) min = 0;
        if (max > 4) max = 4;
        return GetTextScale(Random.Range(min, max + 1));
    }
    public Vector2 GetTextScale(int n)
    {
        Vector2 s = Vector2.one;
        switch (n)
        {
            case 0: // 極小
                s *= 0.01f;
                break;
            case 1: // 小
                s *= 0.02f;
                break;
            case 2: // 中
                s *= 0.04f;
                break;
            case 3: // 大
                s *= 0.06f;
                break;
            case 4: // 極大
                s *= 0.08f;
                break;
        }
        return s;
    }

    public FlowText GetFlowText()
    {
        
        for (int i = 0; i < FlowTexts.Count; i++)
        {
            if (!FlowTexts[i].gameObject.activeSelf)
            {
                FlowTexts[i].gameObject.SetActive(true);
                return FlowTexts[i];
            }
        }

        var obj = Instantiate(flowtextPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
        //obj.SetActive(false);
        var ft = obj.GetComponent<FlowText>();
        ft.Init();          // 初期化
        FlowTexts.Add(ft);
        return FlowTexts[FlowTexts.Count - 1];
    }
}
