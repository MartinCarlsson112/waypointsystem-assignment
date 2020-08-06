# waypointsystem-assignment

see waypointsystem-assignment.scene for example usage.

Features two components:

WaypointGroup

Stores information about the waypoints and exposes them to the entities. Uses a custom inspector to display position handles, and display/edit the underlying array of waypoints. The user can move the position handles in the scene view to change the positions of the waypoints. The user can also change the positions directly by changing the values in the inspector.

In the inspector, the user can also:

Add elements (The + button).

Remove an element (the - Button).

Change the order of the elements (the ↑ and ↓ Buttons).

The WaypointGroup also supports looping waypoints.


WaypointWalker

Interacts with the WaypointGroup to get waypoints to move to. Walks between the waypoints, either goes back and forth through all the waypoints, or cycles through them (depending on the setting in the WaypointGroup). To use the WaypointWalker, a WaypointGroup must be set in reference field of the inspector. The WaypointWalker and WaypointGroup can either be on the same GameObject or on seperate GameObjects, allowing multiple entities to share the same waypoints.

