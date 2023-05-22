using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunSFXPC : MonoBehaviour
{
    [SerializeField]
    PCController pC;

	public void PlayStepSFX()
	{
		pC.PlayStepSFX();
	}


}
