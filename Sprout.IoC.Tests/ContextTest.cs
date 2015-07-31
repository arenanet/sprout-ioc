/*
 * Copyright 2015 ArenaNet, LLC.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this 
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * 	 http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under 
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF 
 * ANY KIND, either express or implied. See the License for the specific language governing 
 * permissions and limitations under the License.
 */
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArenaNet.Sprout.IoC;

using TestSimple.Namespace;
using TestCyclic.Namespace;
using TestLifecycle.Namespace;
using TestInjectionScope.Namespace;
using TestComponentNotFound.Namespace;
using TestComponentName.Namespace;

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// Tests the Context.
    /// </summary>
    [TestClass]
    public class ContextTest
    {
        [TestMethod]
        public void TestEmpty()
        {
            using (Context context = new Context())
            {
                context.Start();

                Assert.AreEqual(ContextState.Started, context.State);

                Assert.AreEqual(0, context.GetComponents().Count);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentNotFoundException))]
        public void TestStartComponentNotFound_Error()
        {
            using (Context context = new Context())
            {
                context.Register<TestComponentNotFoundClass>().Start();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentNameException))]
        public void TestRegisterUsingGenericsComponentName_Error()
        {
            using (Context context = new Context())
            {
                context.Register<TestComponentNameClass1>().Register<TestComponentNameClass2>();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidContextStateException))]
        public void TestRegisterUsingGenericsInvalidState_Error()
        {
            using (Context context = new Context())
            {
                context.Start();
                context.Register<TestClass1>();
            }
        }

        [TestMethod]
        public void TestRegisterUsingGenerics()
        {
            using (Context context = new Context())
            {
                context.Register<TestClass1>()
                        .Register<TestClass2>()
                        .Start();

                Assert.AreEqual(ContextState.Started, context.State);

                Assert.AreEqual(2, context.GetComponents().Count);
                Assert.IsNotNull(context.GetComponent<TestClass1>());
                Assert.IsNotNull(context.GetComponent<TestClass2>());
                Assert.AreEqual(context.GetComponent<TestClass1>().ReferencedTestClass, context.GetComponent<TestClass2>());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentNameException))]
        public void TestRegisterUsingTypeComponentName_Error()
        {
            using (Context context = new Context())
            {
                context.Register(typeof(TestComponentNameClass1)).Register(typeof(TestComponentNameClass2));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidContextStateException))]
        public void TestRegisterUsingTypeInvalidState_Error()
        {
            using (Context context = new Context())
            {
                context.Start();
                context.Register(typeof(TestClass1));
            }
        }

        [TestMethod]
        public void TestRegisterUsingType()
        {
            using (Context context = new Context())
            {
                context.Register(typeof(TestClass1))
                        .Register(typeof(TestClass2))
                        .Start();

                Assert.AreEqual(ContextState.Started, context.State);

                Assert.AreEqual(2, context.GetComponents().Count);
                Assert.IsNotNull(context.GetComponent<TestClass1>());
                Assert.IsNotNull(context.GetComponent<TestClass2>());
                Assert.AreEqual(context.GetComponent<TestClass1>().ReferencedTestClass, context.GetComponent<TestClass2>());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentNameException))]
        public void TestScanUsingGenericsComponentName_Error()
        {
            using (Context context = new Context())
            {
                context.Scan<TestComponentNameClass1>();
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidContextStateException))]
        public void TestScanUsingGenericsInvalidState_Error()
        {
            using (Context context = new Context())
            {
                context.Start();
                context.Scan<TestClass1>();
            }
        }

        [TestMethod]
        public void TestScanUsingGenerics()
        {
            using (Context context = new Context())
            {
                context.Scan<TestClass1>()
                        .Start();

                Assert.AreEqual(ContextState.Started, context.State);

                Assert.AreEqual(2, context.GetComponents().Count);
                Assert.IsNotNull(context.GetComponent<TestClass1>());
                Assert.IsNotNull(context.GetComponent<TestClass2>());
                Assert.AreEqual(context.GetComponent<TestClass1>().ReferencedTestClass, context.GetComponent<TestClass2>());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentNameException))]
        public void TestScanUsingTypeComponentName_Error()
        {
            using (Context context = new Context())
            {
                context.Scan(typeof(TestComponentNameClass1));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidContextStateException))]
        public void TestScanUsingTypeInvalidState_Error()
        {
            using (Context context = new Context())
            {
                context.Start();
                context.Scan(typeof(TestClass1));
            }
        }

        [TestMethod]
        public void TestScanUsingType()
        {
            using (Context context = new Context())
            {
                context.Scan(typeof(TestClass1))
                        .Start();

                Assert.AreEqual(ContextState.Started, context.State);

                Assert.AreEqual(2, context.GetComponents().Count);
                Assert.IsNotNull(context.GetComponent<TestClass1>());
                Assert.IsNotNull(context.GetComponent<TestClass2>());
                Assert.AreEqual(context.GetComponent<TestClass1>().ReferencedTestClass, context.GetComponent<TestClass2>());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ComponentNameException))]
        public void TestScanUsingNamespaceComponentName_Error()
        {
            using (Context context = new Context())
            {
                context.Scan("TestComponentName.Namespace");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidContextStateException))]
        public void TestScanUsingNamespaceInvalidState_Error()
        {
            using (Context context = new Context())
            {
                context.Start();
                context.Scan("TestComponentName.Namespace");
            }
        }

        [TestMethod]
        public void TestScanUsingNamespace()
        {
            using (Context context = new Context())
            {
                context.Scan("TestSimple.Namespace")
                        .Start();

                Assert.AreEqual(ContextState.Started, context.State);

                Assert.AreEqual(2, context.GetComponents().Count);
                Assert.IsNotNull(context.GetComponent<TestClass1>());
                Assert.IsNotNull(context.GetComponent<TestClass2>());
                Assert.AreEqual(context.GetComponent<TestClass1>().ReferencedTestClass, context.GetComponent<TestClass2>());
            }
        }

        [TestMethod]
        public void TestGetComponentByName()
        {
            using (Context context = new Context())
            {
                context.Scan("TestSimple.Namespace")
                        .Start();

                Assert.AreEqual(ContextState.Started, context.State);

                Assert.IsNotNull(context.GetComponent("TestClass1"));
                Assert.IsTrue(context.GetComponent("TestClass1") is TestClass1);
            }
        }

        [TestMethod]
        public void TestGetComponentByType()
        {
            using (Context context = new Context())
            {
                context.Scan("TestSimple.Namespace")
                        .Start();

                Assert.AreEqual(ContextState.Started, context.State);

                Assert.IsNotNull(context.GetComponent<TestClass1>());
            }
        }

        [TestMethod]
        public void TestGetComponentByNameAndType()
        {
            using (Context context = new Context())
            {
                context.Scan("TestSimple.Namespace")
                        .Start();

                Assert.AreEqual(ContextState.Started, context.State);

                Assert.IsNotNull(context.GetComponent("TestClass1", typeof(TestClass1)));
                Assert.IsTrue(context.GetComponent("TestClass1", typeof(TestClass1)) is TestClass1);
            }
        }

        [TestMethod]
        public void TestGetComponentByNameAndTypeGeneric()
        {
            using (Context context = new Context())
            {
                context.Scan("TestSimple.Namespace")
                        .Start();

                Assert.AreEqual(ContextState.Started, context.State);

                Assert.IsNotNull(context.GetComponent<TestClass1>("TestClass1"));
            }
        }

        [TestMethod]
        public void TestScanWithCyclicInjection()
        {
            using (Context context = new Context())
            {
                context.Scan("TestCyclic.Namespace")
                        .Start();

                Assert.AreEqual(ContextState.Started, context.State);

                Assert.AreEqual(2, context.GetComponents().Count);
                Assert.IsNotNull(context.GetComponent<TestCyclicClass1>());
                Assert.IsNotNull(context.GetComponent<TestCyclicClass2>());
                Assert.AreEqual(context.GetComponent<TestCyclicClass1>().ReferencedTestClass, context.GetComponent<TestCyclicClass2>());
                Assert.AreEqual(context.GetComponent<TestCyclicClass2>().ReferencedTestClass, context.GetComponent<TestCyclicClass1>());
            }
        }

        [TestMethod]
        public void TestLifecycle()
        {
            TestLifecycleClass obj = null;

            using (Context context = new Context())
            {
                context.Scan<TestLifecycleClass>()
                        .Start();

                Assert.AreEqual(ContextState.Started, context.State);

                obj = context.GetComponent<TestLifecycleClass>();

                Assert.IsNotNull(obj);
                Assert.IsTrue(obj.OnStartInvoked);
                Assert.IsFalse(obj.OnStopInvoked);
            }

            Assert.IsTrue(obj.OnStartInvoked);
            Assert.IsTrue(obj.OnStopInvoked);
        }

        [TestMethod]
        public void TestInjectionScope()
        {
            using (Context context = new Context())
            {
                context.Scan("TestInjectionScope.Namespace")
                        .Start();

                Assert.AreEqual(ContextState.Started, context.State);

                Assert.AreEqual(2, context.GetComponents().Count);

                Assert.IsNotNull(context.GetComponent<TestInjectionScopeClass1>());
                Assert.IsNotNull(context.GetComponent<TestInjectionScopeClass2>());
                Assert.IsNull(context.GetComponent<TestInjectionScopeClass3>());

                Assert.IsNotNull(context.GetComponent<TestInjectionScopeClass1>().ReferencedTestClass);
                Assert.IsNotNull(context.GetComponent<TestInjectionScopeClass2>().ReferencedTestClass);

                Assert.AreNotEqual(context.GetComponent<TestInjectionScopeClass1>().ReferencedTestClass, 
                    context.GetComponent<TestInjectionScopeClass2>().ReferencedTestClass);
            }
        }
    }
}

