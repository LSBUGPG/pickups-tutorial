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
> For this tutorial, make sure the `Is Trigger` option is ticked. Without this option, the player will collide with the coin causing both objects to get deflected. It is possible to make it work with collision, but you would need to use the `OnCollision...` functions rather than `OnTrigger...` below.

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
