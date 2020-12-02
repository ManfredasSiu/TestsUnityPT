using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class SceneManagerTests
    {

        // A Test behaves as an ordinary method
        [UnityTest]
        public IEnumerator CreateScene_IsNotNull()
        {
            var sceneName = "TestScene1";
            var scene = SceneManager.CreateScene(sceneName);
            sceneName = scene.name;

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
            Assert.IsNotNull(sceneName);
        }

        [UnityTest]
        public IEnumerator GetActiveScene_AreEqual()
        {
            var sceneName = "TestSubjectScene";

            var actual = SceneManager.GetActiveScene();
           
            Debug.Log("Test: GetActive_AreEqual currently active scene before load: " + actual.name);

            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            if (SceneManager.GetSceneByName("TestSubjectScene").name == null) Assert.Inconclusive("Test is inconclusive, scene was not loaded correctly");

            actual = SceneManager.GetActiveScene();
            var currentName = actual.name;
            Debug.Log("Test: GetActive_AreEqual currently active scene after load: " + currentName);

            yield return SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            Assert.AreEqual(sceneName, currentName);
        }
        
        [UnityTest]
        public IEnumerator SetActiveScene_isTrue()
        {
            var sceneName = "TestSubjectScene";
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            var loadedScene = SceneManager.GetSceneByName(sceneName);

            if (loadedScene.name == null) Assert.Inconclusive("Test is inconclusive, the scene was not loaded properly");

            var preActive = SceneManager.GetActiveScene();
            if (sceneName == preActive.name) Assert.Inconclusive("Test is inconclusive, the scene was active from the start");

            var actual = SceneManager.SetActiveScene(loadedScene);

            yield return SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            Assert.IsTrue(actual);
        }

        [UnityTest]
        public IEnumerator GetSceneByName_AreEqual()
        {
            var sceneName = "TestScene3";
            var scene = SceneManager.CreateScene(sceneName);

            if (scene.name == null) Assert.Inconclusive("Test is inconclusive, scene was not created correctly");

            var actual = SceneManager.GetSceneByName(sceneName);

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
            Assert.AreEqual(scene, actual);
        }

        [Test]
        public void GetSceneByName_IsNull()
        {
            var actual = SceneManager.GetSceneByName("NonExistentSceneName");

            Assert.IsNull(actual.name);
        }

        [UnityTest]
        public IEnumerator GetSceneByPath_IsNotNull()
        {
            var sceneName = "TestScene4";
            var scene = SceneManager.CreateScene(sceneName);

            if (scene.name == null) Assert.Inconclusive("Test is inconclusive, scene was not created correctly");

            var scenePath = scene.path;
            var actual = SceneManager.GetSceneByPath(scenePath);

            Debug.Log("Test: GetSceneByPath_IsNotNull scene: " + actual.name);

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
            Assert.IsNotNull(actual);
        }

        [UnityTest]
        public IEnumerator GetSceneAt_IsNotNull()
        {
            var sceneName = "TestScene5";
            var scene = SceneManager.CreateScene(sceneName);

            if (scene.name == null) Assert.Inconclusive("Test is inconclusive, scene was not created correctly");

            var actual = SceneManager.GetSceneAt(1); 

            Debug.Log("Test: GetSceneAt_NotNull scene: " + actual.name);

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
            Assert.IsNotNull(actual);
        }

        [Test]
        public void GetSceneAt_IOORException()
        {
            Assert.That(() => SceneManager.GetSceneAt(1), Throws.TypeOf<IndexOutOfRangeException>());
        }

        [Test]
        public void GetSceneByBuildIndex_IsNull()
        {
            var sceneName = "TestSubjectScene";

            if (SceneManager.GetSceneByName(sceneName).name != null)
                Assert.Inconclusive("Test is inconclusive, the scene was loaded: " + sceneName);

            var actual = SceneManager.GetSceneByBuildIndex(1);

            Assert.IsNull(actual.name);
        }

        [UnityTest]
        public IEnumerator GetSceneByBuildIndex_AreEqual()
        {
            var sceneName = "TestSubjectScene";

            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            var actualName = SceneManager.GetSceneByBuildIndex(1).name;

            yield return SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            Assert.AreEqual(sceneName, actualName);
        }

        [UnityTest]
        public IEnumerator UnloadSceneAsync_IsLoadedFalse()
        {
            var sceneName = "TestScene6";
            var scene = SceneManager.CreateScene(sceneName);

            if (scene.name == null) Assert.Inconclusive("Test is inconclusive, scene was not created correctly");

            if (scene.isLoaded == false) Assert.Inconclusive("Test is inconclusive, scene was not loaded after creation");
            else Debug.Log("Test: UnloadSceneAsync_IsLoadedFalse IsLoaded before unload: " + scene.isLoaded);

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));

            Assert.IsFalse(scene.isLoaded);
        }

        [UnityTest]
        public IEnumerator LoadSceneAsync_IsLoadedTrue()
        {
            var sceneName = "TestSubjectScene";

            var scene = SceneManager.GetSceneByName("TestSubjectScene");

            if (scene.name == null)
                Debug.Log("Test: LoadSceneAsync_IsLoadedTrue IsLoaded before load: " + false);
            else Assert.Inconclusive("Test is inconclusive, the scene was already loaded before load: " + scene.name);
            
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            var isLoaded = scene.isLoaded;

            yield return SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            Assert.IsFalse(isLoaded);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        /*[UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }*/
    }
}
