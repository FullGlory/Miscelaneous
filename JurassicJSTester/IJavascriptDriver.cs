// -----------------------------------------------------------------------
// <copyright file="IJavascriptDriver.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MarabouStork.Scripting.Javascript.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public interface IJavascriptSDriver
	{
		void LoadLibrary(string name);
		string ExecFunction(string name, params string[] parameters);
	}
}
