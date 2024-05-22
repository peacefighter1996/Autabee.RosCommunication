# Autabee.Roscommunicator
This repository contains a collection of objects to make it easier/possible to communicate with ROS from a .NET application. More specifically a Blazor WASM. This is partly to support UI development for the TU-Delft course `RO47007 Multidisciplinary Project 2023/24` for controlling the Mirte Master project. More information about Mirth can be found [here](https://www.mirte.org), Master version is not yet released outside of the course.

Basis of this project is from the Opc Scout project, which is a project that is used to communicate with OPC-UA servers. This project is also developed by the same group and can be found [here](https://github.com/Autabee/Autabee.OpcCommunication)

> :warning: **Warning** 
> This project is still in development and is not yet ready for production use. However, support and pull requests are welcome.

## Current capabilities and features

- Able to talk with topics and services on a ROS bridge
- Able to extract data from the RosMaster 
  - Topics
  - Services
  - Nodes
  - Parameters [coming soon]
- Some initial UI design
- Translation from XMLRPC to C# objects for easier data handling
- Exposed Api With swagger for easy testing and development.
- But also allows other applications to communicate with the ROS bridge / Ros master through the API.

## Projects
The solution contains the following projects:

- **Autabee.Communication.RosClient**: This project contains helper classes and object to communicate with ROS.
- **Autabee.RosScout.BlazorWASM**: This project contains a Blazor WASM application that can be used to control a ROS robot.
- **Autabee.RosScout.ApiHost**: This project contains the API that is used to communicate with the ROS. This is the main project that should be started to use the API.
- **Autabee.RosScout.Components**: This project contains the components that are used in the Blazor WASM application. This is a shared project and can be used in other Blazor/Razor projects using RosClient.

## Libraries
The following libraries are used in this project:

- **[RosSharp](https://github.com/Autabee/ros-sharp)**: Originally developed by Siemens, this library is used to communicate with ROS. However, due to stalled development and specific requirements, this library is forked and modified to support specific needs of this project. (Update follows soon)

## Course Usage

This project is developed as part of the TU-Delft course `RO47007 Multidisciplinary Project 2023/24` by group 11. However, while this project is open source and can be used by anyone. Due to the nature of the project and course development, usage comes with the following conditions and social contract.  

- **Attribution**: If you use this project is used in your project, you must attribute the original authors. This means that in documentation and presentation you must mention the original authors.
- **Credit**: Any contribution to this project you will be credited in the project documentation and presentation of the original authors. Credit will only be given if merged into the main branch. And of significant `Meaningful contribution`. See below [To Do](#to-do) list for possible contributions. If you have any other ideas, please let us know. Contributors will be mentioned in the [Contributors](#contributors) section of the documentation.

This is to ensure that the original authors and contributors are credited for their work. While following the condition of unique work and plagiarism for the course.

## Screenshots

<figure class="image">
  <img src="https://github.com/peacefighter1996/Autabee.RosCommunication/blob/90990691e6e63a9a841ea797eb22f18efb37f295/scout.png" alt="OpcScout">
  <figcaption>Ros Scout diagnostics view (Light)</figcaption>
</figure>



## Getting Started
Following steps are needed to get the project up and running.

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 (Recommended) or Visual Studio Code
- Docker (Optional Useful for testing)

### Installation and Running
1. Clone the repository
   ```sh
   git clone --recursive https://github.com/peacefighter1996/Autabee.RosCommunication 
   cd Autabee.RosCommunication 
   git submodule update --init --recursive
    ```
2. Open the solution in IDE of choice
   - Visual Studio 2022
   Open the solution file `./src/Autabee.RosScout.sln` in Visual Studio 2022. From here you can build the `Autabee.RosScout.ApiHost` using the `http` or `https` profile.
   - Visual Studio Code 
   Open the folder `./src` in Visual Studio Code. From here you can build the `Autabee.RosScout.ApiHost` by running the following command in the terminal.
   ```sh
    dotnet build Autabee.RosScout.ApiHost
    ```
3. Run the project
    - Visual Studio 2022
    Press `F5` to run the project in debug mode. This will start the API and the Blazor WASM application.
    - Visual Studio Code
    Run the following command in the terminal to start the API.
    ```sh
      dotnet run --project Autabee.RosScout.ApiHost
    ```
    Or for development with hot reload use the following command.
    ```sh
      dotnet watch --project Autabee.RosScout.ApiHost
    ```
4. For testing in the `./scripts/test-ros-deployment` there is a docker-compose file that can be used to test the API with a ROS bridge. This is useful as this loosens the Linux requirement for development and testing.

## To Do

- [ ] Documentation for the project. Mostly in therms of architecture and how to use different parts of the project.
- [x] Get Subscription working through SignalR, currently the Web App connects directly to the ROS bridge, but this should not be the case as it comes with remote deployment issues when deployed in an inaccessible network. [15th may]
- [ ] Develop Controls for the Web App to control the robot
  - [ ] Move control 
  - [ ] Ultra Sonic Sensor feed
  - [ ] Camera feed (Challenging)
  - [ ] Arm/Gripper control
  - [ ] Map view (Challenging)
- [ ] Develop ID Node to be launched on robot for dynamic page acquisition. So that the Web App can be used for any robot. 
- [ ] Retrieve specifics about active services for dynamic control creation and validation of system.
- [ ] Authentication and Authorization for the Web API. (Challenging)

## Contributors

<table style="border:0px">
  <tbody>
    <tr style="border:0px">
      <td align="center" valign="top" width="14.28%" style="border:0px"><a href="https://github.com/peacefighter1996"><img src="https://avatars.githubusercontent.com/u/15609940?s=400&u=d4484d7398221a2b894f4328ad374064a319f3f2&v=4" width="100px;" alt="I Arbouw"/><br /><sub><b>I Arbouw</b></sub></a><br />🚧💻🎨</td>
    </tr>
  </tbody>
</table>
