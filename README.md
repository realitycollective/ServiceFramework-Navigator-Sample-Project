# Service Framework Navigator Sample-Project

A Service Framework sample project showcasing features in an AR navigator experience.

## Overview

The Navigator is a very simple example demonstrating both the recommended implementation of the Service Framework and some sample services.  The purpose of the sample app is to showcase a basic AR solution using ARFoundation Image Tracking and asset placement.

## The Services

The sample provides two in-built services for the sample and a packaged service from the Reality Collective.  The implementations are not meant to be extensive but to showcase the ease of their use throughout the application.

|Service Name|Location|Purpose|
|-|-|-|
|SceneLoader|`Assets/Application/Scripts/Services`|This service provides a robust extension to the Unity SceneManagement capabilities and allows you to centrally control scene operations|
|ApplicationSettings|`Assets/Application/Scripts/Services`|A very basic settings service that holds application state and configuration for the entire application|
|Spatial Persistence|Unity git Package|The development version of the Reality Collective Spatial Persistence service with the AR Foundation Image Tracking module installed|

## Scenes

The structure of the sample is split into three scenes:

|Scene Name|Location|Purpose|
|-|-|-|
|BaseScene|`Assets/Application/Scenes`|This is the initialization scene for the application containing only the Service Framework `GlobalServicesManager`, this loads services and launches the application.|
|StartScene|`Assets/Application/Scenes`|The main application scene which contains the `ApplicationManager` MonoBehaviour component that runs the application from the service configuration.|
|AboutScene|`Assets/Application/Scenes`|A base scene that is loaded via the `SceneLoader` and then integrates an option to unload it.|

## Sample Operation

The basic flow for the sample has been kept simple so that it can focus on the Service Framework implementation.

1. Application starts and launches the Service Framework which initializes all registered services.
1. The `SceneLoader` service inspects the configuration managed by the `ApplicationSettings` service and launches the relevant scene (`StartScene`).
1. The `StartScene` loads and starts up the Image Tracking capabilities of the application and loads configured "Trackable Images" for detection from the `ApplicationSettings` configuration.
1. A single Trackable Asset and corresponding prefab are registered with the `Spatial Persistence` service which then registers the image with the ARFoundation Module.

    > [!NOTE]
    > The `Spatial Persistence` service can maintain multiple modules for different types of Spatially Tracked systems, such as Azure Spatial Anchors.  The premise is simple, a location is registered with the vendor service and when that location is detected, a GameObject placed in that position is returned from the `Spatial Persistence` for consumption by the application.

1. Once an image is recognized, the `Spatial Persistence` will return an `ID` and `GameObject` for the detected location, and the application compares this to the configured assets and instantiates the required prefab as a child of the tracked location.

    > [!NOTE]
    > The `Spatial Persistence ARFoundation` component only integrates with `ARFoundation Image Tracking`, all the positioning and recognition features are provided by the underlying functionality, as such any limitations from the functionality need to be managed by the developer (as if you implemented it yourself). The purpose of the `Spatial Persistence` service and its modules is to greatly simplify and unify all implementations behind the service.  So it is the same registration call, the same detection call for ANY provider, making a single interaction pattern for use of many services.

## Further reading

For more information about how the `Service Framework` operates and how to build/query services, please check out our [online documentation](https://serviceframework.realitycollective.net/):

### [https://serviceframework.realitycollective.net/](https://serviceframework.realitycollective.net/)
