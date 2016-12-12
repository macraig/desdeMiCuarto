using System;

public class Prints {
	private Prints() { }

	public static string AddLine() {
		return "<p>&nbsp;</p>";
	}

	//TODO MARIA -> HTML INIT ?????.
	public static string InitFile() {
		return
			"<html xmlns:office=\"urn:schemas-microsoft-com:office:office\" xmlns:word=\"urn:schemas-microsoft-com:office:word\" xmlns=\"http://www.w3.org/TR/REC-html40\">\n" +
			"<head><meta charset='UTF-8'></head>\n" +
			"<body><div>" +
			"<p class='c0'><span style='overflow: hidden; display: inline-block; margin: 0.00px 0.00px; border: 0.00px solid #000000; transform: rotate(0.00rad) translateZ(0px); -webkit-transform: rotate(0.00rad) translateZ(0px); width:100%; height: auto;'><img alt='' src='data:image/png;base64," +
			//Convert.ToBase64String(Resources.Load<Sprite>("header-doc").texture.EncodeToPNG()) +
			"' style='width:100%; height: auto; margin-left: 0.00px; margin-top: 0.00px; transform: rotate(0.00rad) translateZ(0px); -webkit-transform: rotate(0.00rad) translateZ(0px);' title=''></span></p>" +
			"" +
			"";
	}

	public static string AddTitle(string title) {
		return "<h1 style=\"text-align: center;\">" + title + "</h1>\n";
	}

	public static string CloseFile() {
		return "</body></html>";
	}
}