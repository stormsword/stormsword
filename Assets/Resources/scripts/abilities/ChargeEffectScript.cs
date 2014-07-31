using UnityEngine;
using System.Collections;

public class ChargeEffectScript : MonoBehaviour {

  public float duration = 0.5f;   // Time (seconds) the stomp effect lasts once used
  
  public float damage = 1;      // Damage the ability does on impact
  public float distance = 0f;    // Distance the player should travel
  public float radius = 0f;    // Radius the effect should affect

  public Transform spellEffect;     // Effect (Prefab) the ability will apply

  private string ownerType;

  private CircleCollider2D abilityCollider; // Collider on the effect
  private CharacterScript character;    // Character who has the ability equipped
  private MoveScript characterMoveScript;	// Move script so we can push the character

  void Start () {

    character = GetComponentInParent<CharacterScript>();
    ownerType = character.gameObject.tag; // Get the character's tag so we can decide who the ability should damage

	characterMoveScript = character.GetComponent<MoveScript>();
	
	ApplyHaste(character.gameObject);

    abilityCollider = GetComponent<CircleCollider2D>();
    abilityCollider.radius = radius;  // Stomp effect should always match radius of Stomp

    Destroy (gameObject, duration); // effect should go away after <duration>
  }

//  void OnTriggerEnter2D(Collider2D defenderCollider) {
//    
//    HealthScript defenderHealth = defenderCollider.GetComponent<HealthScript>();
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
//

	void ApplyHaste(GameObject character) {
		var effect = Instantiate(spellEffect) as Transform;
		
		var effectScript = effect.gameObject.GetComponent<Effect>();
		effectScript.target = character;
	}
  /* Snare - Slows the movement speed of the defending character
   * defender (GameObject)  - The character affected by the spell
   * snareAmount (float)    - The amount (in percentage) to reduce the character's movement speed
   * snareDuration (float)  - The time (in seconds) the snare effect should last for
//   */
//  void ApplySnare(GameObject defender, float snareAmount, float snareDuration) {
//    var effect = Instantiate(spellEffect) as Transform;
//
//    var effectScript = effect.GetComponent<SnareEffect>();
//    effectScript.target = defender;
//  }
}
