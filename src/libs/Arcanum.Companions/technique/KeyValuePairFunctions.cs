// Copyright (c) Kornei Dontsov. All Rights Reserved. Licensed under the MIT. See LICENSE in the project root for license information.

namespace Arcanum.Companions {
	using System.Collections.Generic;

	static class KeyValuePairFunctions {
		public static void Deconstruct<TKey, TValue>
		(this KeyValuePair<TKey, TValue> keyValuePair,
		 out TKey key,
		 out TValue value) {
			key = keyValuePair.Key;
			value = keyValuePair.Value;
		}
	}
}
