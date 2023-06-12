using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundUIControl : MonoBehaviour, IPointerExitHandler
{
	private Animator anim;
	private GameObject soundPanel;
	public GameObject soundIcon;
	public Button returnButton;
	
	private void Awake() {
		anim = GetComponent<Animator>();
		soundPanel = this.gameObject;
	}
	
	public void OpenSoundPanel()
	{
		anim.SetBool("OpenSound", true);
	}
	
	public void CloseSoundPanel()
	{
		anim.SetBool("OpenSound", false);
	}
	
	public void SetReturnButtonIntractable()
	{
		returnButton.interactable = !returnButton.interactable;
	}
	
	public void SetSoundIconActivity()
	{
		soundIcon.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		soundIcon.SetActive(!soundIcon.activeInHierarchy);
	}

    public void OnPointerExit(PointerEventData eventData)
    {
        CloseSoundPanel();
    }
}
