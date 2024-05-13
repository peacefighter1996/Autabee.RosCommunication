FROM ros:noetic-ros-core-focal

# install ros package
RUN apt-get update 
RUN apt-get install -y ros-noetic-rosbridge-suite

# launch ros package
CMD ["roslaunch", "rosbridge_server", "rosbridge_websocket.launch"]