using UnityEngine;
using System.Collections;

public interface URAction{

    void undo();
    void redo();

    void delete();
}
