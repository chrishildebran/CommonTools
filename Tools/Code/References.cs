﻿namespace Sheasta.Core.Code;

using System.Reflection;

public static class References
{

	#region Methods (Non-Private)

	public static bool LoadTelerikReferences(Assembly assembly)
	{
		var assemblyLocation = assembly.Location;

		List<string> references = GetReferences(assembly, "Starts With", "Telerik");

		var referencesCount = references.Count;

		var loadedCount = 0;

		if(referencesCount <= 0)
		{
			return true;
		}

		foreach(var reference in references)
		{
			if(LoadReference(assemblyLocation, reference))
			{
				loadedCount++;
			}
		}

		return referencesCount == loadedCount;
	}

	#endregion

	#region Methods (Private)

	private static List<string> GetReferences(Assembly assembly, string lookPosition, string searchTerm)
	{
		IEnumerable<AssemblyName> referencedAssemblyNames = null;

		if(string.Equals(lookPosition, "Starts With", StringComparison.OrdinalIgnoreCase))
		{
			referencedAssemblyNames = assembly.GetReferencedAssemblies().Where(assemblyName => assemblyName.Name.StartsWith(searchTerm));
		}
		else if(string.Equals(lookPosition, "Contains", StringComparison.OrdinalIgnoreCase))
		{
			referencedAssemblyNames = assembly.GetReferencedAssemblies().Where(assemblyName => assemblyName.Name.Contains(searchTerm));
		}
		else if(string.Equals(lookPosition, "Ends With", StringComparison.OrdinalIgnoreCase))
		{
			referencedAssemblyNames = assembly.GetReferencedAssemblies().Where(assemblyName => assemblyName.Name.EndsWith(searchTerm));
		}
		else if(string.Equals(lookPosition, "Equals", StringComparison.OrdinalIgnoreCase))
		{
			referencedAssemblyNames = assembly.GetReferencedAssemblies().Where(assemblyName => assemblyName.Name.Equals(searchTerm));
		}

		List<string> references = new();

		if(referencedAssemblyNames == null)
		{
			return references;
		}

		references.AddRange(referencedAssemblyNames.Select(assemblyName => assemblyName.Name + ".dll"));

		return references;
	}


	private static bool LoadReference(string assemblyLocation, string reference)
	{
		var assemblyPath = Path.GetDirectoryName(assemblyLocation);

		if(assemblyPath == null)
		{
			return false;
		}

		var referencePath = Path.Combine(assemblyPath, reference);

		Assembly.LoadFrom(referencePath);

		return true;
	}

	#endregion

}