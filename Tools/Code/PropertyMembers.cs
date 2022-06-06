namespace Sheasta.Core.Code;

using System.Reflection;

public static class PropertyMembers
{

	#region Methods (Non-Private)

	public static List<Tuple<string, object>> GetAll(object o)
	{
		if(o == null)
		{
			return new List<Tuple<string, object>>();
		}

		var t = o.GetType();

		PropertyInfo [ ] properties = t.GetProperties();

		List<Tuple<string, object>> tuples = new();

		foreach(var property in properties)
		{
			var value = property.GetValue(o, new object [ ]
			                                 {});

			tuples.Add(new Tuple<string, object>(property.Name, value));
		}

		return tuples;
	}


	public static List<Tuple<string, object>> GetAllByName(object o, string nameContains)
	{
		if(o == null)
		{
			return new List<Tuple<string, object>>();
		}

		var t = o.GetType();

		PropertyInfo [ ] properties = t.GetProperties();

		List<Tuple<string, object>> tuples = new();

		foreach(var property in properties)
		{
			if(!property.Name.Contains(nameContains))
			{
				continue;
			}

			var value = property.GetValue(o, new object [ ]
			                                 {});

			tuples.Add(new Tuple<string, object>(property.Name, value));
		}

		return tuples;
	}


	public static string GetNameByValue(object passedObject, string propValueIncoming)
	{
		var propertyNameNotFound = "Property Name Not Found";

		if(passedObject == null)
		{
			return propertyNameNotFound;
		}

		var type = passedObject.GetType();

		PropertyInfo [ ] properties = type.GetProperties();

		foreach(var property in properties)
		{
			var propValueCurrent = property.GetValue(passedObject, new object [ ]
			                                                       {});

			if(propValueCurrent.ToString() == propValueIncoming)
			{
				return property.Name;
			}
		}

		return propertyNameNotFound;
	}

	#endregion

}