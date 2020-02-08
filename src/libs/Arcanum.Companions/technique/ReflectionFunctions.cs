// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT. See LICENSE in the project root for license information.

namespace Arcanum.Companions {
	using System;
	using System.Reflection;

	static class ReflectionFunctions {
		public static ConstructorInfo GetDefaultCtor (this Type type) =>
			type.GetConstructor(Type.EmptyTypes);

		public static ConstructorInfo GetCtor (this Type type, params Type[] argTypes) =>
			type.GetConstructor(argTypes);

		public static Object Create (this ConstructorInfo ctorInfo) =>
			ctorInfo.Invoke(Array.Empty<Object>());

		public static Object Create (this ConstructorInfo ctorInfo, params Object[] args) =>
			ctorInfo.Invoke(args);
	}
}
