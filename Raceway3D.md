## Raceway Network in 3D View

![Raceway Network](./TrayNetwork3D.png)

This example is based on a real-world application I have implemented for electrical designer and engineer. In engineering, we need to route cables between two equipments along the cable tray network. Designer typically models the equipment and the cable trays in a plant design CAD software like Hexagon Smart 3D or AVEVA E3D. The models are then exported as lines and nodes with 3D coordinates for the nodes. Such data now can be plotted as lines and points to provide a represented view of the cable tray network.

## Type of Lines

- Tray - white line, which is a physical tray to be installed
- Jump - green line, which defines an abstract path between physical trays even when these trays are not physically connected
- Drop - yellow line, which defines an abstract path where cables between from/to equipments can enter/leave the physical trays
- Selection - magenta line, which highlight selected raceway using left mouse click

## Type of Points

- Tray - white point, which defines a connection between trays and where cables can enter/leave the tray
- Equipment - red point, which defines the location of an equipment

## Manage Layers

- Hide Tray - toggle the display of Tray layer
- Hide Jump - toggle the display of Jump layer
- Hide Tray Node - toggle the display of Tray Node layer
- Hide Drop - toggle the display of Drop layer
- Hide Equipment Node - toggle the display of Equipment Node layer

## Other Possible Useful Visualization Features

- Display raceway by segregation system
- Display a line representing the direction of cable connecting between two equipment locations
- Highlight raceway of a cable route
- Display all the cables/routes from a single distribution center
- Highlight raceways, for example, to identify
    - Overfill cable tray
    - Sub-network of raceway that are not connected with one others

## Overlay Other 3D Models

Small set of other 3D models, such as buildings and walls, can be exported from plant design CAD software and imported into the application to provide context surrounding the raceway network.