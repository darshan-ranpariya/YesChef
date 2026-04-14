using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Handles the player picking up, dropping, and interacting with stations.
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction Settings")] [SerializeField]
    private float triggerHeight = 0.75f;

    [SerializeField] private float interactRange = 1.5f;

    [SerializeField] private float interactAngle = 30f;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private InputActionReference interactAction;
    private float[] _angles;

    public IngredientObject HeldIngredient { get; private set; }

    private void Awake()
    {
        _angles = new[] { 0f, -interactAngle, interactAngle };
    }

    private void OnEnable()
    {
        interactAction.action.Enable();
        interactAction.action.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        interactAction.action.Disable();
        interactAction.action.performed -= OnInteractPerformed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var origin = transform.position + Vector3.up * triggerHeight;
        var angles = new[] { 0f, -interactAngle, interactAngle };
        foreach (var angle in angles)
        {
            var direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            Gizmos.DrawRay(origin, direction * interactRange);
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        var origin = transform.position + Vector3.up * triggerHeight;

        foreach (var angle in _angles)
        {
            var direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            var ray = new Ray(origin, direction);

            if (Physics.Raycast(ray, out var hit, interactRange))
            {
                var interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    if (interactable.TryInteract(this))
                    {
                        break;
                    }
                }
            }
        }
    }

    public void SetHeldIngredient(IngredientObject ingredient)
    {
        HeldIngredient = ingredient;
        if (HeldIngredient != null)
        {
            HeldIngredient.transform.SetParent(holdPoint);
            HeldIngredient.transform.localPosition = Vector3.zero;
            HeldIngredient.transform.localRotation = Quaternion.identity;
        }
    }

    public IngredientObject TakeHeldIngredient()
    {
        var ingredientToReturn = HeldIngredient;
        HeldIngredient = null;
        return ingredientToReturn;
    }
}