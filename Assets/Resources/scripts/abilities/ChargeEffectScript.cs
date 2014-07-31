using UnityEngine;
using System.Collections;

/* ChargeEffectScript - Character charges forward and knocks out all enemies in its path */
public class ChargeEffectScript : MonoBehaviour {

  public float amount = 0f;
  public float radius = 0f;    // Radius around the player the charge affects
  public float duration = 0f;


  public Transform spellEffect;     // Effect (Prefab) the ability will apply to 

  private string ownerType;

  private CircleCollider2D abilityCollider; // Collider on the effect
  private CharacterScript character;    // Character who has the ability equipped
  private MoveScript characterMoveScript;	// Move script so we can push the character

  void Start () {

    character = GetComponentInParent<CharacterScript>();
    ownerType = character.gameObject.tag; // Get the character's tag so we can decide who the ability should damage
	
	Charge(character.gameObject);

//    abilityCollider = GetComponent<CircleCollider2D>();
//    abilityCollider.radius = radius;  // Stomp effect should always match radius of Stomp
//
    Destroy (gameObject, duration); // effect should go away after <duration>
  }

//  void OnTriggerEnter2D(Collider2D defenderCollider) {
//	// Knockback any enemy that the player touches
//    
//    MoveScript defenderMoveScript = defenderCollider.GetComponent<HealthScript>();
//    
//    // Check if the object I'm colliding with can be damaged
//    if(defenderHealth != null) {
//      
//      GameObject defender = defenderHealth.gameObject;
//      
//      if((ownerType == "Player" && defender.tag == "Enemy")
//         ||
//         (ownerType == "Enemy" && defender.tag == "Player")){
//      
//        // Ability should snare (reduce move speed of) the defender
//        ApplySnare (defender, snareAmount, snareDuration);
//      
//        // Calculate armor reduction
//        float totalDamage = damage - defenderHealth.armor;
//        
//        // attacks should always do 1 dmg, even if they are very weak
//        if(totalDamage <= 0) {
//          totalDamage = 1;
//        }
//        
//        defenderHealth.Damage(totalDamage);   // Apply damage to the defender
//      }
//    }
//  }


	void Charge(GameObject character) {
		if(characterMoveScript == null) {
			// Only get target movescript once
			characterMoveScript = character.GetComponent<MoveScript>();	// Push() is part of MoveScript
			
			Vector2 direction = characterMoveScript.facing;
			
			characterMoveScript.Push(direction, amount);
		}

	}

	void ApplyStun(GameObject defender) {
		var effect = Instantiate(spellEffect) as Transform;
		
		var effectScript = effect.gameObject.GetComponent<Effect>();
		effectScript.target = defender;

	}

}
