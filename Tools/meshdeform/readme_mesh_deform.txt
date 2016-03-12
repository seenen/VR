
 Author:    Gabriel Morgenstern
 Contact:   info@vidiludi.com


#### DISCLAIMER ############################################

Use this script/package on your own risk! If your PC blows up or any other bad things happen it's not my fault ;)


#### NOTE ##################################################

If this package helps you, if you are using it in a commercial project or if you want/need support, 
i would be happy about a small donation.

donate here: http://vidiludi.com/?page_id=8


#### DESCRIPTION ###########################################

A little description of the CarDamage2.cs script's params:

- maxMoveDelta: 

     How heavy will the mesh be damaged on impact (distance in meters).

- maxCollisionStrength: 

     Put the highest impact value here or any value which should be the maximum; 
     the less the value is, the higher the damage will be.

- yforcedamp: 

     damp gravity force / A low value will save your car from
     high damages after jumps.

- demolutionRange: 

     How far will the collision spread (in meters)/in which radius will the vertices be moved.

- impactDirManipulator: 

     Combine the collisiondir with the direction to the middle of the car.

- optionalmeshlist: 

     Put the meshes of the car here which should be deformed 
     (if you leave this list empty, the script will take all meshes found - also in children).