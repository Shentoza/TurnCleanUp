using UnityEngine;
using System.Collections;

public interface URAction{

    void Undo();
    void Redo();
    void Delete();
}
