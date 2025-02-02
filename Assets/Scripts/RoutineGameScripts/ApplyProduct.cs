using UnityEngine;
using System.Collections;

public class ApplyProduct : MonoBehaviour
{
    public string requiredProductTag;
    public ProgressBar progressBar;
    public float progressIncrement = 0.5f; // Adjust this value based on the number of steps
    public ParticleSystem shampooParticles; // Reference to the Particle System
    public AudioClip sinkSound;

    private void Start()
    {
        if (shampooParticles == null)
        {
            Debug.LogError("ShampooParticles not assigned!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ApplyProduct OnTriggerEnter2D called with object: " + other.gameObject.name);
        if (other.CompareTag(requiredProductTag))
        {
            Debug.Log("Product applied: " + requiredProductTag);
            progressBar.IncreaseProgress(progressBar.progressBar.fillAmount + progressIncrement);
            StartCoroutine(DisableAfterDelay(other.gameObject, 3f)); // Delay of 0.5 seconds before disabling
            shampooParticles.Play(); // Start emitting particles
        }
        else
        {
            Debug.Log("Object tag does not match required product tag.");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(requiredProductTag))
        {
            if (!shampooParticles.isPlaying)
            {
                SoundFXManager.instance.PlaySoundFXClip(sinkSound,transform,1f);
                shampooParticles.Play(); // Continue emitting particles
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("ApplyProduct OnTriggerExit2D called with object: " + other.gameObject.name);
        if (other.CompareTag(requiredProductTag))
        {
            shampooParticles.Stop(); // Stop emitting particles
        }
    }

    private IEnumerator DisableAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }
}
