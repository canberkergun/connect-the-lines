using System;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private GemType gemType;
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;
    public GemType GemType => gemType;
    public int X { get; set; }
    public int Y { get; set; }
    
    public void PlayExplosionEffect()
    {
        if (explosionEffect != null)
        {
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

    public void SetHiglight(bool state)
    {
        highlight.SetActive(state);
    }

}

public enum GemType
{
    Red,
    Blue,
    Green
}