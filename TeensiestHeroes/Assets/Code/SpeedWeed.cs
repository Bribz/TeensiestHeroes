using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handle generation of all eyecandy pieces. 
/// </summary>
public class SpeedWeed : MonoBehaviour
{

    private MeshFilter m_GrassMeshFilter;
    private Mesh m_GrassMesh;
    public Material m_GrassMaterial;
    private Vector3 m_LastPosition;
    public Vector2 m_BoxSize;
    [Range(1,100000)]
    public int m_ParticleNum = 1000;
    public int m_Seed;
    public float m_StartHeight = 1000;
    public bool SpawnGrass = false;

#if !SERVER
    private void OnEnable()
    {
        m_LastPosition = new Vector3(0, -1000, 0);

        m_GrassMeshFilter = GetComponent<MeshFilter>();
        if (m_GrassMeshFilter == null)
        {
            m_GrassMeshFilter = gameObject.AddComponent<MeshFilter>();
        }

        if(GetComponent<MeshRenderer>() == null)
        {
            var component = gameObject.AddComponent<MeshRenderer>();
            component.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
            component.material = m_GrassMaterial;
            component.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
        }
    }

    private void Update()
    {
        HandleSpawnGrass();
    }

    public void SetWindSpeedStr(float speed = 100f, float strength = 0.05f)
    {
        m_GrassMaterial.SetFloat("_WindSpeed", speed);
        m_GrassMaterial.SetFloat("_WindStrength", strength);
        
    }

    private void HandleSpawnGrass()
    {
        if(m_LastPosition != this.transform.position)
        { 
            Random.InitState(m_Seed);
            List<Vector3> grassPositions = new List<Vector3>(m_ParticleNum);
            int[] grassIndicies = new int[m_ParticleNum];
            List<Color> grassColors = new List<Color>(m_ParticleNum);
            List<Vector3> grassNormals = new List<Vector3>(m_ParticleNum);


            for (int i = 0; i < (m_ParticleNum); ++i)
            {
                Vector3 Origin = transform.position + (Vector3.forward * 10f);
                Origin.y = m_StartHeight;
                Origin.x += m_BoxSize.x * Random.Range(-0.5f, 0.5f);
                Origin.z += m_BoxSize.y * Random.Range(-0.5f, 0.5f);
                Ray ray = new Ray(Origin, Vector3.down);
                RaycastHit hitInfo;
                if (Physics.Linecast(ray.origin, ray.direction * 1300f, out hitInfo, LayerMask.GetMask("Terrain", "Water")))
                {
                    if (hitInfo.transform.gameObject.layer != LayerMask.NameToLayer("Water"))
                    {
                        Origin = hitInfo.point;
                        Origin.x -= this.transform.position.x;
                        Origin.z -= this.transform.position.z;
                        
                        grassPositions.Add(Origin);
                        //float col = Random.Range(.75f, 1f);
                        grassColors.Add(new Color(Random.Range(0.381f, 0.405f), Random.Range(0.578f, 0.598f), Random.Range(0.219f, 0.239f), 1f));
                        grassNormals.Add(hitInfo.normal);
                        
                        //matricies.Add(Matrix4x4.TRS(Origin, Quaternion.Euler(10,0,0), Vector3.one * .5f));
                    }
                }
            }
            grassIndicies = Enumerable.Range(0, grassPositions.Count).ToArray();

            m_GrassMesh = new Mesh();
            m_GrassMesh.SetVertices(grassPositions);
            m_GrassMesh.SetIndices(grassIndicies, MeshTopology.Points, 0);
            m_GrassMesh.SetColors(grassColors);
            m_GrassMesh.SetNormals(grassNormals);
            m_GrassMeshFilter.mesh = m_GrassMesh;

            m_LastPosition = transform.position;
            //m_LastPosition = GameManager.instance.PlayerManager.CLIENT_PLAYER.transform.position; //this.transform.position;

        }
        //Graphics.DrawMeshInstanced(m_GrassMesh, 0, m_GrassMaterial, matricies);
    }
#endif
}
