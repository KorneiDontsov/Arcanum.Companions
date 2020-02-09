// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT. See LICENSE in the project root for license information.

namespace Arcanum.Companions.Tests {
	using FluentAssertions;
	using JetBrains.Annotations;
	using System;
	using Xunit;

	public class TestGenericTypeCompanion {
		public class GenericTypeWithCompanion<[UsedImplicitly] T> {
			public class Companion { }
		}

		[Fact]
		public void Exists () =>
			typeof(GenericTypeWithCompanion<Object>)
				.HasCompanion<GenericTypeWithCompanion<Object>.Companion>()
				.Should().BeTrue();
	}
}
