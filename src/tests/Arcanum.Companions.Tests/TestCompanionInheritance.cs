// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT. See LICENSE in the project root for license information.

namespace Arcanum.Companions.Tests {
	using FluentAssertions;
	using System;
	using Xunit;

	public class TestCompanionInheritance {
		[AttributeUsage(AttributeTargets.Class)]
		class InheritedCompanionAttribute: Attribute { }

		[AttributeUsage(AttributeTargets.Class, Inherited = false)]
		class NonInheritedCompanionAttribute: Attribute { }

		[InheritedCompanion]
		[NonInheritedCompanion]
		class TypeWithCompanionAttributes { }

		class TypeDerivedFromTypeWithCompanionAttributes: TypeWithCompanionAttributes { }

		[Fact]
		public void CompanionAttributeIsInherited () =>
			typeof(TypeDerivedFromTypeWithCompanionAttributes)
				.HasCompanion<InheritedCompanionAttribute>()
				.Should().BeTrue();

		[Fact]
		public void CompanionAttributeIsNotInherited () =>
			typeof(TypeDerivedFromTypeWithCompanionAttributes)
				.HasCompanion<NonInheritedCompanionAttribute>()
				.Should().BeFalse();

		class TypeWithNonInheritedPrimaryCompanion {
			public class Companion { }
		}

		class TypeDerivedFromTypeWithNonInheritedPrimaryCompanion: TypeWithNonInheritedPrimaryCompanion { }

		[Fact]
		public void PrimaryCompanionIsNotInherited () =>
			typeof(TypeDerivedFromTypeWithNonInheritedPrimaryCompanion)
				.HasCompanion<TypeWithNonInheritedPrimaryCompanion.Companion>()
				.Should().BeFalse();

		class TypeWithInheritedPrimaryCompanion {
			[Inherited]
			public class Companion { }
		}

		class TypeDerivedFromTypeWithInheritedPrimaryCompanion: TypeWithInheritedPrimaryCompanion { }

		[Fact]
		public void PrimaryCompanionIsInherited () =>
			typeof(TypeDerivedFromTypeWithInheritedPrimaryCompanion)
				.HasCompanion<TypeWithInheritedPrimaryCompanion.Companion>()
				.Should().BeTrue();
	}
}