namespace TestComponentNotFound.Namespace
{
    [Component(Name = "TestComponentNotFoundClass")]
    class TestComponentNotFoundClass
    {
        [Inject]
        public TestClass1 ReferencedTestClass { set; get; }
    }
}

namespace TestComponentName.Namespace
{
    [Component(Name = "TestComponentNameClass")]
    class TestComponentNameClass1
    {

    }

    [Component(Name = "TestComponentNameClass")]
    class TestComponentNameClass2
    {

    }
}

namespace TestSimple.Namespace
{
    [Component(Name = "TestClass1")]
    class TestClass1
    {
        [Inject]
        public TestClass2 ReferencedTestClass { set; get; }
    }

    [Component(Name = "TestClass2")]
    class TestClass2
    {

    }
}

namespace TestLifecycle.Namespace
{
    [Component(Name = "TestLifecycleClass")]
    class TestLifecycleClass
    {
        public bool OnStartInvoked
        {
            private set;
            get;
        }

        public bool OnStopInvoked
        {
            private set;
            get;
        }

        public TestLifecycleClass()
        {
            OnStartInvoked = false;
            OnStopInvoked = false;
        }

        [OnStart]
        void OnStart()
        {
            OnStartInvoked = true;
        }

        [OnStop]
        void OnStop()
        {
            OnStopInvoked = true;
        }
    }
}

namespace TestCyclic.Namespace
{
    [Component(Name = "TestCyclicClass1")]
    class TestCyclicClass1
    {
        [Inject]
        public TestCyclicClass2 ReferencedTestClass { set; get; }
    }

    [Component(Name = "TestCyclicClass2")]
    class TestCyclicClass2
    {
        [Inject]
        public TestCyclicClass1 ReferencedTestClass { set; get; }
    }
}

namespace TestInjectionScope.Namespace
{
    [Component(Name = "TestInjectionScopeClass1", Scope = ComponentScope.Context)]
    class TestInjectionScopeClass1
    {
        [Inject]
        public TestInjectionScopeClass3 ReferencedTestClass { set; get; }
    }

    [Component(Name = "TestInjectionScopeClass2", Scope = ComponentScope.Context)]
    class TestInjectionScopeClass2
    {
        [Inject]
        public TestInjectionScopeClass3 ReferencedTestClass { set; get; }
    }

    [Component(Name = "TestInjectionScopeClass3", Scope = ComponentScope.Injection)]
    class TestInjectionScopeClass3
    {
    }
}