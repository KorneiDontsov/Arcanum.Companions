// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT. See LICENSE in the project root for license information.

namespace Arcanum.Companions.Tests {
	using FluentAssertions;
	using JetBrains.Annotations;
	using System;
	using System.Linq;
	using Xunit;

	public class TestCompanionEnumeration {
		interface IAnyCompanion { }

		class NotAnyCompanion: Attribute { }

		class AnyCompanion: Attribute, IAnyCompanion { }

		[AnyCompanion]
		[NotAnyCompanion]
		class BaseType {
			[Inherited]
			[UsedImplicitly]
			public class Companion: IAnyCompanion { }
		}

		class SomeCompanionAttribute: Attribute, IAnyCompanion { }

		[SomeCompanion]
		[NotAnyCompanion]
		class TargetType: BaseType {
			[UsedImplicitly]
			public new class Companion: IAnyCompanion { }
		}

		[Fact]
		public void IsWorked () {
			var companions = typeof(TargetType).EnumerateCompanions<IAnyCompanion>();
			var expectedCompanionTypes =
				new[] {
					typeof(TargetType.Companion),
					typeof(SomeCompanionAttribute),
					typeof(BaseType.Companion),
					typeof(AnyCompanion)
				};
			var actualCompanionTypes = companions.Select(c => c.GetType());
			actualCompanionTypes.Should().Equal(expectedCompanionTypes);
		}
	}
}
