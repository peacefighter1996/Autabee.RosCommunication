# Autabee.Roscommunicator
This repository contains a collection of objects to make it easier/possible to communicate with ROS from a .NET application. More specifically a Blazor WASM. This is partly to support UI development for the TU-Delft course `RO47007 Multidisciplinary Project 2023/24` for controlling the Mirte Master project. More information about Mirth can be found [here](https://www.mirte.org), Master version is not yet released outside of the course.

Basis of this project is from the Opc Scout project, which is a project that is used to communicate with OPC-UA servers. This project is also developed by the same group and can be found [here](https://github.com/Autabee/Autabee.OpcCommunication)

> **Warning:** This project is still in development and is not yet ready for production use. However, support and pull requests are welcome.

## Projects
The solution contains the following projects:

- **Autabee.Communication.RosClient**: This project contains helper classes and object to communicate with ROS.
- **Autabee.RosScout.BlazorWASM**: This project contains a Blazor WASM application that can be used to control a ROS robot.
- **Autabee.RosScout.BlazorWASM.Server**: This project contains the server side of the Blazor WASM application.

## Libraries
The following libraries are used in this project:

- **[RosSharp](https://github.com/Autabee/ros-sharp)**: Originally developed by Siemens, this library is used to communicate with ROS. However, due to stalled development and specific requirements, this library is forked and modified to support specific needs of this project. (Update follows soon)

## Course Usage
This project is developed as part of the TU-Delft course `RO47007 Multidisciplinary Project 2023/24` by one of the groups. However, this project is open source and can be used by anyone. But due to the nature of the project and course development, usage comes with the following conditions.

- **Attribution**: If you use this project is used in your project, you must attribute the original authors. This means that in documentation and presentation you must mention the original authors.
- **Credit**: Any contribution to this project you will be credited in the project documentation and presentation of the original authors.

## Screenshots

<figure class="image">
  <img src="https://github.com/Autabee/Autabee.RosScout/blob/main/scout.png?raw=true" alt="OpcScout">
  <figcaption>Ros Scout diagnostics view (Light)</figcaption>
</figure>

