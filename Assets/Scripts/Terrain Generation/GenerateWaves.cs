using UnityEngine;

public class GenerateWaves : MonoBehaviour
{
    [Header("Size")]
    [Range(1, 100)]
    [SerializeField] int xSize;
    [Range(1, 100)]
    [SerializeField] int zSize;

    [Header("Noise Settings")]
    [SerializeField] float xOffset;
    [SerializeField] float zOffset;
    [SerializeField] float xAmp = 1f;
    [SerializeField] float zAmp = 1f;
    [SerializeField] float yAmp = 1f;

    [Header("Multiple Noise Layer Settings")]
    [SerializeField] int octavesCount = 1;
    [SerializeField] float lacunarity = 2f;
    [SerializeField] float persistance = 0.5f;

    [Header("Custom Shader")]
    [SerializeField] Gradient terrainGradient;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Color[] _colors;
    private float _minTerrainHeight;
    private float _maxTerrainHeight;

    private void Start()
    {
        xSize = 30;
        zSize = 30;
    }
    private void FixedUpdate()
    {
        CreateShape();
        UpdateMesh();
    }

    public void CreateShape()
    {
        xOffset += Time.deltaTime;
        zOffset += Time.deltaTime;
        int vert = 0;

        //MESH
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;

        //VERTICES
        _vertices = new Vector3[((xSize) + 1) * ((zSize) + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
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

                i++;
            }
        }

        //TRIANGLES
        _triangles = new int[xSize * zSize * 6];

        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                _triangles[tris + 0] = vert;
                _triangles[tris + 1] = vert + xSize + 1;
                _triangles[tris + 2] = vert + 1;
                _triangles[tris + 3] = vert + 1;
                _triangles[tris + 4] = vert + xSize + 1;
                _triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

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
    }

    public void UpdateMesh()
    {
        _mesh.Clear();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;

        _mesh.RecalculateNormals();
    }
}
