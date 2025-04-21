using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageMaterial : MonoBehaviour
{
    public Texture2D imageTexture; // �ν����Ϳ��� �̹��� �ֱ�

    void Start()
    {
        // 1. Quad ����
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);

        // 2. �� ��Ƽ���� �����ϰ� �ؽ�ó ����
        Material material = new Material(Shader.Find("Unlit/Texture"));
        material.mainTexture = imageTexture;

        // 3. ��Ƽ���� ����
        quad.GetComponent<Renderer>().material = material;

        // 4. �̹��� ���� ����ؼ� ũ�� ����
        float aspect = (float)imageTexture.width / imageTexture.height;
        quad.transform.localScale = new Vector3(aspect, 1, 1); // X�� ���� ����

        // 5. ī�޶� �տ� ��ġ
        quad.transform.position = new Vector3(0, 0, 5);
    }
}
