# I5UnityTools
Just a few things that make my Avatar Creation workflow a bit easier.

All tools located in `Tools/I5Tools`

## Make All Bones Immovable in World Space

Sets all Physbones immobileType to World and sets it to 1. I just like to use that personally :)

## Optimize Texture Formats

- Sets all Normal maps to BC5 Texture format.
- Sets all textures that have an alpha channel to BC7. aka (DXT5 -> BC7)

Might expand this later when i got time for it.

## Force Scene View in Play Mode

Creates a `ForceSceneView` Gameobject that switches to SceneView instead of GameView when going into play mode.
