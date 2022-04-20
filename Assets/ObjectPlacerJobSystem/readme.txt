Support : 
mail : pawelstupak2@gmail.com
page : http://pawelstupak.com/

Collider based object placement system

1. Setup 
System is based on class ObjectPlacementJobs. With passed data it places objects into scene after calling a SpawnObjects or SpawnObject method. Methods could be customized with some arguments. Check ObjectPlaceJob.cs to see more details. Main part of a method is passed delegate Func<PositionCheck, bool> RandomizeMethod. In this passed method you should generate random position, and rotation. Generated position and rotation should be save into passed PositionCheck class with Set(Vector3 position, Quaternion rotation) method. PositionCheck class is simple data structure class for saving data between called coroutines, since coroutine needed to return IEnumerator.  Next delegate Func<GameObject, bool> InitializeSpawnedObject providing current spawned object for our purposes. Passed method should initialize object, that way we need into project. There is also generic parameter <T> where T : CheckForCollisions provided to position checks methods. CheckForCollisions class is special class converting every object to physic active template of that object. It make sure to generate collision even on non physic active objects. CheckForCollisions must be a Monobehaviour class since its added to template object as a Component. Package contains also CheckForCollisions2D class which must be used to generate environment based on 2D colliders since it using Rigidbody2D component instead of Rigidbody. 
Worth noticing is that every moving object at spawning area can cause errors in collision checks. To prevent physic active object from moving is freeze attached rigid body on spawn and unfreeze they in ObjectPlacementJobs.onComplete event. PhysicsObject component can be used for that.

2. Creating a job
Jobs can be created as component itself like a BasicOPJ class or crated by passing parameters into manager. Creating job as a component should be created by write own class inheriting from BasicOPJ class. BasicOPJ provides public in inspector visible fields where we should customize job. New class should add data necessary to generate randomized position and rotation of object. You can override RandomizePosition, InitializeSpawnedObject and Run BasicOPJ class methods and call StartCoroutine(SpawnObjects) from Run method. That approach should allow to call jobs from provided with project simple OPJManager which is a singleton. 
Another approach is creating EmptyOPJ job with passed data. EmptyOPJ class is just a data container for necessary arguments. Storing a reference to created object allow us to run a job. General manager like OPJManager should have capability to create, store and run a job. CreateJob method in OPJManager class allow to create job with given name, and call it using that name. For moe information look OPJManager.cs.

3. Job Examples 
Package contains 3 major jobs examples. TerrainOPJ works with Unity Terrain component, MeshOPJ works with meshes and DoubleDimensionOBJ works with 2D sprites. Additional i put TextureTerrainOPJ, which works with expands TerrainOPJ to customize objects based on texture. 
TerrainOPJ job inside override RandomizePosition method generate random point and matching to point quaternion (based on vertex normal) into terrain surface. Override InitializeSpawnedObject method keep rigid body objects to be friezed during generate process. 
MeshOPJ job works in same way. RandomizePosition method looking for random point into mesh surface and matching quaternion to that position.
DoubleDimensionOBJ is job works with 2D colliders. Main difference between generating 3D and 2D objects is passed generic <T> parameter into position checks methods. 3D colliders must use CheckForCollisions component to handle collisions, since 2D colliders were handled by CheckForCollisions2D component. 

4. Examples
Provides scenes shows examples of jobs usage and different approach to manager and call them. 
TerrainExample scene contains terrain based jobs. Jobs are added as components to ObjectPlacerJob game object and data are passed by dragging assets into inspector window. SceneManager object had added TerrainSceneExample component which call Run method for passed job.
PlanetiodExample scene contains mesh based jobs and using same approach as terrain example. Instead of terrain based job, ObjectPlacerJob game object had added MeshOPJ component.
2DExample scene contains 2D sprites example. In this example ObjectPlacerJobsManager game object contains OPJManager component which manage jobs into entire scene. Jobs registers itself to manager with given name. Jobs should be called by OPJManager TryRunJob method with passing current job name into method.


