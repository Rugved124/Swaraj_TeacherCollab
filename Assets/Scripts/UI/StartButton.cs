using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
	//public void OnButtonClick()
	//{
	//	// Start the game
	//	Debug.Log("Game started!");
	//	// Add your game start logic here
	//}

	//private void OnEnable()
	//{
	//	// Register for button press events
	//	var button = GetComponent<Button>();
	//	button.onClick.AddListener(OnButtonClick);

	//	// Register for controller button press events
	//	InputSystem.onDeviceChange += (device, change) =>
	//	{
	//		if (change == InputDeviceChange.Added && device is Gamepad)
	//		{
	//			var gamepad = (Gamepad)device;
	//			//gamepad.buttonSouth.performed += _ => OnButtonClick();
	//		}
	//	};
	//}

	//private void OnDisable()
	//{
	//	// Unregister from button press events
	//	var button = GetComponent<Button>();
	//	button.onClick.RemoveListener(OnButtonClick);

	//	// Unregister from controller button press events
	//	InputSystem.onDeviceChange -= (device, change) =>
	//	{
	//		if (change == InputDeviceChange.Added && device is Gamepad)
	//		{
	//			var gamepad = (Gamepad)device;
	//			//gamepad.buttonSouth.performed -= _ => OnButtonClick();
	//		}
	//	};
	//}

	private Button button;


}