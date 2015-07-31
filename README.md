<img src="http://cdn.flaticon.com/png/256/66630.png" alt="Icon" width="30" height="30"/> Sprout IoC
=====
[![Build status](https://ci.appveyor.com/api/projects/status/txmmhy4jjvjlubut?svg=true)](https://ci.appveyor.com/project/elvirb/sprout-ioc)
[![License](https://img.shields.io/github/license/arenanet/sprout-ioc.svg)](http://www.apache.org/licenses/LICENSE-2.0)
[![Stories in Ready](https://img.shields.io/github/issues/arenanet/sprout-ioc.svg?label=tasks)](http://waffle.io/arenanet/sprout-ioc)

A down-to-earth IoC container implementation written in C#.

Usage
==========
```csharp
namespace MyNamespace
{
	[Component]
	public class MyComponent
	{
		[Inject]
		MyOtherComponent OtherComponent
		{
			set;
			get;
		}

		public void DoStuff()
		{
			...
		}

		[OnStart]
		void OnStart()
		{
			...
		}

		[OnEnd]
		void OnEnd()
		{
			...
		}
	}

	[Component]
	public class MyOtherComponent
	{
		...
	}
}

...

class MyMain
{
	static void Main(string[] args)
	{
		// Create context instance
		Context context = new Context();

		// Register application namespace and start
		context.Scan("MyNamespace").Start();

		// Register shutdown hook
		AppDomain.CurrentDomain.ProcessExit += (s, e) => context.Stop();

		// Get Context scoped components and interact with them
		MyComponent component = context.GetComponent<MyComponent>();
		component.DoStuff();

		// Wait until Context stops
		while (context.State != ContextState.Stopped)
		{
			Thread.Sleep(1000);
		}
	}
}
```

## Bugs and Feedback

For bugs, questions and discussions please use the [GitHub Issues](https://github.com/ArenaNet/sprout-ioc/issues).

## LICENSE

Copyright 2015 ArenaNet, LLC.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

<http://www.apache.org/licenses/LICENSE-2.0>

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
