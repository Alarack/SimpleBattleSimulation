using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "NameDatabase", menuName = "Scriptable Objects/NameDatabase")]
public class NameDatabase : ScriptableObject
{

    public string[] firstNames;
    public string[] lastNames;
    public string[] connectors;



    public string GenerateName() {
        StringBuilder builder = new StringBuilder();

        builder.Append(firstNames[Random.Range(0, firstNames.Length)]);
        builder.Append(' ');
        builder.Append(lastNames[Random.Range(0, lastNames.Length)]);
   
        // 30% of names will have a connector and an extra set of names to make them sound cooler.
        float procRollForFancyName = Random.Range(0f, 1f);

        if(procRollForFancyName > 0.3f) {
            return builder.ToString();
        }

        builder.Append(' ');
        builder.Append(connectors[Random.Range(0, connectors.Length)]);
        builder.Append(' ');
        builder.Append(firstNames[Random.Range(0, firstNames.Length)]);
        builder.Append(' ');
        builder.Append(lastNames[Random.Range(0, lastNames.Length)]);

        return builder.ToString();


    }




}
