using UnityEngine;
using System.Collections;

public class Turret : BaseTurret {
	
	public GameObject leftBarrel;
	
	public GameObject rightBarrel;
	
	private float lastAnimationTime = 0f;

	public override void Start () {
		base.Start();
		
		if (!leftBarrel && !rightBarrel)
			Debug.LogError("Turret barrels are not attachned");	
	}
	
	public override void PlayAnimation() {				
		//0.75 the length of the shooting animation part
		if (Time.time - lastAnimationTime > 0.75f) {
			
			//0.45 - shooting start time animation
			if (leftBarrel)
			{
			    Animation leftBarrelAnimation = leftBarrel.animation;
			    AnimationState lAnimationState = leftBarrel.animation["left_barrel_shoot"];
                if (leftBarrelAnimation && lAnimationState)
			    {
                    lAnimationState.time = 0.45f;
                    leftBarrelAnimation.Play();
			    }
			}
			
			if (rightBarrel) {
                Animation rightBarrelAnimation = rightBarrel.animation;
                AnimationState rAnimationState = rightBarrel.animation["right_barrel_shoot"];
                if (rightBarrelAnimation && rAnimationState)
			    {
                    rAnimationState.time = 0.45f;
                    rightBarrelAnimation.Play();
			    }
			}
			
			lastAnimationTime = Time.time;
		}
		
	}
}
