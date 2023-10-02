using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    Waypoint waypoint;

    void Awake() {
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        waypoint = GetComponentInParent<Waypoint>();
        DisplayCoordinates();
    }

    // Update is called once per frame
    void Update()
    {
        // run in edit mode only
        // runs if application is not playing
        if(!Application.isPlaying) {
            DisplayCoordinates();
            UpdateObjectName();
            label.enabled = true;   // remove later
        }
        
        SetLabelColor();
        ToggleLabels();
    }

    void ToggleLabels() {
        if (Input.GetKeyDown(KeyCode.C)) {
            label.enabled = !label.IsActive();
        }
    }

    void SetLabelColor() {
        // change the color of the label based on the value of IsPlaceable in teh Waypoint script;
        if (waypoint.IsPlaceable) {
            // white 
            label.color = defaultColor;          
        } else {
            // black
            label.color = blockedColor;
        }
    }

    void DisplayCoordinates() {
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

        label.text = coordinates.x + "," + coordinates.y;
    }

    void UpdateObjectName() {
        transform.parent.name = coordinates.ToString();
    }
}
