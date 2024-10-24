using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindChimeHandler : MonoBehaviour
{

    AudioSource chimeSFX;
    SpriteRenderer sprite;
    bool isDragging;
    private Vector3 offset;
    private bool isAnimating = false;  // Check if animation is running

    public int chimeIndex;
    public float tipAngle;       // Angle to tip away
    public float tipDuration;   // Duration for tipping and returning


    // Start is called before the first frame update
    void Start()
    {
        chimeSFX = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
        WindChimeManager.RegisterChime(gameObject, chimeIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            // Let the manager handle position adjustments for other chimes
            WindChimeManager.HandleDrag(gameObject);
        }
    }

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        sprite.sortingOrder = 2;
        isDragging = true;
        PlayChime(); // Trigger the chime and animation on click
    }

    private void OnMouseUp()
    {
        isDragging = false;
        sprite.sortingOrder = 1;

        // Just reset to ensure positions are correct after the drag
        WindChimeManager.ResetChimesPositions();
    }

    // Function to play chime sound and trigger tipping animation
    private void PlayChime()
    {
        if (chimeSFX.isPlaying == false)
        {
            chimeSFX.Play();
        }

        if (!isAnimating)
        {
            StartCoroutine(TipChimeAnimation());
        }
    }

    // Coroutine for tipping the chime away and bringing it back
    private IEnumerator TipChimeAnimation()
    {
        isAnimating = true;

        // Tip backward
        float elapsedTime = 0f;
        while (elapsedTime < tipDuration)
        {
            transform.localRotation = Quaternion.Euler(Mathf.Lerp(0, tipAngle, elapsedTime / tipDuration), 0, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Tip forward back to original position
        elapsedTime = 0f;
        while (elapsedTime < tipDuration)
        {
            transform.localRotation = Quaternion.Euler(Mathf.Lerp(tipAngle, 0, elapsedTime / tipDuration), 0, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(0, 0, 0);
        isAnimating = false;
    }
}
