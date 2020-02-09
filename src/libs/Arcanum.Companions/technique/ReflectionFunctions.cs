// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT. See LICENSE in the project root for license information.

namespace Arcanum.Companions {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;
	using System.Reflection;

	static class ReflectionFunctions {
		public static Boolean HasSameGenSignatureAs (this Type type, Type other) {
			static IEnumerable<Type[]> SelectConstraints (Type type) {
				var t = ! type.IsGenericTypeDefinition ? type.GetGenericTypeDefinition() : type;
				return t.GetGenericArguments().Select(arg => arg.GetGenericParameterConstraints());
			}

			var firstConstraints = SelectConstraints(type);
			var secondConstraints = SelectConstraints(other);
			return firstConstraints.SequenceEqual(secondConstraints, TypeArrayEqualityComparer.shared);
		}

		public static Boolean IsClosedGenType (this Type type, [MaybeNullWhen(false)] out Type definition) {
			if (type.IsGenericType && ! type.IsGenericTypeDefinition) {
				definition = type.GetGenericTypeDefinition();
				return true;
			}
			else {
				definition = null!;
				return false;
			}
		}

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
