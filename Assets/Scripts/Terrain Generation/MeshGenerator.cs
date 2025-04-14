using UnityEngine;

//TODO: Implement Level Of Detail feature
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [Header("Update On Change")]
    [SerializeField] bool AutoUpdate;
    public bool autoUpdate { get { return AutoUpdate; } private set { AutoUpdate = value; } }

    [Header("Size")]
    [Range(1, 241)]
    [SerializeField] int xSize;
    [Range(1, 241)]
    [SerializeField] int zSize;

    [Header("Noise Settings")]
    [SerializeField] int xOffset;
    [SerializeField] int zOffset;
    [SerializeField] float xAmp = 1f;
    [SerializeField] float zAmp = 1f;
    [SerializeField] float yAmp = 1f;

    [Header("Multiple Noise Layer Settings")]
    [SerializeField] int octavesCount = 1;
    [SerializeField] float lacunarity = 2f;
    [SerializeField] float persistance = 0.5f;

    [Header("Custom Shader")]
    [SerializeField] Gradient terrainGradient;
    [SerializeField] Material mat;
    [SerializeField] Texture2D gradientTexture;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Color[] _colors;
    private float _minTerrainHeight;
    private float _maxTerrainHeight;

    [Header("Debugging")]
    [SerializeField] GameManager gameManger;
    public void CreateShape()
    {
        //Initialize GameManager - DANGEROUS! -> Find a new way to do this!!
        GameManager.Instance = gameManger;
        //----------------------

        _maxTerrainHeight = 0f;
        _minTerrainHeight = 0f;

        //MESH
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        //VERTICES
        _vertices = new Vector3[((xSize) + 1) * ((zSize) + 1)];

        for (int i = 0, z = 0; z <= zSize; z ++)
        {
            for (int x = 0; x <= xSize; x ++)
            {

                float y = 0;

                for (int o = 0; o < octavesCount; o++)
                {
                    float frequency = Mathf.Pow(lacunarity, o);
                    float amplitude = Mathf.Pow(persistance, o);

                    y += Mathf.PerlinNoise((x + xOffset) * xAmp / 100 * frequency, (z + zOffset) * zAmp / 100 * frequency) * amplitude;
                }

                y *= (yAmp * 2);

                _vertices[i] = new Vector3(x, y, z);

                FindMinMaxHeight(y);

                i++;
            }
        }

        //TRIANGLES
        int vert = 0;
        _triangles = new int[xSize * zSize * 6];

        int tris = 0;

        for (int z = 0; z < zSize; z ++)
        {
            for (int x = 0; x < xSize; x ++)
            {
                _triangles[tris + 0] = vert;
                _triangles[tris + 1] = vert + xSize + 1;
                _triangles[tris + 2] = vert + 1;
                _triangles[tris + 3] = vert + 1;
                _triangles[tris + 4] = vert + xSize + 1;
                _triangles[tris + 5] = vert + xSize + 2;

                vert ++;
                tris += 6;
            }
            vert ++;
        }

        //OCEAN BOUNDS
        if(GetComponentInChildren<OceanWalls>() != null)
        {
            OceanWalls oceanWalls = GetComponentInChildren<OceanWalls>();
            oceanWalls.SetOceanBounds(xSize, zSize, _minTerrainHeight, _maxTerrainHeight);
        }

        //PLANT GENERATION
        if(GetComponentInChildren<PlantGenerator>() != null)
        {
            PlantGenerator plantGenerator = GetComponentInChildren<PlantGenerator>();
            plantGenerator.GeneratePlants(_vertices, (_minTerrainHeight + _maxTerrainHeight) / 2f);
        }

        //TEXTURE
        HandleColors();
    }

    public void UpdateMesh()
    {
        _mesh.Clear();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.colors = _colors;

        _mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = _mesh;
    }

    private void HandleColors()
    {
        mat.SetTexture("terrainGradient", gradientTexture);
        mat.SetFloat("minTerrainHeight", _minTerrainHeight);
        mat.SetFloat("maxTerrainHeight", _maxTerrainHeight);

        //COLORS
        _colors = new Color[_vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(_minTerrainHeight, _maxTerrainHeight, _vertices[i].y);
                _colors[i] = terrainGradient.Evaluate(height);
                i++;
            }
        }

        gradientTexture = new Texture2D(1, 100); //Change Resolution

        Color[] pixelColors = new Color[100];

        for (int i = 0; i < pixelColors.Length; i++)
        {
            pixelColors[i] = terrainGradient.Evaluate((float)i / 100);
        }

        gradientTexture.SetPixels(pixelColors);
        gradientTexture.Apply();
    }

    private void FindMinMaxHeight(float y)
    {
        if (y > _maxTerrainHeight)
        {
            _maxTerrainHeight = y;
        }
        if (y < _minTerrainHeight)
        {
            _minTerrainHeight = y;
        }
    }

}
