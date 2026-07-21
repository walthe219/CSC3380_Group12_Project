using TMPro;
using System.Collections;
using UnityEngine;

public class DamageNumberScript : MonoBehaviour
{
    [SerializeField] TMP_Text DMGText;
    Camera cam;
    
    [Header("Parameters")]
    [Tooltip("Units offset from enemy spawn")]
    [SerializeField] float horzontalSlideOffeset = 2;
    [Tooltip("")]
    [SerializeField] float verticalRandomOffsetMin = 0f;
    [Tooltip("")]
    [SerializeField] float verticalRandomOffsetMax = 1f;
    [Tooltip("Number of damage numbers to occur before they spawn on the opposite side of the target")]
    [SerializeField] int swapSideAmount = 3;
    [Tooltip("Damage Number duration in seconds")]
    [SerializeField] float duration = 1f;

    public AnimationCurve curve;

    float timer;
    static int counter = 0;
    static int direction = -1;

    public void OnEnable()
    {
        //every 3 damage numbers, switch spawn direction 
        direction *= counter % swapSideAmount == 0 ? -1 : 1;
        transform.position += Vector3.up * verticalRandomOffsetMax * (counter % swapSideAmount)/ swapSideAmount;
        counter++;

        cam = Camera.main;
        timer = duration;
    }

    public void Initialize(int damage, int numCrits)
    {
        DMGText.text = damage.ToString();

        //transform.position += Vector3.up * Random.Range(verticalRandomOffsetMin, verticalRandomOffsetMax);
        StartCoroutine(slide(transform.position + cam.transform.right * horzontalSlideOffeset * direction));

        //change color for num crits
        switch (numCrits)
        {
            case 0:
                DMGText.color = Color.white;
                break;
            case 1:
                DMGText.color = Color.yellow;
                break;
            case 2:
                DMGText.color = Color.orange;
                break;
            case 3:
                DMGText.color = Color.red;
                break;
            default:
                //for every crit after three add an ! to the end of the damage number
                DMGText.color = Color.red;
                for (int i = 0; i < numCrits; i++)
                {
                    DMGText.text += '!';
                }
                break;
        }
    }

    public void LateUpdate()
    {
        transform.rotation = cam.transform.rotation;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator slide(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            // Calculate progress percentage (0 to 1)
            float t = elapsedTime / duration;
            float tSmoothed = curve.Evaluate(t);

            // Interpolate position
            transform.position = Vector3.Lerp(startPosition, targetPosition, tSmoothed);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure it snaps perfectly to the final destination
        transform.position = targetPosition;
    }
}
