using System.Collections;
using System.Windows.Forms;

public class DialogManager {

    private static OpenFileDialog openDialog;
    private static SaveFileDialog saveFileDialog;

    public static string OpenFileDialog()
    {
        openDialog = new OpenFileDialog();
        openDialog.InitialDirectory = UnityEngine.Application.dataPath;
        openDialog.Filter = "CivilWar Maps(*.cwmap)|*.CWMAP;|Alle Dateien (*.*)|*.*";
        openDialog.Title = "Karte zum öffnen auswählen";
        openDialog.Multiselect = false;
        string result = "";

        if(openDialog.ShowDialog() == DialogResult.OK) {
            result = openDialog.FileName;
        }
        if(result.Equals(string.Empty)) {
            result = null;
        }

        openDialog = null;
        return result;
    }

    public static string SaveFileDialog()
    {
        saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "CivilWar Maps(*.cwmap)|*.CWMAP;|Alle Dateien (*.*)|*.*";
        saveFileDialog.Title = "Karte speichern unter...";
        saveFileDialog.AddExtension = true;
        saveFileDialog.DefaultExt = Constants.FILE_EXTENSION;


        string result = null;
        if(saveFileDialog.ShowDialog() == DialogResult.OK) {
            result = saveFileDialog.FileName;
        }
        if (result.Equals(string.Empty)) {
            result = null;
        }

        saveFileDialog = null;
        return result;
    }
}

