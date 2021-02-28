using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace uMMORPG.Tests
{
    public class UtilsTests
    {
        [Test]
        public void Clamp()
        {
            Assert.That(Utils.Clamp(11, 0, 10), Is.EqualTo(10));
            Assert.That(Utils.Clamp( 5, 0, 10), Is.EqualTo(5));
            Assert.That(Utils.Clamp(-1, 0, 10), Is.EqualTo(0));
        }

        [Test]
        public void BoundsRadius()
        {
            Assert.That(Utils.BoundsRadius(new Bounds(Vector3.zero, Vector3.one)), Is.EqualTo(0.5f));
            Assert.That(Utils.BoundsRadius(new Bounds(Vector3.one, Vector3.one)), Is.EqualTo(0.5f));
        }

        class MockSkills : Skills {}

        class MockEntity : Entity
        {
            protected override void OnInteract() {}
        }

        Entity CreateMockEntityWithCollider(Vector3 colliderSize)
        {
            // prepare two entities with colliders
            GameObject gameObject = new GameObject();
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.size = colliderSize;
            gameObject.AddComponent<MockSkills>(); // required by Entity
            Entity entity = gameObject.AddComponent<MockEntity>();

            // assign component
            entity.collider = collider;
            return entity;
        }

        [Test]
        public void ClosestDistanceFarApart()
        {
            // prepare two entities with colliders
            Entity a = CreateMockEntityWithCollider(Vector3.one);
            Entity b = CreateMockEntityWithCollider(Vector3.one * 2);
            b.transform.position = new Vector3(0, 0, 2);

            // calculate closest distance
            //   a is at (0,0,0) with size (1,1,1)
            //   b is at (0,0,2) with size (2,2,2)
            //   => distance from center to center is 2
            //   => a bounds radius is 0.5
            //   => b bounds radius is 1
            //   ==> 2 - 0.5 - 1 = 0.5
            Assert.That(Utils.ClosestDistance(a, b), Is.EqualTo(0.5f));

            // cleanup
            GameObject.DestroyImmediate(a.gameObject);
            GameObject.DestroyImmediate(b.gameObject);
        }

        [Test]
        public void ClosestDistanceOverlapping()
        {
            // prepare two entities with colliders
            Entity a = CreateMockEntityWithCollider(Vector3.one);
            Entity b = CreateMockEntityWithCollider(Vector3.one);
            b.transform.position = new Vector3(0, 0, 0.5f);

            // calculate closest distance
            //   a is at (0,0,0) with size (1,1,1)
            //   b is at (0,0,0.5) with size (1,1,1)
            //   => their colliders overlap so distance should be 0
            Assert.That(Utils.ClosestDistance(a, b), Is.EqualTo(0));

            // cleanup
            GameObject.DestroyImmediate(a.gameObject);
            GameObject.DestroyImmediate(b.gameObject);
        }

        [Test]
        public void ClosestPoint()
        {
            // prepare an entity with collider at non default position
            Entity entity = CreateMockEntityWithCollider(Vector3.one);
            entity.transform.position = new Vector3(0, 0, 2);

            // calculate closest point on collider to center
            //   entity is at (0,0,2) with size (1,1,1)
            //   => collider radius is 0.5
            //   => z = 2 - 0.5
            Assert.That(Utils.ClosestPoint(entity, Vector3.zero), Is.EqualTo(new Vector3(0, 0, 1.5f)));

            // cleanup
            GameObject.DestroyImmediate(entity.gameObject);
        }

        [Test]
        public void GetNearestTransform()
        {
            // create two transforms
            GameObject gameObjectA = new GameObject();
            GameObject gameObjectB = new GameObject();
            gameObjectA.transform.position = Vector3.one;

            // add to list
            List<Transform> transforms = new List<Transform>{
                gameObjectA.transform,
                gameObjectB.transform
            };

            // find closest to a point near B
            Assert.That(Utils.GetNearestTransform(transforms, new Vector3(2, 2, 2)), Is.EqualTo(gameObjectB.transform));

            // clean up
            GameObject.DestroyImmediate(gameObjectA);
            GameObject.DestroyImmediate(gameObjectB);
        }

        [Test]
        public void PrettySeconds()
        {
            Assert.That(Utils.PrettySeconds(0), Is.EqualTo("0s"));
            Assert.That(Utils.PrettySeconds(0.5f), Is.EqualTo("0.5s"));
            Assert.That(Utils.PrettySeconds(1), Is.EqualTo("1s"));
            Assert.That(Utils.PrettySeconds(1.5f), Is.EqualTo("1.5s"));
            Assert.That(Utils.PrettySeconds(65), Is.EqualTo("1m 5s"));
            Assert.That(Utils.PrettySeconds(60 * 61 + 5), Is.EqualTo("1h 1m 5s"));
            Assert.That(Utils.PrettySeconds(24 * 60 * 60 + 60 * 61 + 5), Is.EqualTo("1d 1h 1m 5s"));
        }

        [Test]
        public void ParseLastNoun()
        {
            Assert.That(Utils.ParseLastNoun(""), Is.EqualTo(""));
            Assert.That(Utils.ParseLastNoun("EquipmentWeapon"), Is.EqualTo("Weapon"));
            Assert.That(Utils.ParseLastNoun("EquipmentWeaponShield"), Is.EqualTo("Shield"));
        }

        [Test]
        public void PBKDF2Hash()
        {
            Assert.That(Utils.PBKDF2Hash("text", "salt_eight_bytes"), Is.EqualTo("FE4FDF101BE6607509F67058AB533EC98896DF59"));
        }

        class MethodsByPrefixMock
        {
            public void Method_First() {}
            public void Method_Second() {}
        }

        [Test]
        public void GetMethodsByPrefix()
        {
            MethodInfo[] methods = Utils.GetMethodsByPrefix(typeof(MethodsByPrefixMock), "Method_");
            Assert.That(methods.Length, Is.EqualTo(2));
            Assert.That(methods[0].Name, Is.EqualTo(nameof(MethodsByPrefixMock.Method_First)));
            Assert.That(methods[1].Name, Is.EqualTo(nameof(MethodsByPrefixMock.Method_Second)));
        }

        [Test]
        public void ClampRotationAroundXAxis()
        {
            // clamp 45 between 0 and 90 => no change
            Quaternion clamped = Utils.ClampRotationAroundXAxis(Quaternion.Euler(45, 10, 20), 0, 90);
            Debug.Log($"unclamped: {clamped.eulerAngles}");
            Assert.That(clamped.eulerAngles.x, Is.EqualTo(45).Within(0.01f));
            Assert.That(clamped.eulerAngles.y, Is.EqualTo(10).Within(0.01f));
            Assert.That(clamped.eulerAngles.z, Is.EqualTo(20).Within(0.01f));

            // clamp 45 between 50 and 90 on lower end => 50
            // note that we need a high tolerance at the moment
            clamped = Utils.ClampRotationAroundXAxis(Quaternion.Euler(45, 10, 20), 50, 90);
            Debug.Log($"clamped on lower bound: {clamped.eulerAngles}");
            Assert.That(clamped.eulerAngles.x, Is.EqualTo(50).Within(2f));
            Assert.That(clamped.eulerAngles.y, Is.EqualTo(10).Within(2f));
            Assert.That(clamped.eulerAngles.z, Is.EqualTo(20).Within(2f));

            // clamp 45 between 10 and 40 on higher end => 90
            // note that we need a high tolerance at the moment
            clamped = Utils.ClampRotationAroundXAxis(Quaternion.Euler(45, 10, 20), 10, 40);
            Debug.Log($"clamped on upper bound: {clamped.eulerAngles}");
            Assert.That(clamped.eulerAngles.x, Is.EqualTo(40).Within(2f));
            Assert.That(clamped.eulerAngles.y, Is.EqualTo(10).Within(2f));
            Assert.That(clamped.eulerAngles.z, Is.EqualTo(20).Within(2f));

            // test case from our third person camera which is easy to miss
            // because of negative values.
            //   clamped between -70 and 70
            //   camera goes from -70 to -75
            //   result should be -70 and not 0 or anything else
            //   (wrapped around 0..360 => -70 + 360 => 290
            // example from third person camera which is clamped between -70 and 70
            // and when rotating it to see character from below, camera x rotation
            // becomes -75 etc.
            // => result should be -70, or 360-70 wrapped
            clamped = Utils.ClampRotationAroundXAxis(Quaternion.Euler(-75, 0, 0), -70, 70);
            Debug.Log($"clamped third person: {clamped.eulerAngles}");
            Assert.That(clamped.eulerAngles.x, Is.EqualTo(-70 + 360).Within(0.01f));
            Assert.That(clamped.eulerAngles.y, Is.EqualTo(0).Within(0.01f));
            Assert.That(clamped.eulerAngles.z, Is.EqualTo(0).Within(0.01f));
        }
    }
}
