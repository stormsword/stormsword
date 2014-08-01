using UnityEngine;
using System.Collections;

public class StompScript : MonoBehaviour {

  public float duration = 0.5f;   // Time (seconds) the stomp effect lasts once used
  public float damage = 1;      // Damage the ability does on impact
  public float radius = 0f;       // Radius the ability affects upon impact
  public float snareAmount = 0f;    // Percentage (%) to snare affected targets (0-100)
  public float snareDuration = 0f;  // Time (seconds) snare the effect lasts

  public Transform spellEffect;     // Effect (Prefab) the stomp will apply

  private string ownerType;

  private CircleCollider2D stompCollider; // Collider on the stomp effect
  private CharacterScript character;    // Character who has the ability equipped

  void Start () {

    character = GetComponentInParent<CharacterScript>();
    ownerType = character.gameObject.tag; // Get the character's tag so we can decide who the ability should damage

    stompCollider = GetComponent<CircleCollider2D>();

    stompCollider.radius = radius;  // Stomp effect should always match radius of Stomp

    Destroy (gameObject, duration); // effect should go away after <duration>
  }

  void OnTriggerEnter2D(Collider2D defenderCollider) {
    
    HealthScript defenderHealth = defenderCollider.GetComponent<HealthScript>();
    
    // Check if the object I'm colliding with can be damaged
    if(defenderHealth != null) {
      
      GameObject defender = defenderHealth.gameObject;
      
      if((ownerType == "Player" && defender.tag == "Enemy")
         ||
         (ownerType == "Enemy" && defender.tag == "Player")){
      
        // Ability should snare (reduce move speed of) the defender
        ApplySnare (defender, snareAmount, snareDuration);
      
        // Calculate armor reduction
        float totalDamage = damage - defenderHealth.armor;
        
        // attacks should always do 1 dmg, even if they are very weak
        if(totalDamage <= 0) {
          totalDamage = 1;
        }
        
        defenderHealth.Damage(totalDamage);   // Apply damage to the defender
      }
    }
  }

  /* Snare - Slows the movement speed of the defending character
   * defender (GameObject)  - The character affected by the spell
   * snareAmount (float)    - The amount (in percentage) to reduce the character's movement speed
   * snareDuration (float)  - The time (in seconds) the snare effect should last for
   */
  void ApplySnare(GameObject defender, float snareAmount, float snareDuration) {
    var effect = Instantiate(spellEffect) as Transform;

    var effectScript = effect.GetComponent<SnareEffect>();
    effectScript.target = defender;
  }
}
