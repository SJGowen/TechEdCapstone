using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    //This represents the scene it is transitioning too
    [SerializeField] private string transitionTo;

    //The starting point for the character when game starts or when the character moves to the next scene
    [SerializeField] Transform startPoint;

    //The direction of where the player character exits the scene
    [SerializeField] Vector2 exitDirection;

    //The amount of time it will take for a player to exit the scene transition
    [SerializeField] private float exitTime;

    private void OnTriggerEnter2D(Collider2D _other)
    {
        if(_other.CompareTag("Player"))
        {
            GameManager.Instance.transitionedFromScene = SceneManager.GetActiveScene().name;

            PlayerController.Instance.pState.cutscene = true;

            SceneManager.LoadScene(transitionTo);
        }
    }

    private void Awake()
    {
        if (transitionTo == GameManager.Instance.transitionedFromScene)
        {
            PlayerController.Instance.transform.position = startPoint.position;

            StartCoroutine(PlayerController.Instance.WalkIntoNewScene(exitDirection, exitTime));
        }
    }
}
