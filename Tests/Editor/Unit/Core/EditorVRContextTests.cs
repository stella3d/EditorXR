using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.Experimental.EditorVR.Core;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEditor.Experimental.EditorVR.Tools;
using EditorVR = UnityEditor.Experimental.EditorVR.Core.EditorVR;
using NSubstitute;

namespace UnityEditor.Experimental.EditorVR.Tests.Core
{
    [TestFixture]
    public class EditorVrContextTests
    {
        GameObject go;
        EditorVRContext context;

        [OneTimeSetUp]
        public void Setup()
        {
            go = new GameObject("add monobehaviours here");
            var transformTool = go.AddComponent<TransformTool>();
            var createPrimitiveTool = go.AddComponent<CreatePrimitiveTool>();
            //var evr = go.AddComponent<EditorVR.Core.EditorVR>();

            //Debug.Log(evr);

            context = ScriptableObject.CreateInstance<EditorVRContext>();
            context.m_DefaultToolStack = new List<MonoScript>();
            context.m_DefaultToolStack.Add(MonoScript.FromMonoBehaviour(transformTool));
            context.m_DefaultToolStack.Add(MonoScript.FromMonoBehaviour(createPrimitiveTool));

        }

        [Test]
        public void CheckSetupWorks()
        {
            Debug.Log(context.m_DefaultToolStack);
            Debug.Log(context.m_Instance);
            Debug.Log(EditorVR.Core.EditorVR.defaultTools);
            context.Setup();

            foreach (var t in EditorVR.Core.EditorVR.defaultTools)
            {
                Debug.Log(t.ToString());
            }
        }

        [TearDown]
        public void Cleanup()
        {
            ObjectUtils.Destroy(go);
            ObjectUtils.Destroy(context);
        }
    }
}


