// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT. See LICENSE in the project root for license information.

namespace Arcanum.Companions.Tests {
	using FluentAssertions;
	using JetBrains.Annotations;
	using System;
	using Xunit;

	public class TestCompanion {
		interface ICompanion { }

		class CompanionAttribute: Attribute, ICompanion { }

		[CompanionAttribute]
		class TypeWithCompanionAttribute { }

		class TypeWithCompanionWithDefaultCtor {
			[UsedImplicitly]
			public class Companion: ICompanion { }
		}

		class TypeWithCompanionWithParamCtor {
			[UsedImplicitly]
			public class Companion: ICompanion {
				public Companion (Type type) =>
					type.Should().Be(typeof(TypeWithCompanionWithParamCtor));
			}
		}

		[Theory]
		[InlineData(typeof(TypeWithCompanionAttribute))]
		[InlineData(typeof(TypeWithCompanionWithDefaultCtor))]
		[InlineData(typeof(TypeWithCompanionWithParamCtor))]
		public void Exists (Type type) =>
			type.HasCompanion<ICompanion>().Should().BeTrue();

		[Theory]
		[InlineData(typeof(TypeWithCompanionAttribute))]
		[InlineData(typeof(TypeWithCompanionWithDefaultCtor))]
		[InlineData(typeof(TypeWithCompanionWithParamCtor))]
		public void IsSingleton (Type type) {
			var companion1 = type.GetCompanion<ICompanion>();
			var companion2 = type.GetCompanion<ICompanion>();
			companion1.Should().BeSameAs(companion2);
		}
	}
}
