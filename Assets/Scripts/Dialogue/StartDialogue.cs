using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
	[SerializeField] private string fileName;

	public void OnTriggerStay(Collider other)
	{
		if (other.gameObject.TryGetComponent<Movement>(out _) && Input.GetKeyDown(KeyCode.E) 
			&& fileName != string.Empty && Background.BackgroundActive == false)
		{

			DialogueManager.Internal.DialogueStart(fileName);//передаем название файла диалога
		}
	}
}
