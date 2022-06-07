using UnityEngine;
using UnityEngine.Events;

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;



public interface IJsonParser {
	void Parse(IDictionary<string, object> data);
}

public static class JsonDictionaryExtention {
	static public bool ObjectToValue(object o, out byte dest) {
		if (o is ulong) { dest = (byte)(ulong)o; }
		else if (o is long) { dest = (byte)(long)o; }
		else if (o is double) { dest = (byte)(double)o; }
		else { dest = 0; return false; }
		return true;
	}
	static public bool ObjectToValue(object o, out sbyte dest) {
		if (o is ulong) { dest = (sbyte)(ulong)o; }
		else if (o is long) { dest = (sbyte)(long)o; }
		else if (o is double) { dest = (sbyte)(double)o; }
		else { dest = 0; return false; }
		return true;
	}
	static public bool ObjectToValue(object o, out short dest) {
		if (o is ulong) { dest = (short)(ulong)o; }
		else if (o is long) { dest = (short)(long)o; }
		else if (o is double) { dest = (short)(double)o; }
		else { dest = 0; return false; }
		return true;
	}
	static public bool ObjectToValue(object o, out ushort dest) {
		if (o is ulong) { dest = (ushort)(ulong)o; }
		else if (o is long) { dest = (ushort)(long)o; }
		else if (o is double) { dest = (ushort)(double)o; }
		else { dest = 0; return false; }
		return true;
	}
	static public bool ObjectToValue(object o, out int dest) {
		if (o is ulong) { dest = (int)(ulong)o; }
		else if (o is long) { dest = (int)(long)o; }
		else if (o is double) { dest = (int)(double)o; }
		else { dest = 0; return false; }
		return true;
	}
	static public bool ObjectToValue(object o, out uint dest) {
		if (o is ulong) { dest = (uint)(ulong)o; }
		else if (o is long) { dest = (uint)(long)o; }
		else if (o is double) { dest = (uint)(double)o; }
		else { dest = 0; return false; }
		return true;
	}
	static public bool ObjectToValue(object o, out long dest) {
		if (o is ulong) { dest = (long)(ulong)o; }
		else if (o is long) { dest = (long)o; }
		else if (o is double) { dest = (long)(double)o; }
		else { dest = 0; return false; }
		return true;
	}
	static public bool ObjectToValue(object o, out ulong dest) {
		if (o is ulong) { dest = (ulong)o; }
		else if (o is long) { dest = (ulong)(long)o; }
		else if (o is double) { dest = (ulong)(double)o; }
		else { dest = 0; return false; }
		return true;
	}
	static public bool ObjectToValue(object o, out float dest) {
		if (o is ulong) { dest = (ulong)o; }
		else if (o is long) { dest = (long)o; }
		else if (o is double) { dest = (float)(double)o; }
		else { dest = 0; return false; }
		return true;
	}
	static public bool ObjectToValue(object o, out double dest) {
		if (o is ulong) { dest = (ulong)o; }
		else if (o is long) { dest = (long)o; }
		else if (o is double) { dest = (double)o; }
		else { dest = 0; return false; }
		return true;
	}
	static public bool ObjectToValue(object o, out bool dest) {
		if (o is bool) { dest = (bool)o; }
		else if (o is ulong) { dest = (ulong)o != 0; }
		else if (o is long) { dest = (long)o != 0; }
		else if (o is double) { dest = (double)o != 0; }
		else { dest = false; return false; }
		return true;
	}
	static public bool ObjectToValue(object o, out string dest) {
		if (o is string) { dest = (string)o; }
		else { dest = null; return false; }
		return true;
	}

