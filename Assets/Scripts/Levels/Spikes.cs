using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public LayerMask PC;
    public LayerMask Enemy;
	PCController pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PCController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerEnter2D(Collider2D Spikes)
	{
		if ((PC.value & (1 << Spikes.gameObject.layer)) != 0)
		{
			pc.Die();
		}
		else if ((Enemy.value & (1 << Spikes.gameObject.layer)) != 0)
		{
			BaseEnemy baseEnemy = Spikes.gameObject.GetComponent<BaseEnemy>();
			if (baseEnemy != null)
			{
				baseEnemy.OnHitByHazard();
			}
			
		}
	}

}
