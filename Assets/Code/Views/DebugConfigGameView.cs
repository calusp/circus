using Code.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugConfigGameView : MonoBehaviour
{
    [SerializeField] private GameConfiguration configuration;
    [SerializeField] private GameObject container;
    [Header("Player Speed")]
    [SerializeField] private Slider playerSpeedSlider;
    [SerializeField] private TextMeshProUGUI playerSpeedLabel;

    [Header("Chunk Speed")]
    [SerializeField] private Slider chunkSpeedSlider;
    [SerializeField] private TextMeshProUGUI chunkSpeedLabel;

    [Header("Increment Speed")]
    [SerializeField] private Slider incrementSlider;
    [SerializeField] private TextMeshProUGUI incrementLabel;

    [Header("Distance Cap")]
    [SerializeField] private Slider distanceCapSlider;
    [SerializeField] private TextMeshProUGUI distanceCapLabel;

    [Header("Jump Force")]
    [SerializeField] private TMP_InputField xValue;
    [SerializeField] private TMP_InputField yValue;
    [SerializeField] private TextMeshProUGUI xPlaceHolder;
    [SerializeField] private TextMeshProUGUI yPlaceHolder;
    // Start is called before the first frame update
    void Start()
    {
        container.SetActive(false);
        playerSpeedSlider.onValueChanged.AddListener(UpdatePlayerSpeed);
        playerSpeedSlider.minValue = 0;
        playerSpeedSlider.maxValue = 10;
        playerSpeedSlider.value = configuration.PlayerSpeed;

        chunkSpeedSlider.onValueChanged.AddListener(UpdateChunkSpeed);
        chunkSpeedSlider.minValue = 0;
        chunkSpeedSlider.maxValue = 10;
        chunkSpeedSlider.value = configuration.CameraSpeed;

        incrementSlider.onValueChanged.AddListener(UpdateIncrement);
        incrementSlider.minValue = 0;
        incrementSlider.maxValue = 1;
        incrementSlider.value = configuration.Increment;


        distanceCapSlider.onValueChanged.AddListener(UpdateDistance);
        distanceCapSlider.minValue = 5;
        distanceCapSlider.maxValue = 100;
        distanceCapSlider.value = configuration.DistanceCap;

        xValue.onValueChanged.AddListener(UpdateJumpForceX);
        xPlaceHolder.text = configuration.JumpForce.x.ToString();

        yValue.onValueChanged.AddListener(UpdateJumpForceY);
        yPlaceHolder.text = configuration.JumpForce.y.ToString();
    }

    private void OnEnable()
    {
        xPlaceHolder.text = configuration.JumpForce.x.ToString();
        yPlaceHolder.text = configuration.JumpForce.y.ToString();
        playerSpeedLabel.text = configuration.PlayerSpeed.ToString("0.00");
        chunkSpeedLabel.text = configuration.CameraSpeed.ToString("0.00");
        incrementLabel.text = configuration.Increment.ToString("0.00");
        distanceCapLabel.text = configuration.DistanceCap.ToString("0.00");

    }

    private void UpdateJumpForceY(string updateValue)
    {
        configuration.SetJumpForceYValue(float.Parse(updateValue));
        xPlaceHolder.text = updateValue;
    }

    private void UpdateJumpForceX(string updateValue)
    {
        configuration.SetJumpForceXValue(float.Parse(updateValue));
        yPlaceHolder.text = updateValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            container.SetActive(!container.activeSelf);
            Time.timeScale = container.activeSelf ? 0 : 1;
        }

    }

    private void UpdatePlayerSpeed(float speed)
    {
        configuration.SetPlayerSpeed(speed);
        playerSpeedLabel.text = speed.ToString("0.00");
    }

    private void UpdateChunkSpeed(float speed)
    {
        configuration.SetChunkSpeed(speed);
        chunkSpeedLabel.text = speed.ToString("0.00");
    }

    private void UpdateIncrement(float ratio)
    {
        configuration.SetIncrementRatio(ratio);
        incrementLabel.text = ratio.ToString("0.00");
    }

    private void UpdateDistance(float distance)
    {
        configuration.SetDistanceCap(distance);
        distanceCapLabel.text = distance.ToString("0.00");
    }
}
