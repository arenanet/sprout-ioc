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

namespace TestSimple.Namespace
{
    [Component(Name = "TestClass1")]
    public class TestClass1
    {
        [Inject]
        public TestClass2 ReferencedTestClass { set; get; }
    }

    [Component(Name = "TestClass2")]
    public class TestClass2
    {

    }
}

namespace TestLifecycle.Namespace
{
    [Component(Name = "TestLifecycleClass")]
    public class TestLifecycleClass
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
    public class TestCyclicClass1
    {
        [Inject]
        public TestCyclicClass2 ReferencedTestClass { set; get; }
    }

    [Component(Name = "TestCyclicClass2")]
    public class TestCyclicClass2
    {
        [Inject]
        public TestCyclicClass1 ReferencedTestClass { set; get; }
    }
}

namespace TestInjectionScope.Namespace
{
    [Component(Name = "TestInjectionScopeClass1", Scope = ComponentScope.Context)]
    public class TestInjectionScopeClass1
    {
        [Inject]
        public TestInjectionScopeClass3 ReferencedTestClass { set; get; }
    }

    [Component(Name = "TestInjectionScopeClass2", Scope = ComponentScope.Context)]
    public class TestInjectionScopeClass2
    {
        [Inject]
        public TestInjectionScopeClass3 ReferencedTestClass { set; get; }
    }

    [Component(Name = "TestInjectionScopeClass3", Scope = ComponentScope.Injection)]
    public class TestInjectionScopeClass3
    {
    }
}

namespace ArenaNet.Sprout.IoC
{
    /// <summary>
    /// Tests the Context.
    /// </summary>
    [TestClass]
    public class ContextTest
    {
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
        public void TestScanNamespaceUsingGenerics()
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
        public void TestScanNamespaceUsingType()
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
        public void TestScanNamespaceUsingNamespace()
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
