using System;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private GemType gemType;
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private SpriteRenderer renderer;
    public GemType GemType => gemType;
    public int X { get; set; }
    public int Y { get; set; }
    
    public void PlayExplosionEffect()
    {
        if (explosionEffect != null)
        {
            Debug.Log("Playing particle system: " + explosionEffect.name);
            explosionEffect.Play();
        }
    }

    public float GetExplosionDuration()
    {
        if (explosionEffect != null)
        {
            return explosionEffect.main.duration;
        }

        return 0f; // Default if no particle system
    }

    public void SetSpriteDisable()
    {
        renderer.enabled = false;
    }
}

public enum GemType
{
    Red,
    Blue,
    Green
}