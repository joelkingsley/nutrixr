# NutriXR

A Virtual Reality application for nutrition education and awareness, built with Unity and Meta XR SDK. NutriXR provides immersive experiences in virtual supermarket and kitchen environments to help users make informed food choices and understand nutritional and environmental impacts.

## Description

NutriXR is an educational VR application that combines interactive shopping and cooking experiences with real-time feedback on nutritional choices and environmental impact. The application features:

- **Virtual Supermarket**: Browse and select ingredients in a realistic 3D supermarket environment
- **Virtual Kitchen**: Cook recipes using selected ingredients with interactive kitchen appliances
- **Nutrition Feedback**: Visual feedback systems showing nutritional content and health impacts
- **Environmental Impact**: Real-time feedback on the environmental effects of food choices
- **Recipe System**: Predefined recipes with ingredient tracking and cooking simulation
- **Data Logging**: Comprehensive tracking of user interactions and choices for research purposes
- **Multiplayer Support**: Network-enabled collaborative experiences

## Features

- 🥽 **VR Experience**: Full immersion using Meta Quest/Oculus headsets
- 🛒 **Interactive Shopping**: Realistic supermarket with grabbable products
- 👨‍🍳 **Cooking Simulation**: Virtual kitchen with working appliances and cooking mechanics
- 📊 **Nutrition Visualization**: Real-time feedback on nutritional content
- 🌱 **Environmental Awareness**: Visual feedback on environmental impact of food choices
- 📈 **Progress Tracking**: Data logging and analytics for research purposes
- 🤝 **Collaborative Mode**: Multi-user support for shared experiences
- 🎯 **Educational Goals**: Customizable learning objectives and feedback modes

## System Requirements

### Minimum Requirements
- **VR Headset**: Meta Quest 2, Quest 3, or Oculus Rift
- **Unity Version**: Unity 2022.3.0f1 or later
- **Platform**: Windows 10/11 (for development)
- **RAM**: 8GB minimum, 16GB recommended
- **Storage**: 5GB available space

### Development Requirements
- Unity 2022.3.0f1
- Meta XR SDK 62.0.0
- Visual Studio 2019/2022 or VS Code
- Git LFS (for 3D models and assets)

## Installation

### For Developers

1. **Clone the repository**
   ```bash
   git clone https://git.uni-paderborn.de/awab/nutrixr.git
   cd nutrixr
   ```

2. **Install Unity Hub and Unity 2022.3.0f1**
   - Download from [Unity Download Page](https://unity3d.com/get-unity/download)
   - Install Unity Hub
   - Install Unity 2022.3.0f1 through Unity Hub

3. **Open the Project**
   - Open Unity Hub
   - Click "Add" and select the `nutrixr/NutriXR` folder
   - Open the project in Unity

4. **Configure XR Settings**
   - Ensure Meta XR SDK packages are properly imported
   - Configure build settings for your target VR platform
   - Set up the appropriate XR Management settings

5. **Build and Deploy**
   - Connect your VR headset
   - Switch platform to Android (for Quest) or Windows (for PC VR)
   - Build and deploy to your device

### For End Users

1. Install the built APK on your Meta Quest device
2. Enable Developer Mode on your headset
3. Launch NutriXR from your library
4. Follow the in-app tutorial to get started

## Usage

### Getting Started

1. **Launch the Application**: Put on your VR headset and start NutriXR
2. **Choose Your Goal**: Select between nutrition-focused or environment-focused feedback
3. **Enter the Supermarket**: Browse and select ingredients for your recipes
4. **Cook in the Kitchen**: Use the ingredients to prepare meals in the virtual kitchen
5. **View Feedback**: Receive visual feedback on your nutritional and environmental choices

### Key Interactions

- **Hand Tracking/Controllers**: Grab and manipulate objects
- **Recipe Selection**: Choose from available recipes at cooking stations
- **Ingredient Shopping**: Pick up items from supermarket shelves
- **Cooking Actions**: Use ovens, mixers, and other kitchen appliances
- **Feedback Review**: View nutrition circles and environmental impact bars

## Project Structure

```
NutriXR/
├── Assets/
│   ├── Scenes/
│   │   ├── StartScene.unity          # Main menu and setup
│   │   ├── Supermarket/              # Shopping environment
│   │   ├── Kitchen/                  # Cooking environment
│   │   └── Global/                   # Shared scripts and systems
│   ├── Resources/                    # Game assets and data
│   ├── Plugins/                      # Third-party integrations
│   └── XR/                          # VR-specific configurations
├── Packages/                         # Unity package dependencies
├── ProjectSettings/                  # Unity project configuration
└── WorkingFiles/                     # Development assets and documentation
    ├── Blender Files/               # 3D model source files
    └── Documents/                   # Research and design documents
```

## Development

### Key Scripts

- `Recipe.cs` - Defines recipe data structure with ingredients and weights
- `KitchenFeedbackSystem.cs` - Handles nutrition and environmental feedback
- `DataLogger.cs` - Tracks user interactions for research purposes
- `IngredientItem.cs` - Manages individual ingredient properties
- `BasketRecipeSystem.cs` - Handles recipe completion logic

### Adding New Features

1. **New Ingredients**: Create new ingredient ScriptableObjects
2. **New Recipes**: Define recipes using the Recipe system
3. **Feedback Systems**: Extend the feedback classes for new metrics
4. **Scenes**: Add new environments following the existing structure

## Research Integration

NutriXR includes comprehensive data logging for research purposes:

- User interaction tracking
- Choice logging with timestamps
- Nutrition feedback effectiveness
- Environmental awareness metrics
- Movement and behavior patterns

Data is logged locally and can be exported for analysis.

## Contributing

This project is part of ongoing research at the University of Paderborn. For collaboration opportunities or research partnerships, please contact the development team.

### Guidelines

1. Follow Unity coding conventions
2. Test all VR interactions thoroughly
3. Maintain compatibility with Meta XR SDK
4. Document any new features or systems
5. Ensure data privacy compliance for research logging

## Support

For technical issues or questions:
- Check the Unity Console for error messages
- Verify VR headset compatibility and setup
- Ensure all required packages are properly installed
- Review the data logging output for debugging information

## Authors and Acknowledgment

- **Development Team**: University of Paderborn
- **Research Supervision**: [Add supervisor names]
- **3D Assets**: Custom models created for educational purposes
- **Unity Technologies**: For the Unity engine and XR framework
- **Meta**: For the XR SDK and VR platform support

## License

This project is developed for research and educational purposes. Please contact the development team regarding usage and distribution rights.

## Project Status

This project is actively under development as part of ongoing research into VR-based nutrition education. The current version includes core functionality for supermarket shopping, kitchen cooking, and feedback systems.

### Current Features
- ✅ VR supermarket environment
- ✅ Virtual kitchen with cooking mechanics  
- ✅ Nutrition feedback visualization
- ✅ Environmental impact feedback
- ✅ Data logging and analytics
- ✅ Multi-user networking support

### Planned Features
- 🔄 Expanded ingredient database
- 🔄 Additional recipe categories
- 🔄 Enhanced feedback visualizations
- 🔄 Mobile companion app
- 🔄 Advanced analytics dashboard
