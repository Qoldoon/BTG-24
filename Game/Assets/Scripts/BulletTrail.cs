using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [Header("Trail Settings")]
    [SerializeField] private TrailRenderer trail;
    
    [Header("Color Settings")]
    public Color startColor = Color.red;
    public Color endColor = new Color(1f, 0f, 0f, 0f); // Transparent red
    
    [Header("Trail Properties")]
    public float trailTime = 0.2f;    // How long the trail lasts
    public float startWidth = 0.2f;    // Width at the start of trail
    public float endWidth = 0.0f;      // Width at the end of trail
    public Material trailMaterial;      // Material for the trail

    void Start()
    {
        // Get or add TrailRenderer component
        if (trail == null)
        {
            trail = gameObject.AddComponent<TrailRenderer>();
        }

        // Configure the trail
        SetupTrail();
    }

    void SetupTrail()
    {
        // Basic properties
        trail.time = trailTime;
        trail.startWidth = startWidth;
        trail.endWidth = endWidth;
        
        // Color gradient setup
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

        // Material setup
        if (trailMaterial != null)
        {
            trail.material = trailMaterial;
        }
        
        // Additional settings for better visual quality
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        trail.receiveShadows = false;
        trail.numCapVertices = 1;       // Smooth trail start
        trail.numCornerVertices = 5;    // Smooth trail corners
        trail.minVertexDistance = 0.1f; // Minimum distance between trail points
    }
}