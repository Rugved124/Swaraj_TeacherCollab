using System.Xml;
using System.Xml.Serialization;

/// <summary>
/// Type that holds data that are serialized and stored in PlayerPrefs
/// </summary>
[XmlRoot("SavedData")]
public class SvgSerializableData
{
	[XmlElement("Scene")]
	public string currentSceneName;
	[XmlElement("Checkpt")]
	public int currCheckpointID;

	//Constructor
	public SvgSerializableData()
	{
		currentSceneName = "level 1";
		currCheckpointID = 0;
	}
}
