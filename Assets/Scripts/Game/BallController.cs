using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public GameObject gameScene;
    public GameObject oppsText;
    public AudioSource _sound;
    public AudioClip _soundClip;
    [Header("Movement")]
    public float forwardSpeed = 1f;
    public float horizontalSpeed = 5f;
    public float maxX = 3f;

    [Header("Speed Increase Over Time")]
    public float speedIncreaseAmount = 0.2f;
    public float maxForwardSpeed = 10f;
    public float speedIncreaseInterval = 3f;

    [Header("Scale")]
    public float scaleChangeAmount = 0.1f;
    public float minScale = 0.3f;
    public float maxScale = 2f;
    public float scaleIncreaseOverTime = 0.02f;
    public float scaleIncreaseInterval = 1f;

    [Header("UI")]
    public TMP_Text speed;
    public TMP_Text size;
    public TMP_Text distance;

    public TMP_Text speedPopup;
    public TMP_Text sizePopup;
    public TMP_Text distancePopup;

    public GameObject gameOwer;

    public Button _close;
    public Button _okay;

    private Rigidbody2D rb;
    private Vector2 touchStartPosition;
    private float screenHalfWidth;
    private float startScale;

    private float timeSinceLastScaleUp = 0f;
    private float timeSinceLastSpeedUp = 0f;
    private bool isDead = false;

    private int distanceInt;

    public static event Action OnDeath;

    private float horizontalInput;

    private float maxSize;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        screenHalfWidth = Screen.width / 2f;
        startScale = transform.localScale.x;

        StartCoroutine(Timer());

        _close.onClick.AddListener(Close);
        _okay.onClick.AddListener(Close);
    }

    private void OnDestroy()
    {
        _close.onClick.RemoveListener(Close);
        _okay.onClick.RemoveListener(Close);
    }
    void Update()
    {
        if (isDead) return;

#if UNITY_EDITOR
        horizontalInput = Input.GetAxis("Horizontal");
#else
        HandleTouchInput();
#endif

        GradualScaleIncrease(Time.deltaTime);
        GradualSpeedIncrease(Time.deltaTime);
        CheckDeath();
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            distanceInt++;
            distance.text = distanceInt + "m";
        }
    }

    void FixedUpdate()
    {

        if (isDead) return;

        // Обчислюємо нову позицію
        float newX = Mathf.Clamp(rb.position.x + horizontalInput * horizontalSpeed * Time.fixedDeltaTime, -maxX, maxX);
        float newY = rb.position.y + forwardSpeed * Time.fixedDeltaTime;

        Vector2 targetPosition = new Vector2(newX, newY);
        rb.MovePosition(targetPosition);
    }

    void HandleTouchInput()
    {
        horizontalInput = 0f;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                float deltaX = touch.position.x - touchStartPosition.x;
                horizontalInput = Mathf.Clamp(deltaX / screenHalfWidth, -1f, 1f);
            }
        }
    }

    void HandleKeyboardInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // підтримує клавіатуру та геймпад
        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            MoveHorizontally(horizontalInput);
        }
    }

    void MoveHorizontally(float direction)
    {
        float newX = transform.position.x + direction * horizontalSpeed * Time.deltaTime;
        newX = Mathf.Clamp(newX, -maxX, maxX);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void GradualScaleIncrease(float deltaTime)
    {
        timeSinceLastScaleUp += deltaTime;
        if (timeSinceLastScaleUp >= scaleIncreaseInterval)
        {
            ChangeScale(scaleIncreaseOverTime);
            timeSinceLastScaleUp = 0f;
        }
    }

    void GradualSpeedIncrease(float deltaTime)
    {
        timeSinceLastSpeedUp += deltaTime;
        if (timeSinceLastSpeedUp >= speedIncreaseInterval)
        {
            forwardSpeed = Mathf.Min(forwardSpeed + speedIncreaseAmount, maxForwardSpeed);
            timeSinceLastSpeedUp = 0f;

            speed.text = forwardSpeed.ToString("F2") + "m/s";
        }
    }

    void CheckDeath()
    {
        if (transform.localScale.x < startScale)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.velocity = Vector2.zero;
        Debug.Log("🧨 Ball died!");
        Time.timeScale = 0f;
        OnDeath?.Invoke();
        gameOwer.SetActive(true);
        distancePopup.text = distanceInt + "m";
        speedPopup.text = forwardSpeed.ToString("F2") + "m/s";
        sizePopup.text = maxSize.ToString("F2");

        if(distanceInt > PlayerPrefs.GetFloat("RecordDistance"))
        {
            PlayerPrefs.SetFloat("RecordDistance", distanceInt);
        }
        if (forwardSpeed > PlayerPrefs.GetFloat("RecordSpeed"))
        {
            PlayerPrefs.SetFloat("RecordSpeed", forwardSpeed);
        }
        if (maxSize > PlayerPrefs.GetFloat("RecordBallSize"))
        {
            PlayerPrefs.SetFloat("RecordBallSize", maxSize);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (PlayerPrefs.GetInt("Vibration") == 1)
            {
                Handheld.Vibrate();
            }
            if (PlayerPrefs.GetInt("Sounds") == 1)
            {
                _sound.PlayOneShot(_soundClip);
            }
            ChangeScale(-scaleChangeAmount);
            GameObject spawnedText = Instantiate(oppsText);
            spawnedText.transform.position = collision.gameObject.transform.position;
            Destroy(collision.gameObject);
        }
    }

    public void ChangeScale(float amount)
    {
        float current = transform.localScale.x;
        float newScale = Mathf.Clamp(current + amount, minScale, maxScale);
        transform.localScale = new Vector3(newScale, newScale, newScale);

        size.text = transform.localScale.x.ToString("F2");
        if(maxSize < transform.localScale.x)
        {
            maxSize = transform.localScale.x;
        }
    }


    private void Close()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
