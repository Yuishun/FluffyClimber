using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AY
{

    public class PostEffect : MonoBehaviour
    {
        [SerializeField]
        private Material outline;

        private Camera cam;

        private void Start()
        {
            cam = GetComponent<Camera>();

            //  Unityに_CameraDepthNormalsTextureを生成させるためにカメラにフラグをセットする
            cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.DepthNormals;
        }

        //  全てのオブジェクトのレンダリング完了後に呼び出されるメソッド
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            //  src画像に第三引数のシェーダを掛けてdstに書き込み。dstが画面に表示される。
            Graphics.Blit(source, destination, outline);
        }
    }

}