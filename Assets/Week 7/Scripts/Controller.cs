using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Player selectedPlayer { get; private set; }
    public Slider chargeSlider;
    public float maxCharge;
    private float charge;
    private Vector2 direction;

    public void Start()
    {
        chargeSlider.maxValue = maxCharge;
    }
    public static void SelectPlayer(Player selection)
    {
        if (selectedPlayer != null) selectedPlayer.Selected(false);
        selectedPlayer = selection;
        selection.Selected(true);
    }
    private void FixedUpdate()
    {
        if (direction != Vector2.zero)
        {
            selectedPlayer.Move(direction);
            direction = Vector2.zero;
        }
    }
    private void Update()
    {
        if (selectedPlayer == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            charge = 0;
            direction = Vector2.zero;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            charge += Time.deltaTime;
            charge = Mathf.Clamp(charge, 0, maxCharge);
            chargeSlider.value = charge;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)selectedPlayer.transform.position).normalized * charge;
        }
    }
}
