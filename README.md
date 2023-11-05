# Unity Bone Fixer

This collection of Unity Editor tools provides essential functionalities for checking and fixing SkinnedMeshRenderer components in 3D models. 

Do you know the situation when FBX Exporter can't export a model?
Well, this script fixes it.

## Features

### Skinned Mesh Bone Checker
- **Recursive Bone Checking**: Checks all SkinnedMeshRenderers in the selected GameObject and its children.
- **Immediate Diagnosis**: Logs results instantly in the Unity Console to inform if there are missing bone references.

### Unity Skeleton Fixer
- **Automatic Bone Repair**: Detects and replaces missing bones with placeholder objects for all SkinnedMeshRenderers within the selected GameObject's children.
- **Non-Destructive**: Ensures the visual appearance of the mesh is not changed.

## Installation

1. Download last unitypackage: [UnityBoneFixer Releases](https://github.com/EchoVRC/UnityBoneFixer/releases)
2. **OR** Clone/download the repository.

## Usage

### Checking Bones
1. Select a GameObject in the Unity Editor's hierarchy view.
2. Right-click and choose `Check Skinned Mesh Bones` from the GameObject menu.
3. Review the Unity Console for messages about the integrity of the bones.

### Fixing Bones
1. Select the GameObject with SkinnedMeshRenderer components in the Unity Editor.
2. Navigate to `GameObject` in the menu bar.
3. Click on `Fix Skinned Mesh Bones in Children`.
4. A message in the Unity Console will confirm the completion of the operation.

## How It Works

### Skinned Mesh Bone Checker
- Triggers a recursive search through the selected GameObject's hierarchy.
- Checks each SkinnedMeshRenderer's bones array for null references.
- Reports back in the console about the status of each SkinnedMeshRenderer's bones.

### Unity Skeleton Fixer
- Scans for all SkinnedMeshRenderers within the children of the selected GameObject.
- Creates placeholder bones for any missing bone references and properly parents them under the root bone.
- Updates the bones array in SkinnedMeshRenderer to reference new placeholder bones.

## Contribution

Feel free to fork the repository, make changes, and submit a pull request if you have improvements or fixes to the tools. 

## Support

If you have any issues or questions about these tools, please file an issue in the GitHub repository.

## License

These tools are distributed under the MIT License. 
