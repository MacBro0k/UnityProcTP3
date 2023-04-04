using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropdownScript : MonoBehaviour
{
    [SerializeField] TMP_Dropdown Dropdown;
    public List<Texture2D> Heightmaps;
    private CreateTerrain createTerrain;

    private void Start() {
        createTerrain = GameObject.FindGameObjectWithTag("Terrain").GetComponent<CreateTerrain>();
    }

    void DropdownValueChanged() {
        // Texture2D tex = new Texture2D(Dropdown.itemImage.mainTexture.width, Dropdown.itemImage.mainTexture.height, TextureFormat.DXT5, false);
        // tex.LoadImage(Dropdown.itemImage.mainTexture);
        // _texture.wrapMode = TextureWrapMode.Clamp;

        // GameObject.FindGameObjectWithTag("Terrain").GetComponent<CreateTerrain>().heightmap = Dropdown.itemImage;
    }

    public void DimValueChanged(int value) {
        createTerrain.dimension = value;
    }

}
