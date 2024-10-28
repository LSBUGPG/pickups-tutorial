# Pickups Tutorial

This tutorial covers how to add simple collectable pickups to a game

## Prerequisites

Before approaching this tutorial, you will need a current version of Unity and a code editor (such as Microsoft Visual Studio Community) installed and ready to use.

This tutorial was created with Unity 2022.3 LTS and Microsoft Visual Studio Community 2022 versions. It should work with earlier or later versions. But you should check the release notes for other versions as the Editor controls or Scripting API functions may have changed.

If you need help installing Unity you can find many online tutorials such as:
https://learn.unity.com/tutorial/install-the-unity-hub-and-editor

You will also need to know how to create an empty project, add primitive objects to your scene, create blank scripts, create prefabs, and run projects from within the editor. If you need help with this, there is a short video demonstrating how to do all of these things here: 

https://www.youtube.com/watch?v=eQpWPfP1T6g

## Objectives

In this tutorial our objectives are to create collectable pickups such as you would see in early 3D platform games like Super Mario 64:
![Super Mario collecting coins](https://cdn.videogamesblogger.com/wp-content/uploads/2011/08/super-mario-3d-land-screenshot-coin-collecting-646x387.jpg)
*Image credit: [Video Games Blogger](https://videogamesblogger.com)*

To demonstrate we are going to create a scene with some coins on a platform. And we are going to push a block (representing the player) into the coins to pick them up.

https://github.com/user-attachments/assets/21d94515-f8e4-44f5-99a6-88db01f3c6df

## Coin pickup prefab

- Create a new Unity 3D core project
- Add a cylinder object and name it "Coin"
- Remove the cylinder collider
- Add a sphere collider
- Set the radius of the sphere collider to 0.5
- Tick the `Is Trigger` box
- Change the y scale to make it look a little more coin like
- Change the rotation x value to 90
- Drag the object into the Assets window

This creates a prefab which we can use to scatter instances of this coin pickup around.

> [!TIP]
> To make it stand out, create a new Material called "Gold". Change the `Metallic` slider up to 1. Change the colour to yellow. 
>
> ![image](https://github.com/user-attachments/assets/cb17cd32-b403-4736-bf1b-3676fcc1f4f5)
>
> To edit a prefab, you can open it by double clicking on the prefab object in the asset window.
>
> ![image](https://github.com/user-attachments/assets/cce8cb28-28f0-4941-8ca1-f636818ed026)
>
> With the prefab open, drag or select the gold material in element 0 of the `Mesh Renderer` and `Materials` section.

Your prefab should look like this:

![image](https://github.com/user-attachments/assets/984bdbe0-c145-40cf-82ee-d480a513b07a)

> [!CAUTION]
> For this tutorial, make sure the `Is Trigger` option is ticked. Without this option, the player will collide with the coin causing both objects to get deflected. It is possible to make pickups work with collision rather than triggers, but you would need to use the `OnCollision...` functions rather than `OnTrigger...` below.

## Create a platform

We'll need somewhere to place our coins. So let's create a simple platform. Start by creating a cube and name it "Platform". Change the x scale to 30 and make sure the position is 0, 0, 0.

![image](https://github.com/user-attachments/assets/75ece902-52af-4efc-b289-229e1959ddfc)

> [!TIP]
> To make this stand out, create a new material and call it "Grass." Set the colour to green and apply this material to the platform.

## Place the coins

Take the coin in the scene and place it somewhere on the platform. Then duplicate by choosing the `Duplicate` option from the context menu, or type `Ctrl D`. This will create a copy at the same location so you will need to move the copy to a new location to see it. Create as many copies as you need to make a good demonstration.

![image](https://github.com/user-attachments/assets/62a9599a-567d-423a-a94c-436fad655f25)

> [!NOTE]
> The positions illustrated above are:
> | GameObject | X position | Y position | Z position |
> | :--- | ---: | ---: | ---: |
> | Coin | -5 | 1 | 0 |
> | Coin (1) | -3 | 1 | 0 |
> | Coin (2) | -1 | 1 | 0 |
> | Coin (3) |  1 | 1 | 0 |
> | Coin (4) |  3 | 1 | 0 |
> | Coin (5) |  5 | 1 | 0 |

## Create a player object

To test the coins we need an object to represent the player.

- Create a cube and name it "Player"
- Place it next to the coins
- Add a `Rigidbody` component

> [!CAUTION]
> For the purposes of this tutorial, the gravity option should not be set. Applying gravity in this simple example will cause the cube object to experience friction with the platform surface and tip over when pushed. This might cause the cube to fall off of the platform before hitting any of the coins.
>![image](https://github.com/user-attachments/assets/fc29e548-0c60-4892-9c6f-89ad3966a5aa)

Creating player control is not an objective of this tutorial, so to create some movement we'll add a simple script to add a force to push the player when the space bar is pressed.

Create a new script and call it player:

```cs
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody body;
    public float force = 2;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            body.AddForce(Vector3.right * force, ForceMode.Impulse);
        }
    }
}
```

This script will test for keyboard input every frame. If the space bar is pressed, an impulse force is added to the referenced rigid body object which should cause it to move in the given direction.

> [!IMPORTANT]
> We use an impulse force to make sure that the force applied is consistent. With an impulse, the force applied will result in an instantaneous change to the velocity of the object depending on Newton's second law $F=ma$. Since the default mass is 1, this means applying an impulse force of 2 should accelerate the object from standstill to 2 units per second.

Add this script to the player object and drag the player's rigid body component into the body slot attached to the script.

![image](https://github.com/user-attachments/assets/1c2720ca-0c48-4cdf-807a-5e8cb71ba2f1)

With this set up, you be able to test the script by running the project and pressing the space bar.

The player object should move to the right at 2 Unity units per second and go through each of the coin objects. The coins shouldn't react in this test.

## Create a pickup script

To get Unity to detect that the player has touched a coin one of the two needs to have a rigid body physics component. Because we have set up the player to have one, that is enough.

When Unity detects that a player and coin object intersect in a physics update frame it calls `OnCollision...` or `OnTrigger...` functions on both objects. Since we have set the coin colliders to be triggers, both objects will receive `OnTriggerEnter` calls on the frame that this first happens. We can use this to collect the pickup.

Create a new script, and call it `Pickup`. In this script, add an `OnTriggerEnter` function.

```cs
using UnityEngine;

public class Pickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
    }
}
```

In this function:
- `void` is the return type (that is, no return value)
- [`OnTriggerEnter`](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Collider.OnTriggerEnter.html) is the name of the Unity message function
- `Collider` is the base type of component collided with
- `other` is a name to refer to the component collided with (by convention this is called "other")

As this function will get called whatever other object collided with this coin, we should start by checking if it is indeed the player that this call is for. To do that we can use the [`GetComponent`](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Component.GetComponent.html) function to find the `Player` component on the `other` object if one exists.

```cs
void OnTriggerEnter(Collider other)
{
    Player player = other.GetComponent<Player>();
}
```

In this line:
- `Player` is the script we made for the player object
- `player` is the name for the variable to refer to the component if one exists
- `other` is the parameter given to us by Unity referring to the collider that entered the pickup's trigger
- `GetComponent` is the function we are calling
- `<Player>` is the type of the component that we are looking for
- `()` means that we aren't providing any arguments to the `GetComponent` function

After executing this line, the variable we are calling `player` should either contain a reference to a `Player` component, or `null` is the object that entered the trigger didn't have one.

Here we can use an `if` statement to test if we got a player or not.

```cs
void OnTriggerEnter(Collider other)
{
    Player player = other.GetComponent<Player>();
    if (player != null)
    {
    }
}
```

In the `if` instruction:

- `if` is the instruction
- `player` is the name of the variable we are using to refer to the component
- `!=` means not equal to
- `null` here means no component
- `{}` these brackets enclose instructions that are only executed if the condition is true

So inside the curly brackets we can turn off the coin object by using the [`SetActive`](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/GameObject.SetActive.html) function. This function takes one argument which is a boolean value (that is, either `true` to turn it on, or `false` to turn it off.)

To refer to the pickup `GameObject` Unity provides the variable [`gameObject`](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Component-gameObject.html) as part of the `MonoBehaviour` base class.

```cs
void OnTriggerEnter(Collider other)
{
    Player player = other.GetComponent<Player>();
    if (player != null)
    {
        gameObject.SetActive(false);
    }
}
```

To attach this script to every coin, open the Coin prefab object and drag the script onto it. Modifying the prefab will automatically update all instances of the prefab.

To test this out, click run in Unity and press the space bar. If it works, each coin should disappear as soon as the player cube touches it.

## Counting the coins

To count up the coins as they are collected we can call a function on the player script to increase the count when the pickup is turned off.

In the player script we need to add a variable to count up the coins.

```cs
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody body;
    public float force = 2;

    int coins;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            body.AddForce(Vector3.right * force, ForceMode.Impulse);
        }
    }
}
```

In this variable definition:

- `int` is the type of the variable (an integer in this case)
- `coins` is the name of the variable

By not specifying the `public` keyword we are by default declaring the variable as private to this class. And by not providing an initial value, the initial value will be set to the default value, which is `0`.

> [!TIP]
> We could make this variable public, but it is good practice to limit the access to variables. Instead we'll provide a `public` function called `AddCoin` to narrow the possible interactions with other objects.

Next, we'll add a function `AddCoin`:

```cs
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody body;
    public float force = 2;

    int coins;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            body.AddForce(Vector3.right * force, ForceMode.Impulse);
        }
    }

    public void AddCoin()
    {
    }
}
```

This will allow the pickup script to call into the player script and execute this function when the pickup detects that the player has entered the trigger.

All we need to do inside the `AddCoin` function is to count up the coins. We'll also add a `Debug.Log` instruction so we can see the current coin count go up as each coin is collected.

```cs
public void AddCoin()
{
    coins++;
    Debug.Log($"Coins collected: {coins}");
}
```

In the C# language, the instruction `coins++` means add 1 to the `coins` variable.

In the `Debug.Log` line:
- `$` means that the following is a format string
- `""` everything between the quotes will be included in the output
- `Coins collected: ` is the literal text of the message
- `{}` within a format string, the curly brackets refer to an expression to be evaluated as part of the output
- `coins` refers to the script member variable coins

In this instruction, the value of the `coins` variable will be embedded into the output string where the `{coins}` is. So if the `coins` variable contains `2` then the output string will be `Coins collected: 2`

Of course, the `AddCoin` function won't do anything until we add an instruction in the pickup script to call it.

```cs
using UnityEngine;

public class Pickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddCoin();
            gameObject.SetActive(false);
        }
    }
}
```

With this code in place, we can test the counting function by running the game, pressing the space bar, and watching the console output for messages.

![image](https://github.com/user-attachments/assets/e82a6a80-00b9-4563-b06e-3828b2006373)

## Adding polish

To make our coins a little more visually attractive, we can spin them on the vertical axis. To do this we can rotate the pickup around the global up vector.

Add a rotational speed variable to represent the rotation in terms of degrees per second:
```cs
public float speed = 360;
```

Then, add an update function to rotate the object's transform component:

```cs
void Update()
{
    transform.Rotate(0, speed * Time.deltaTime, 0, Space.World);
}
```

> [!NOTE]
> Multiplying by `Time.deltaTime` here is required to split the rotation amount into exactly enough steps to reach `speed` degrees in one second.

> [!CAUTION]
> The last argument `Space.World` tells Unity to interpret the values given as global X, Y, and Z axes. Without this, the coin would spin around its centre and so wouldn't appear to be moving at all.

One other nice addition is to trigger a sound effect each time a coin is collected. You can use your own sound file for this, although I have included an example in this repository called `Coin.wav`.

To play a sound clip, you first need to import the sound file into Unity. Then add a variable pointing to the clip inside the script you want to use. In our case, this will be in the pickup script.

```cs
public AudioClip clink;
```

In Unity, this variable should be assigned to your collection sound effect. The best way to do this is to open the Coin prefab and then assign the variable in there. This will automatically attach the same sound to every instance.

Finally, on collection of the pickup we can trigger the sound. Here we will use the [`AudioSource.PlayClipAtPoint`](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/AudioSource.PlayClipAtPoint.html) function. The arguments to this function are the clip to play, and the position in 3D space.

```cs
AudioSource.PlayClipAtPoint(clink, Camera.main.transform.position);
```

- `AudioSource` is the class containing the function
- `PlayClipAtPoint` the function to start playing a sound effect
- `clink` the member variable we have defined to contain our sound clip
- `Camera` the class representing the camera object
- `main` a variable pointing to the main camera in the scene
- `transform` a variable pointing to the Transform component
- `position` the 3D position of the Transform

Playing the sound effect at the position of the camera ensures that the volume and the stereo position of the sound is consistent.

> [!CAUTION]
> Using an `AudioSource` component for this kind of sound effect won't work as when the object is switched off, any sound effects that is playing are also stopped.

```cs
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float speed = 360;
    public AudioClip clink;

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            AudioSource.PlayClipAtPoint(clink, Camera.main.transform.position);
            player.AddCoin();
            gameObject.SetActive(false);
        }
    }
}
```
