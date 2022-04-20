# collision-based-object-placement

Unity package available at https://assetstore.unity.com/packages/tools/physics/collider-based-perfect-object-placement-system-116503 !

Simple to use procedural system to place objects based on their colliders for Unity engine. System engages Unity Physic build system (PhysX) for accurate collision detection. It already support multiple colliders even in child's game objects, rigidbodies and every collider's type.

Due to this the system has few limitations, such as:
- system cant generate object in one frame, it using coroutines to check any collisions beetwen objects (it's necessary to wait for physic engine update);
- system need to be set in run time (physic engine cant be updated into edit mode);

IMPORTANT!
Assets into video or preview are downloaded for free from asset store and they arent included into package. They are only used to show how system works. Instead of them into examples I've used placeholders models/sprites. Package contains full system and prefabs to show current system possibilites.

KNOWN ISSUES
- terrain checker doesn't work with unity new terrain system 

MEDIA

http://www.youtube.com/watch?v=5iAGp1t1s_Y - Showcase

http://www.youtube.com/watch?v=1xYgVOe2NKg - System Preview

![14fac392-26b4-4b0d-956e-f3774189ef8b_orig](https://user-images.githubusercontent.com/22291563/164237785-2815f009-f5f2-4cb8-bdef-48d52e6d9556.png)
![c54c766b-071c-4a26-a1b0-913c3b605cfc_orig](https://user-images.githubusercontent.com/22291563/164237789-43b52fa9-416e-4ecb-b620-05d4ed7fedad.png)
![c313fd56-130a-4d70-abb1-79617137c650_orig](https://user-images.githubusercontent.com/22291563/164237792-afd72a41-8332-489d-be50-b39041db40bc.png)
![5b0ed7b7-cd13-447e-a13e-027bfb930069_orig](https://user-images.githubusercontent.com/22291563/164237794-b25ca25e-84e8-44c8-8078-6bcd5cf524ee.png)
![9ce1e4a7-2ad2-4688-bab4-6b9ec6d9c728_orig](https://user-images.githubusercontent.com/22291563/164237796-af16b2aa-d45f-47e9-9c30-dbeab9418a98.gif)