	static public bool Get(this IDictionary<string, object> data, string key, out byte dest) {
		object o; dest = 0;
		return data.TryGetValue(key, out o) ? ObjectToValue(o, out dest) : false;
	}
	static public bool Get(this IDictionary<string, object> data, string key, out sbyte dest) {
		object o; dest = 0;
		return data.TryGetValue(key, out o) ? ObjectToValue(o, out dest) : false;
	}
	static public bool Get(this IDictionary<string, object> data, string key, out short dest) {
		object o; dest = 0;
		return data.TryGetValue(key, out o) ? ObjectToValue(o, out dest) : false;
	}
	static public bool Get(this IDictionary<string, object> data, string key, out ushort dest) {
		object o; dest = 0;
		return data.TryGetValue(key, out o) ? ObjectToValue(o, out dest) : false;
	}
	static public bool Get(this IDictionary<string, object> data, string key, out int dest) {
		object o; dest = 0;
		return data.TryGetValue(key, out o) ? ObjectToValue(o, out dest) : false;
	}
	static public bool Get(this IDictionary<string, object> data, string key, out uint dest) {
		object o; dest = 0;
		return data.TryGetValue(key, out o) ? ObjectToValue(o, out dest) : false;
	}
	static public bool Get(this IDictionary<string, object> data, string key, out long dest) {
		object o; dest = 0;
		return data.TryGetValue(key, out o) ? ObjectToValue(o, out dest) : false;
	}
	static public bool Get(this IDictionary<string, object> data, string key, out ulong dest) {
		object o; dest = 0;
		return data.TryGetValue(key, out o) ? ObjectToValue(o, out dest) : false;
	}
	static public bool Get(this IDictionary<string, object> data, string key, out float dest) {
		object o; dest = 0;
		return data.TryGetValue(key, out o) ? ObjectToValue(o, out dest) : false;
	}
	static public bool Get(this IDictionary<string, object> data, string key, out double dest) {
		object o; dest = 0;
		return data.TryGetValue(key, out o) ? ObjectToValue(o, out dest) : false;
	}
	static public bool Get(this IDictionary<string, object> data, string key, out bool dest) {
		object o; dest = false;
		return data.TryGetValue(key, out o) ? ObjectToValue(o, out dest) : false;
	}
	static public bool Get(this IDictionary<string, object> data, string key, out string dest) {
		object o; dest = null;
		return data.TryGetValue(key, out o) ? ObjectToValue(o, out dest) : false;
	}
	static public bool Get<T>(this IDictionary<string, object> data, string key, out T dest) where T : class, IJsonParser, new() {
		object o; dest = null;
		if (!data.TryGetValue(key, out o)) { return false; }
		if (o == null) { return true; }
		IDictionary<string, object> obj = null;
		if ((obj = o as IDictionary<string, object>) != null) {
			dest = new T();
			dest.Parse(obj);
			return true;
		}
		return false;
	}

