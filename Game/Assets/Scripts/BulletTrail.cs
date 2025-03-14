using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [Header("Trail Settings")]
    [SerializeField] private TrailRenderer trail;
    
    [Header("Color Settings")]
    public Color startColor = Color.red;
    public Color endColor = new Color(1f, 0f, 0f, 0f);
    
    [Header("Trail Properties")]
    public float trailTime = 0.2f;
    public float startWidth = 0.2f;
    public float endWidth = 0.0f;
    public Material trailMaterial;

    void Start()
    {
        if (trail == null)
        {
            trail = gameObject.AddComponent<TrailRenderer>();
        }
        
        SetupTrail();
    }

    void SetupTrail()
    {
        trail.time = trailTime;
        trail.startWidth = startWidth;
        trail.endWidth = endWidth;
        
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(startColor, 0.0f),
                new GradientColorKey(endColor, 1.0f) 
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(1.0f, 0.0f), 
                new GradientAlphaKey(0.0f, 1.0f) 
            }
        );
        trail.colorGradient = gradient;
        
        if (trailMaterial != null)
        {
            trail.material = trailMaterial;
        }
        
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        trail.receiveShadows = false;
        trail.numCapVertices = 1;
        trail.numCornerVertices = 5;
        trail.minVertexDistance = 0.1f;
    }
}