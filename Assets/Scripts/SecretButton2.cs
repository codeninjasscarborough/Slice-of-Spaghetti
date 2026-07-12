using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SecretButton2 : MonoBehaviour
{
    public enum BizarreType { Runaway, Melting, HeavyGravity, Staring, Shy, Screaming, FakeWall }
    [Header("Choose Your Chaos")]
    public BizarreType behaviorType;

    [Header("Runaway Settings")]
    public float fleeSpeed = 500f;
    public float fleeDistance = 150f;

    [Header("Melting Settings")]
    public float meltSpeed = 2f;

    [Header("Staring Settings")]
    public RectTransform leftEye;
    public RectTransform rightEye;
    public float eyeLookRadius = 10f;

    [Header("Screaming Settings")]
    public AudioSource screamSource;

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Image buttonImage;
    private TMP_Text buttonText;
    private bool isHovered = false;
    private bool isDead = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.localPosition;
        originalScale = rectTransform.localScale;
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TMP_Text>();

        // Setup Fake Wall if selected
        if (behaviorType == BizarreType.FakeWall)
        {
            SetupFakeWall();
        }
    }

    void Update()
    {
        if (isDead) return;

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            Input.mousePosition,
            null,
            out mousePos
        );

        switch (behaviorType)
        {
            case BizarreType.Runaway:
                HandleRunaway(mousePos);
                break;
            case BizarreType.Melting:
                HandleMelting();
                break;
            case BizarreType.Staring:
                HandleStaring();
                break;
            case BizarreType.Shy:
                HandleShy(mousePos);
                break;
            case BizarreType.Screaming:
                HandleScreaming(mousePos);
                break;
        }
    }

    // --- 1. RUNAWAY BUTTON ---
    void HandleRunaway(Vector2 mousePos)
    {
        float distance = Vector2.Distance(rectTransform.localPosition, mousePos);
        if (distance < fleeDistance)
        {
            Vector2 direction = ((Vector2)rectTransform.localPosition - mousePos).normalized;
            rectTransform.localPosition += (Vector3)(direction * fleeSpeed * Time.deltaTime);
            KeepOnScreen();
        }
    }

    void KeepOnScreen()
    {
        RectTransform parentRect = rectTransform.parent as RectTransform;
        Vector3[] corners = new Vector3[4];
        parentRect.GetLocalCorners(corners);

        float minX = corners[0].x + rectTransform.rect.width / 2;
        float maxX = corners[2].x - rectTransform.rect.width / 2;
        float minY = corners[0].y + rectTransform.rect.height / 2;
        float maxY = corners[2].y - rectTransform.rect.height / 2;

        Vector3 pos = rectTransform.localPosition;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        rectTransform.localPosition = pos;
    }

    // --- 2. MELTING BUTTON ---
    void HandleMelting()
    {
        if (isHovered && rectTransform.localScale.y > 0.01f)
        {
            // Stretch down, squash flat
            rectTransform.localScale -= new Vector3(-Time.deltaTime * meltSpeed, Time.deltaTime * meltSpeed, 0);
            rectTransform.localPosition += Vector3.down * meltSpeed * 50f * Time.deltaTime;
        }
    }

    // --- 3. HEAVY GRAVITY BUTTON ---
    void TriggerHeavyGravity()
    {
        isDead = true;
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        gameObject.AddComponent<BoxCollider2D>();
        rb.gravityScale = 400f; // High UI gravity
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    // --- 4. STARING BUTTON ---
    void HandleStaring()
    {
        if (leftEye == null || rightEye == null) return;
        Vector3 mouseWorldPos = Input.mousePosition;
        UpdateEye(leftEye, mouseWorldPos);
        UpdateEye(rightEye, mouseWorldPos);
    }

    void UpdateEye(RectTransform eye, Vector3 mousePos)
    {
        Vector3[] eyeCorners = new Vector3[4];
        eye.GetWorldCorners(eyeCorners);
        Vector3 eyeCenter = (eyeCorners[0] + eyeCorners[2]) / 2f;
        Vector3 dir = (mousePos - eyeCenter).normalized;
        eye.localPosition = dir * eyeLookRadius;
    }

    // --- 5. SHY BUTTON ---
    void HandleShy(Vector2 mousePos)
    {
        float distance = Vector2.Distance(rectTransform.localPosition, mousePos);
        float alpha = Mathf.Clamp01((distance - 50f) / fleeDistance);

        if (buttonImage) buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, alpha);
        if (buttonText) buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, alpha);

        GetComponent<CanvasGroup>().blocksRaycasts = alpha > 0.2f;
    }

    // --- 6. SCREAMING BUTTON ---
    void HandleScreaming(Vector2 mousePos)
    {
        if (screamSource == null) return;
        float distance = Vector2.Distance(rectTransform.localPosition, mousePos);

        if (distance < fleeDistance * 2)
        {
            if (!screamSource.isPlaying) screamSource.Play();
            float intensity = 1f - Mathf.Clamp01(distance / (fleeDistance * 2));
            screamSource.volume = intensity;
            screamSource.pitch = 0.5f + intensity * 1.5f;

            // Tremble effect
            rectTransform.localPosition = originalPosition + (Vector3)Random.insideUnitCircle * intensity * 15f;
        }
        else
        {
            screamSource.Stop();
            rectTransform.localPosition = originalPosition;
        }
    }

    // --- 7. FAKE WALL BUTTON ---
    void SetupFakeWall()
    {
        gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        gameObject.AddComponent<BoxCollider2D>();
    }

    // --- INTERFACE DETECTORS ---
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        if (behaviorType == BizarreType.Shy && !GetComponent<CanvasGroup>())
        {
            gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        if (behaviorType == BizarreType.Screaming && screamSource) screamSource.Stop();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (behaviorType == BizarreType.HeavyGravity)
        {
            TriggerHeavyGravity();
        }
        if (behaviorType == BizarreType.Staring)
        {
            StartCoroutine(BlinkRoutine());
        }
    }

    IEnumerator BlinkRoutine()
    {
        leftEye.gameObject.SetActive(false);
        rightEye.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        leftEye.gameObject.SetActive(true);
        rightEye.gameObject.SetActive(true);
    }


}
