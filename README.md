# I5UnityTools 
[![Github All Releases](https://img.shields.io/github/downloads/i5ucc/I5UnityTools/total.svg)](https://github.com/I5UCC/I5UnityTools/releases/latest)
[![GitHub release (latest by date)](https://img.shields.io/github/downloads/i5ucc/I5UnityTools/latest/total?label=Latest%20version%20downloads)](https://github.com/I5UCC/I5UnityTools/releases/latest)
<a href='https://ko-fi.com/i5ucc' target='_blank'><img height='35' style='border:0px;height:25px;' src='https://az743702.vo.msecnd.net/cdn/kofi3.png?v=0' border='0' alt='Buy Me a Coffee at ko-fi.com' />

A set of opinionated tools for VRChat Avatar creation.

All tools located in `Tools/I5Tools`

## Optimize Texture Formats

- Sets all uncompressed textures to normal quality compression.
- Sets all textures mipmapping and streaming mipmaps to be on.
- Sets all textures Crunch Compression to be off.
- Sets all Normal maps to BC5 Texture format.
- Sets all textures that have an alpha channel to BC7.

Might expand this later when i got time for it.

## Force Scene View in Play Mode

Creates a `ForceSceneView` Gameobject that switches to SceneView instead of GameView when going into play mode.

## Make All Bones Immovable in World Space

Sets all Physbones immobileType to World and sets it to 1.0. I just like to use that personally :)

## Delete All OSC Config Files

Deletes all usr_ folders in the VRChat OSC folder. OSC Configurator is a bit annoying sometimes :)

# Download / Install

Either download the latest release from [here](https://github.com/I5UCC/I5UnityTools/releases/latest), or add my Repository to VCC (VRChat Creator Companion): <br>

[<img src="https://github.com/I5UCC/VRCMultiUploader/assets/43730681/7130b744-dcb7-4525-a62c-9bad41331c1c"  width="102" height="35">](https://i5ucc.github.io/vpm/I5Tools.html) <br>
or <br>

[<img src="https://user-images.githubusercontent.com/43730681/235304229-ce2b4689-4945-4282-967e-40bfbf8ebf54.png" height="35">](https://i5ucc.github.io/vpm/main.html) <br>

<details>
  <summary>Manually adding to VCC:</summary>
  
  1. Open VCC
  2. Click "Settings" in the bottom left
  3. Click the "Packages" tab at the top
  4. Click "Add Repository" in the top right
  5. Paste `https://i5ucc.github.io/vpm/VRCMultiUploader.json` into the text field and click "Add"
  6. Click "I understand, Add Repository" in the popup after reading its contents
  7. Activate the checkbox next to the repository "VRCMultiUploader"
  
