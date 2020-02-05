// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT. See LICENSE in the project root for license information.

namespace Arcanum.Companions {
	using System;
	using System.Collections.Immutable;
	using System.Reflection;

	public static class Module {
		readonly struct CompanionInfo {
			public Object instance { get; }
			public Boolean inherited { get; }

			public CompanionInfo (Object instance, Boolean inherited) {
				this.instance = instance;
				this.inherited = inherited;
			}
		}

		static ImmutableArray<CompanionInfo> CreateCompanionInfos (Type type) {
			var result = ImmutableArray.CreateBuilder<CompanionInfo>();

			var attributes = type.GetCustomAttributes(inherit: false);
			foreach (var attribute in attributes) {
				var attributeUsage = attribute.GetType().GetCustomAttribute<AttributeUsageAttribute?>();
				var inherited = attributeUsage?.Inherited ?? true;
				result.Add(new CompanionInfo(attribute, inherited));
			}

			if (type.GetNestedType("Companion") is {IsClass: true} primCompType) {
				var primaryCompanion =
					primCompType.GetDefaultCtor()?.Create()
					?? primCompType.GetCtor(typeof(Type))?.Create(type);
				if (primaryCompanion is {}) {
					var inherited = Attribute.IsDefined(primCompType, typeof(InheritedAttribute));
					result.Add(new CompanionInfo(primaryCompanion, inherited));
				}
			}

			result.Reverse();
			return result.ToImmutable();
		}

		class CompanionResolver {
			ImmutableArray<CompanionInfo> companionInfos { get; }

			CompanionResolver? maybeParentCompanionResolver { get; }

			public CompanionResolver
			(ImmutableArray<CompanionInfo> companionInfos,
			 CompanionResolver? maybeParentCompanionResolver) {
				this.companionInfos = companionInfos;
				this.maybeParentCompanionResolver = maybeParentCompanionResolver;
			}

			T? MayGetInheritedCompanion<T> () where T: class {
				foreach (var companionInfo in companionInfos)
					if (companionInfo.inherited && companionInfo.instance is T matchedCompanion)
						return matchedCompanion;
				return maybeParentCompanionResolver?.MayGetInheritedCompanion<T>();
			}

			public T? MayGetCompanion<T> () where T: class {
				foreach (var companionInfo in companionInfos)
					if (companionInfo.instance is T matchedCompanion)
						return matchedCompanion;
				return maybeParentCompanionResolver?.MayGetInheritedCompanion<T>();
			}
		}

		static Func<Type, CompanionResolver> getCompanionResolver { get; } =
			MemoizeFunc.Create<Type, CompanionResolver>(
				(type, self) => {
					var companionInfos = CreateCompanionInfos(type);
					var maybeParentCompanionResolver = type.BaseType is {} baseType ? self(baseType) : null;
					return new CompanionResolver(companionInfos, maybeParentCompanionResolver);
				});

		public static T? MayGetCompanion<T> (this Type type) where T: class =>
			getCompanionResolver(type).MayGetCompanion<T>();

		public static T GetCompanion<T> (this Type type) where T: class =>
			MayGetCompanion<T>(type)
			?? throw new Exception($"Companion {typeof(T)} is not found in {type}.");

		public static Boolean HasCompanion<T> (this Type type) where T: class =>
			type.MayGetCompanion<T>() is {};
	}
}
