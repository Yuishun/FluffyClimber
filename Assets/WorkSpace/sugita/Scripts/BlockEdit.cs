using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockEdit : MonoBehaviour
{

    public GameObject _BlockPrefab;

    readonly float[] BlockData = {
       7.5f,-0.382172f,0f, 8f,0.5f,1f,
       6.5f, 2.282172f,0f, 4f,0.5f,1f,
       2.5f, 3.282172f,0f, 4f,0.5f,1f,
      -1.5f, 4.282172f,0f, 4f,0.5f,1f,
      -5.5f, 5.282172f,0f, 4f,0.5f,1f,
      -9.5f, 6.232172f,0f, 4f,0.5f,1f,
       2.5f, 9.282172f,0f,18f,0.5f,1f,
      -9.5f,12.282172f,0f, 4f,0.5f,1f,
         0f,12.282172f,0f, 4f,0.5f,1f,
       9.5f,12.282172f,0f, 4f,0.5f,1f,
     -4.25f,14.782172f,0f, 4f,0.5f,1f,
      4.25f,14.782172f,0f, 4f,0.5f,1f,
      -9.5f,17.282172f,0f, 4f,0.5f,1f,
       9.5f,17.282172f,0f, 4f,0.5f,1f,
         0f,19.782172f,0f,16f,0.5f,1f,
      5.25f,22.282172f,0f, 6f,0.5f,1f,
     -2.25f,24.782172f,0f, 3f,0.5f,1f,
      3.25f,27.282172f,0f, 1f,0.5f,1f,
     -2.75f,29.782172f,0f,0.5f,0.5f,1f

    };

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < BlockData.Length; i += 6)
        {//Length 配列の長さ 6個でワンセット{position.x,position.y,psition.z,scale.x,scale.y,scale.z}
            GameObject go = Instantiate(_BlockPrefab) as GameObject;
            go.transform.position = new Vector3(BlockData[i], BlockData[i + 1], BlockData[i + 2]);
            go.transform.localScale = new Vector3(BlockData[i + 3], BlockData[i + 4], BlockData[i + 5]);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
