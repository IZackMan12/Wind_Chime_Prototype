using System.Collections.Generic;
using UnityEngine;
public class WindChimeManager : MonoBehaviour
{
    public static List<GameObject> windChimes = new List<GameObject>();
    [SerializeField]public static List<Vector3> allChimePositions = new List<Vector3>();
    public static float moveSpeed = 5.0f; // Adjust the speed for smoother movement

    // Register a wind chime at a specific index to maintain correct order

    public static void RegisterChime(GameObject windChime, int index)
    {
        // Ensure the list has enough elements to hold the chime at the given index
        while (windChimes.Count <= index)
        {
            windChimes.Add(null);
            allChimePositions.Add(Vector3.zero);
        }

        // Register the chime
        windChimes[index] = windChime;
        allChimePositions[index] = windChime.transform.position;

        // Debug log to confirm registration
        Debug.Log($"Registered Chime at index {index}: {windChime.name}");
    }

    public static void HandleDrag(GameObject draggedChime)
    {
        int draggedIndex = windChimes.IndexOf(draggedChime);

        // Find the closest position while dragging
        Vector3 closestPosition = allChimePositions[0];
        int closestIndex = 0;
        float closestDistance = Vector3.Distance(draggedChime.transform.position, closestPosition);

        for (int i = 1; i < allChimePositions.Count; i++)
        {
            float distance = Vector3.Distance(draggedChime.transform.position, allChimePositions[i]);
            if (distance < closestDistance)
            {
                closestIndex = i;
                closestDistance = distance;
            }
        }

        // If the closest index has changed (dragged over another chime), update the list
        if (closestIndex != draggedIndex)
        {
            // Remove the dragged chime from the list and insert it at the closest index
            windChimes.Remove(draggedChime);
            windChimes.Insert(closestIndex, draggedChime);

            // Reset positions of all other chimes according to the updated list
            ResetChimesPositions();
        }
    }

    //This was used in a previus version before the windchime list was updating properly
    /*public static void MoveChimeToPosition(int index, Vector3 newPosition)
    {
        if (index >= 0 && index < windChimes.Count)
        {
            Vector3 targetPosition = new Vector3(newPosition.x, windChimes[index].transform.position.y, windChimes[index].transform.position.z);
            windChimes[index].transform.position = Vector3.MoveTowards(
                windChimes[index].transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }
    }*/

    public static void ResetChimesPositions()
    {
        // Reset all chimes back to their designated X positions, keeping Y and Z unchanged
        for (int i = 0; i < windChimes.Count; i++)
        {
            Vector3 targetPosition = allChimePositions[i];
            windChimes[i].transform.position = new Vector3(
                targetPosition.x,
                targetPosition.y,  // Keep current Y
                windChimes[i].transform.position.z   // Keep current Z
            );
        }
    }
}
