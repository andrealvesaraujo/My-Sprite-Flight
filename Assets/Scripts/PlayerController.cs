using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    private float elapsedTime = 0f;
    private float score = 0f;

    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;

    Rigidbody2D rb;

    public float maxSpeed = 5f;
    public GameObject boosterFlame;

    public UIDocument uiDocument;
    private Label scoreText;
    private Button restartButton;

    public GameObject explosionEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScore();
        MovePlayer();        
    }

    void UpdateScore(){
        elapsedTime +=Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;
    }

    void MovePlayer(){
        if(Mouse.current.leftButton.isPressed){
            //Calculate mouse direction
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;
            
            // Move player in direction of mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if(rb.linearVelocity.magnitude > maxSpeed){
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }

        if(Mouse.current.leftButton.wasPressedThisFrame){
            boosterFlame.SetActive(true);
        } else if (Mouse.current.leftButton.wasReleasedThisFrame){
            boosterFlame.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        restartButton.style.display = DisplayStyle.Flex;
    }
}
