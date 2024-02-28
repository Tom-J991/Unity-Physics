using UnityEngine;
using System.Collections;

/// LiquidParticle. 
/// Class contains a circle with multiple states to represent different liquid types.
/// Particles will need to scale in size over time with scaling effecting overall velocity of the sprite.
/// Code has been provided to get you started. You will need to fill in the missing information from each function.

[RequireComponent (typeof(Rigidbody2D))]  
public class LiquidParticle : MonoBehaviour
{

    public enum LiquidStates
    {
	    Water,
	    Lava,	
        Steam   //3 States
    };

    //Different liquid types
    LiquidStates currentState = LiquidStates.Water;
    
    public Material waterMaterial;
    public Material lavaMaterial;
    public Material steamMaterial;

    float startTime = 0.0f;
    float particleLifeTime = 3.0f;

    const float WATER_GRAVITYSCALE = 1.0f;
    const float LAVA_GRAVITYSCALE = 0.3f;
    const float STEAM_GRAVITYSCALE = -1.0f;

    private Rigidbody2D _rigidbody = null;

    
    void Awake ()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        startTime = 0.0f;
	    SetState (currentState);
    }
    
    void Update ()
    {
        // Update for each state.
        switch (currentState)
        {
            case LiquidStates.Water:
            {
                MovementAnimation();
                ScaleDown();
            } break;
            case LiquidStates.Lava:
            {
                MovementAnimation();
                ScaleDown();
            } break;
            case LiquidStates.Steam:
            {
                MovementAnimation();
                ScaleDown();
            } break;
        }
    }


    /// SetState
    /// Change an existing particle to a new type (eg water to lava)
    /// a_newState: The new particle type to be passed in eg. LiquidStates.Lava 
    public void SetState (LiquidStates newState)
    {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        switch (newState)
        {
            // Configure according to new state.
            case LiquidStates.Lava:
            {
                renderer.material = lavaMaterial;
                _rigidbody.gravityScale = LAVA_GRAVITYSCALE;
            } break;
            case LiquidStates.Water:
            {
                renderer.material = waterMaterial;
                _rigidbody.gravityScale = WATER_GRAVITYSCALE;
            } break;
            case LiquidStates.Steam:
            {
                renderer.material = steamMaterial;
                _rigidbody.gravityScale = STEAM_GRAVITYSCALE;
            } break;
        }
        currentState = newState; //

        _rigidbody.velocity = Vector3.zero; // Reset particle velocity.

        startTime = Time.time; // Reset lifetime of the particle.
    }


    /// MovementAnimation
    /// Scales the particle based on its velocity.
    void MovementAnimation ()
    {
        Vector3 movementScale = Vector3.one;
        movementScale.x += _rigidbody.velocity.x / 30.0f;
        movementScale.y += _rigidbody.velocity.y / 30.0f;

        transform.localScale = movementScale;
    }


    /// ScaleDown
    /// Scales the size of the particle based on how long it has been alive. 
    /// Gives the impression of a dying particle.
    void ScaleDown ()
    {
        float scaleValue = 1.0f - ((Time.time - startTime) / particleLifeTime);

        Vector2 particleScale = Vector2.one;
        if (scaleValue <= 0.0f)
        {
            Destroy(gameObject); // Destroy because it's not gonna be seen anyways.
        }
        else
        {
            particleScale.x = scaleValue;
            particleScale.y = scaleValue;
            transform.localScale = particleScale; // Resize.
        }
    }


    /// SetLifeTime
    /// Function allows for the external changing of the particles lifetime.
    /// a_newLifetime: The new time the particle should live for. (eg. 4.0f seconds) 
    public void SetLifeTime (float newLifetime)
    {
        particleLifeTime = newLifetime;
    }


    /// OnCollisionEnter2D
    /// This is where we would handle collisions between particles and call functions like our setState to change
    /// partcle types. Or we could just flat out destroy them etc..
    /// a_otherParticle: The collision with another particle. Obviously not limited to particles so do a check in the method 
    void OnCollisionEnter2D (Collision2D a_otherParticle)
    {
        LiquidParticle otherParticle = a_otherParticle.transform.GetComponent<LiquidParticle>();
        if (otherParticle) // Collided with another particle.
        {
            if (currentState == LiquidStates.Lava && otherParticle.currentState == LiquidStates.Water) // Is lava and has collided with water.
            {
                SetState(LiquidStates.Steam); // Set to steam.
            }
        }

    }
	
}
