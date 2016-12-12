using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Printer {
	public static void SavePrintable(Printable p, string pathFilename) {
		string file = p.GetAsHtml();

		System.IO.File.WriteAllText(@pathFilename + p.GetFileName(), file);
	}
}