	static public bool GetArray(this IDictionary<string, object> data, string key, out IList<byte> dest) {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<byte>(array.Count);
		for (int i = 0; i < array.Count; ++i) { byte tmp; ObjectToValue(array[i], out tmp); d.Add(tmp); }
		dest = d;
		return true;
	}
	static public bool GetArray(this IDictionary<string, object> data, string key, out IList<sbyte> dest) {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<sbyte>(array.Count);
		for (int i = 0; i < array.Count; ++i) { sbyte tmp; ObjectToValue(array[i], out tmp); d.Add(tmp); }
		dest = d;
		return true;
	}
	static public bool GetArray(this IDictionary<string, object> data, string key, out IList<short> dest) {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<short>(array.Count);
		for (int i = 0; i < array.Count; ++i) { short tmp; ObjectToValue(array[i], out tmp); d.Add(tmp); }
		dest = d;
		return true;
	}
	static public bool GetArray(this IDictionary<string, object> data, string key, out IList<ushort> dest) {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<ushort>(array.Count);
		for (int i = 0; i < array.Count; ++i) { ushort tmp; ObjectToValue(array[i], out tmp); d.Add(tmp); }
		dest = d;
		return true;
	}
	static public bool GetArray(this IDictionary<string, object> data, string key, out IList<int> dest) {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<int>(array.Count);
		for (int i = 0; i < array.Count; ++i) { int tmp; ObjectToValue(array[i], out tmp); d.Add(tmp); }
		dest = d;
		return true;
	}
	static public bool GetArray(this IDictionary<string, object> data, string key, out IList<uint> dest) {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<uint>(array.Count);
		for (int i = 0; i < array.Count; ++i) { uint tmp; ObjectToValue(array[i], out tmp); d.Add(tmp); }
		dest = d;
		return true;
	}
	static public bool GetArray(this IDictionary<string, object> data, string key, out IList<long> dest) {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<long>(array.Count);
		for (int i = 0; i < array.Count; ++i) { long tmp; ObjectToValue(array[i], out tmp); d.Add(tmp); }
		dest = d;
		return true;
	}
	static public bool GetArray(this IDictionary<string, object> data, string key, out IList<ulong> dest) {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<ulong>(array.Count);
		for (int i = 0; i < array.Count; ++i) { ulong tmp; ObjectToValue(array[i], out tmp); d.Add(tmp); }
		dest = d;
		return true;
	}
	static public bool GetArray(this IDictionary<string, object> data, string key, out IList<float> dest) {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<float>(array.Count);
		for (int i = 0; i < array.Count; ++i) { float tmp; ObjectToValue(array[i], out tmp); d.Add(tmp); }
		dest = d;
		return true;
	}
	static public bool GetArray(this IDictionary<string, object> data, string key, out IList<double> dest) {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<double>(array.Count);
		for (int i = 0; i < array.Count; ++i) { double tmp; ObjectToValue(array[i], out tmp); d.Add(tmp); }
		dest = d;
		return true;
	}
	static public bool GetArray(this IDictionary<string, object> data, string key, out IList<bool> dest) {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<bool>(array.Count);
		for (int i = 0; i < array.Count; ++i) { bool tmp; ObjectToValue(array[i], out tmp); d.Add(tmp); }
		dest = d;
		return true;
	}
	static public bool GetArray(this IDictionary<string, object> data, string key, out IList<string> dest) {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<string>(array.Count);
		for (int i = 0; i < array.Count; ++i) { string tmp; ObjectToValue(array[i], out tmp); d.Add(tmp); }
		dest = d;
		return true;
	}
	static public bool GetArray<T>(this IDictionary<string, object> data, string key, out IList<T> dest) where T : class, IJsonParser, new() {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new List<T>(array.Count);
		for (int i = 0; i < array.Count; ++i) {
			if (array[i] != null) {
				IDictionary<string, object> obj = null;
				if ((obj = array[i] as IDictionary<string, object>) == null) { dest = null; return false; }
				var v = new T();
				v.Parse(obj);
				d.Add(v);
			}
		}
		dest = d;
		return true;
	}
	static public bool GetDictionaryFromArray<T, U, V>(
		this IDictionary<string, object> data,
		string key,
		Action<T, IDictionary<U, V>> addCallback,
		out IDictionary<U, V> dest)
		where T : class, IJsonParser, new() {
		object o;
		IList<object> array;
		if (!data.TryGetValue(key, out o) || (array = o as IList<object>) == null) { dest = null; return false; }
		var d = new Dictionary<U, V>(array.Count);
		for (int i = 0; i < array.Count; ++i) {
			if (array[i] != null) {
				IDictionary<string, object> obj = null;
				if ((obj = array[i] as IDictionary<string, object>) == null) { dest = null; return false; }
				var v = new T();
				v.Parse(obj);
				addCallback(v, d);
			}
		}
		dest = d;
		return true;
	}

    static public bool GetEnum<EnumT>(this IDictionary<string, object> data, string key, out EnumT dest) {
        int res = 0;
        data.Get(key, out res);
        dest = (EnumT)(object)res;
        return true;
    }

}
