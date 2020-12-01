using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace OutlineShaderScript
{

    using PlaneListPointMap = Dictionary<Vector3, List<Plane>>;

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CBOutlineShaderScript : MonoBehaviour
    {
        [SerializeField]
        private Color outlineColor = Color.red;

        [SerializeField]
        [Range(0.01f, 1.0f)]
        private float outlineWidth = 0.1f;

        //  compute buffer
        struct VertexExpansion
        {
            public Vector3 normal;
        }
        private VertexExpansion[] vertexExpansionArray;
        private ComputeBuffer computeBuffer;

        //  command buffer
        private CommandBuffer commandBuffer;
        private Material outlineMaterial;


        //---------------------------------------------------------------------
        /**
         * @brief 要点は頂点とポイントの区別。
         *        ポイントは面を構成するためにインデックスに従って選ばれた頂点。
         *        頂点は複数回ポイントに選ばれる可能性がある。
         *        ここでは、頂点に、自身が利用されている平面の法線を重複なく紐づけてそれらを合成して、
         *        頂点の法線を求める。この法線に沿えば、面を分離させずに頂点を動かせる。
         */
        private System.Collections.IEnumerator Start()
        {
            var mesh = GetComponent<MeshFilter>().mesh;

            //  インデックス取得
            List<int> indices = new List<int>();
            mesh.GetIndices(indices, 0);
            int indexCount = indices.Count;

            //  頂点取得
            var vertices = new List<Vector3>();
            mesh.GetVertices(vertices);

            const float ZERO_TOLERANCE = 1e-08f;

            //  共面(与えられた点を全て含む平面)がなければ追加するデリゲートを用意。要は関数。
            /**
             * @param[in] map   頂点と平面配列の連想配列
             * @param[in] pos   mapのkey。頂点を表す。
             * @param[in] plane mapのvalue候補。頂点が含まれる平面(共面)であり、既にmap[pos]に格納されている平面(共面)と一致しない場合は新たに追加する。
             */
            System.Action<PlaneListPointMap, Vector3, Plane> mappingPlane = (map, pos, plane) =>
            {
                List<Plane> planeList;
                if(!map.TryGetValue(pos, out planeList))
                {
                    planeList = new List<Plane>();
                    map[pos] = planeList;
                }

                //  共面がなければ or 既に追加されている共面と一致しなければ、今回の平面を追加
                if(!planeList.Any(dest => {
                    float dot = Vector3.Dot(plane.normal, dest.normal);
                    if(dot >= 1.0f - ZERO_TOLERANCE)    //  法線が一致
                    {
                        if(Mathf.Abs( (dot >= 0) ? (plane.distance - dest.distance) : (plane.distance + dest.distance) ) < ZERO_TOLERANCE)
                        {
                            //すでに追加済みの共面である
                            return true;
                        }
                    }
                    //  まだ追加されていない共面である
                    return false;
                }))
                {
                    planeList.Add(plane);
                }
            };

            //  ポイントが含まれる平面をリストアップする
            var planeListPointMap = new PlaneListPointMap();
            for(int i = 0; i < indexCount; i += 3)
            {
                int index0 = indices[i + 0];
                int index1 = indices[i + 1];
                int index2 = indices[i + 2];

                var pos0 = vertices[index0];
                var pos1 = vertices[index1];
                var pos2 = vertices[index2];

                var plane = new Plane(pos0, pos1, pos2);

                mappingPlane(planeListPointMap, pos0, plane);
                mappingPlane(planeListPointMap, pos1, plane);
                mappingPlane(planeListPointMap, pos2, plane);
            }

            //  平面リストから頂点法線を求める
            var normalMap = new Dictionary<Vector3, Vector3>();
            foreach(var it in planeListPointMap)
            {
                //  頂点が含まれる面の法線を全て合成(加算)
                var planeList = it.Value;
                var normal = Vector3.zero;
                foreach(var plane in planeList)
                {
                    normal += plane.normal;
                }
                normalMap[it.Key] = normal.normalized;
            }

            //  compute bufferにコピーする
            vertexExpansionArray = new VertexExpansion[vertices.Count];
            for(int i = 0; i < vertices.Count; ++i)
            {
                var pos = vertices[i];
                vertexExpansionArray[i].normal = normalMap[pos];
            }

            computeBuffer = new ComputeBuffer(vertexExpansionArray.Length, System.Runtime.InteropServices.Marshal.SizeOf(typeof(VertexExpansion)) );
            computeBuffer.SetData(vertexExpansionArray);

            //  シェーダ作成(インスタンスマテリアル?)
            outlineMaterial = new Material(Shader.Find("Custom/CBOutlineShader"));
            outlineMaterial.SetBuffer("_VertexExpansionArray", computeBuffer);
            outlineMaterial.SetColor("_Color", outlineColor);
            outlineMaterial.SetFloat("_Thickness", outlineWidth);

            //  カメラにアウトライン用フローを追加
            var camera = Camera.main;
            commandBuffer = new CommandBuffer();
            camera.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, commandBuffer);
            commandBuffer.name = "CB Outline";
            commandBuffer.DrawRenderer(
                GetComponent<Renderer>(),
                outlineMaterial,
                0
            );

            yield break;
        }

        //---------------------------------------------------------------------
        //  解放
        private void OnDestroy()
        {
            computeBuffer.Release();
            computeBuffer = null;
            if(outlineMaterial != null)
            {
                Destroy(outlineMaterial);
                outlineMaterial = null;
            }
            commandBuffer.Clear();
            commandBuffer = null;
        }

    }

}
