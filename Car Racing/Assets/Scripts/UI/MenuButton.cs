using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, /*IPointerClickHandler,*/ IPointerExitHandler
{

    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;
    [SerializeField] MenuManager menuManager;
    [SerializeField] int Level;

    // Start is called before the first frame update
    void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (menuButtonController.index == thisIndex)
        {

            animator.SetBool("selected", true);
            if(Input.GetAxis("Submit")==1 || Input.GetMouseButton(0))
            {

                    menuManager.LoadLevel(Level);
                    

                if (thisIndex == 2 && this.gameObject.name=="Exit")
                {
                    Application.Quit();
                }

                animator.SetBool("pressed", true);
            }
            else if(animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
                animatorFunctions.disableonce = true;
            }

        }
        else
        {
            animator.SetBool("selected", false);
        }
    }
    // Mouse hover event

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuButtonController.index = thisIndex;
        animator.SetBool("selected", true);
    }
    


    // Mouse exit event (optional, if you want to handle deselection on mouse exit)
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("selected", false);
    }

    // Function to trigger the press animation
    private void TriggerPressAnimation()
    {
        animator.SetBool("pressed", true);

        // Reset the "pressed" state after a short delay so the animation can play fully
        StartCoroutine(ResetPressedState());
    }

    // Coroutine to reset the pressed state
    IEnumerator ResetPressedState()
    {
        yield return new WaitForSeconds(0.1f); // Adjust this value based on your animation duration
        animator.SetBool("pressed", false);
        animatorFunctions.disableonce = true;  // Ensure disableonce is triggered after the press
    }

    /*
         // Mouse click or touch event
    public void OnPointerClick(PointerEventData eventData)
    {

        if (menuButtonController.index == thisIndex)
        {
            if (thisIndex == 0)
            {
                menuManager.OpenTackMainLevel();
            }
            animator.SetBool("pressed", true);
        }
        else
        {
            animator.SetBool("selected", false);
        }
        //TriggerPressAnimation();
        Debug.Log("Pressed");
        
        if (menuButtonController.index == thisIndex)
        {
            if (thisIndex == 0)
            {
                menuManager.OpenTackMainLevel();
            }
            animator.SetBool("selected", true);
            if (animator.GetBool("pressed"))
            {
                animator.SetBool("pressed", false);
                animatorFunctions.disableonce = true;
            }
            else
            {
                animator.SetBool("pressed", true);
            }
        }
        else
        {
            animator.SetBool("selected", false);
        }
        
    }
    */
}
