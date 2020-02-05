// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT. See LICENSE in the project root for license information.

namespace Arcanum.Companions.Tests {
	using FluentAssertions;
	using System;
	using Xunit;

	public class TestCompanionOrder {
		interface ILowestCompanion { }

		interface ILowCompanion { }

		interface IMiddleCompanion { }

		interface IHighCompanion { }

		interface IHighestCompanion { }

		class BaseTypeCompanionAttribute: Attribute,
			ILowestCompanion,
			ILowCompanion,
			IMiddleCompanion,
			IHighCompanion,
			IHighestCompanion { }

		[BaseTypeCompanionAttribute]
		class BaseType {
			[Inherited]
			public class Companion:
				ILowCompanion,
				IMiddleCompanion,
				IHighCompanion,
				IHighestCompanion { }
		}

		class DerivedTypeLowerCompanionAttribute: Attribute,
			IMiddleCompanion,
			IHighCompanion,
			IHighestCompanion { }

		class DerivedTypeHigherCompanionAttribute: Attribute,
			IHighCompanion,
			IHighestCompanion { }

		[DerivedTypeLowerCompanion]
		[DerivedTypeHigherCompanion]
		class TargetType: BaseType {
			public new class Companion: IHighestCompanion { }
		}

		static Type type { get; } = typeof(TargetType);

		[Fact]
		public void BaseTypeCompanionAttributeIsMatched () =>
			type.GetCompanion<ILowestCompanion>().Should().BeOfType<BaseTypeCompanionAttribute>();

		[Fact]
		public void BaseTypePrimaryCompanionIsMatched () =>
			type.GetCompanion<ILowCompanion>().Should().BeOfType<BaseType.Companion>();

		[Fact]
		public void LowerCompanionAttributeIsMatched () =>
			type.GetCompanion<IMiddleCompanion>().Should().BeOfType<DerivedTypeLowerCompanionAttribute>();

		[Fact]
		public void HigherCompanionAttributeIsMatched () =>
			type.GetCompanion<IHighCompanion>().Should().BeOfType<DerivedTypeHigherCompanionAttribute>();

		[Fact]
		public void PrimaryCompanionIsMatched () =>
			type.GetCompanion<IHighestCompanion>().Should().BeOfType<TargetType.Companion>();
	}
}
