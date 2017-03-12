//// ------------------------------------------
//// ReflectionUtil.cs, Laska.D3
//// 
//// Created by Pedro Sequeira, 2015/04/01
//// 
//// pedro.sequeira@gaips.inesc-id.pt
//// ------------------------------------------

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//namespace Agnes.Agnes.D3.Util
//{
//	public static class ReflectionUtil
//	{
//		private const BindingFlags FULL_BINDING_FLAGS =
//			BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

//		private static readonly Dictionary<KeyValuePair<Type, Type>, IEnumerable<KeyValuePair<Attribute, PropertyInfo>>>
//			AttributeCache =
//				new Dictionary<KeyValuePair<Type, Type>, IEnumerable<KeyValuePair<Attribute, PropertyInfo>>>();

//		private static readonly Dictionary<KeyValuePair<Type, Type>, KeyValuePair<Attribute, MethodInfo>>
//			MethodCache = new Dictionary<KeyValuePair<Type, Type>, KeyValuePair<Attribute, MethodInfo>>();

//		private static readonly Dictionary<Type, IEnumerable<PropertyInfo>> PropertyCache =
//			new Dictionary<Type, IEnumerable<PropertyInfo>>();

//		public static IEnumerable<PropertyInfo> GetProperties(Type targetType)
//		{
//			if (targetType == null) return null;

//			//verifies cache
//			if (PropertyCache.ContainsKey(targetType)) return PropertyCache[targetType];

//			//gets properties and adds to cache
//			var list = targetType.GetProperties(FULL_BINDING_FLAGS).ToList();
//			PropertyCache[targetType] = list;
//			return list;
//		}

//		private static readonly Dictionary<Type, IEnumerable<FieldInfo>> FieldCache =
//			new Dictionary<Type, IEnumerable<FieldInfo>>();

//		public static IEnumerable<FieldInfo> GetFields(Type targetType)
//		{
//			if (targetType == null) return null;

//			//verifies cache
//			if (FieldCache.ContainsKey(targetType)) return FieldCache[targetType];

//			//gets fields hierarchically and adds to cache
//			var list = new List<FieldInfo>();
//			var type = targetType;
//			while (type != null)
//			{
//				list.AddRange(type.GetFields(FULL_BINDING_FLAGS));
//				type = type.BaseType;
//			}

//			FieldCache[targetType] = list;
//			return list;
//		}

//		public static IEnumerable<KeyValuePair<Attribute, PropertyInfo>> GetProperties<TAttribute>(
//			Object target, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance,
//			bool searchAncestors = true) where TAttribute : Attribute
//		{
//			return target == null ? null : GetProperties<TAttribute>(target.GetType(), bindingFlags, searchAncestors);
//		}

//		public static IEnumerable<KeyValuePair<Attribute, PropertyInfo>> GetProperties<TAttribute>(
//			Type targetType, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance,
//			bool searchAncestors = true) where TAttribute : Attribute
//		{
//			if (targetType == null) return null;

//			//verifies cache, send required attribute-property right away
//			var key = new KeyValuePair<Type, Type>(typeof(TAttribute), targetType);
//			if (AttributeCache.ContainsKey(key))
//				return AttributeCache[key];

//			//retrieve such list using reflection
//			var list = new List<KeyValuePair<Attribute, PropertyInfo>>();
//			foreach (var property in targetType.GetProperties(bindingFlags))
//			{
//				if ((property == null) || !property.CanRead || !property.CanWrite)
//					continue;

//				var setMethod = property.GetSetMethod();
//				if (setMethod == null || setMethod.IsStatic)
//					continue;

//				var attribute = Attribute.GetCustomAttribute(property, typeof(TAttribute), searchAncestors);
//				if (attribute != null)
//					list.Add(new KeyValuePair<Attribute, PropertyInfo>(attribute, property));
//			}

//			//store list in cache
//			AttributeCache[key] = list;
//			return list;
//		}

//		public static KeyValuePair<Attribute, MethodInfo> GetMethod<TAttribute>(
//			object target, Func<MethodInfo, bool> checkMethodSignature = null,
//			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance, bool searchAncestors = true)
//			where TAttribute : Attribute
//		{
//			if (target == null) return new KeyValuePair<Attribute, MethodInfo>(null, null);

//			//verifies cache, send required attribute-method right away
//			var targetType = target.GetType();B
//			var key = new KeyValuePair<Type, Type>(typeof(TAttribute), targetType);
//			if (MethodCache.ContainsKey(key))
//				return MethodCache[key];

//			foreach (var method in target.GetType().GetMethods(bindingFlags))
//			{
//				if (method.IsStatic)
//					continue;

//				var attribute = Attribute.GetCustomAttribute(
//					method, typeof(TAttribute), searchAncestors);
//				if ((attribute == null) || ((checkMethodSignature != null) && !checkMethodSignature(method)))
//					continue;

//				//return first method found, ignore others..
//				return MethodCache[key] = new KeyValuePair<Attribute, MethodInfo>(attribute, method);
//			}

//			return MethodCache[key];
//		}
//	}
//}