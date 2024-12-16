using UnityEngine;

public class NurseStethoscope : MonoBehaviour
{
    // The stethoscope object that the nurse can interact with
    public GameObject stethoscope;

    // The position where the stethoscope should be placed when picked up
    public Transform handPosition;

    // Boolean to keep track of whether the stethoscope is being held
    private bool isHoldingStethoscope = false;

    // Called once on load.
    private void Start()
    {
        // Check if the stethoscope is assigned in the editor, warn if not
        if (stethoscope == null)
        {
            Debug.LogWarning("Stethoscope object is not assigned!");
        }
        
        // Check if the hand position is assigned in the editor, warn if not
        if (handPosition == null)
        {
            Debug.LogWarning("Hand position is not assigned!");
        }
    }
    
    // Called once per frame.
    private void Update()
    {
        // Check for user input - pressing the 'E' key
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Toggle between picking up and dropping the stethoscope
            if (isHoldingStethoscope)
            {
                DropStethoscope();
            }
            else
            {
                PickUpStethoscope();
            }
        }
    }

    // Method to pick up the stethoscope
    private void PickUpStethoscope()
    {
        if (stethoscope != null)
        {
            // Set the stethoscope as a child of the hand position
            stethoscope.transform.SetParent(handPosition);

            // Set local position and rotation to align the stethoscope with the hand
            stethoscope.transform.localPosition = Vector3.zero;
            stethoscope.transform.localRotation = Quaternion.identity;
            
            // Update the boolean to indicate the stethoscope is being held
            isHoldingStethoscope = true;

            // Log the action
            Debug.Log("Picked up the stethoscope.");
        }
    }

    // Method to drop the stethoscope
    private void DropStethoscope()
    {
        if (stethoscope != null)
        {
            // Remove the stethoscope from being a child of any transform
            stethoscope.transform.SetParent(null);

            // Update the boolean to indicate the stethoscope is no longer held
            isHoldingStethoscope = false;

            // Log the action
            Debug.Log("Dropped the stethoscope.");
        }
    }
}
