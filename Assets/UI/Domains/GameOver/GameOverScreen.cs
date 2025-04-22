using UnityEngine;
using UnityEngine.UIElements;

public class GameOverScreen : MonoBehaviour
{
    void Start()
    {
        // Get the UIDocument component attached to this GameObject
        UIDocument uiDocument = GetComponent<UIDocument>();

        // Get the root visual element from the UIDocument
        VisualElement root = uiDocument.rootVisualElement;

        // Find the button by its name (assuming the button is named "RetryButton" in the UI Builder)
        var RestartButton = root.Q<Button>("RestartButton");

        // Assign a click event to the button
        if (RestartButton != null)
        {
            RestartButton.clicked += OnRetryButtonClicked;
        }
        else
        {
            Debug.LogError("RetryButton not found in the UI Document.");
        }
    }

    private void OnRetryButtonClicked()
    {
        // Logic to handle retry button click
        Debug.Log("Retry button clicked!");
        GameLootLoading.Instance.RestartLevel();
        // Add your retry logic here
    }
}
