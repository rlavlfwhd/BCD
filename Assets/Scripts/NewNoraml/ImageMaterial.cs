using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageMaterial : MonoBehaviour
{
    public Texture2D imageTexture; // 인스펙터에서 이미지 넣기

    void Start()
    {
        // 1. Quad 생성
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

        // 2. 새 머티리얼 생성하고 텍스처 연결
        Material material = new Material(Shader.Find("Unlit/Texture"));
        material.mainTexture = imageTexture;

        // 3. 머티리얼 적용
        quad.GetComponent<Renderer>().material = material;

        // 4. 이미지 비율 계산해서 크기 조절
        float aspect = (float)imageTexture.width / imageTexture.height;
        quad.transform.localScale = new Vector3(aspect, 1, 1); // X축 비율 조정

        // 5. 카메라 앞에 배치
        quad.transform.position = new Vector3(0, 0, 5);
    }
}
