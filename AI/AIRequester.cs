using System.Collections.Generic;

public interface AIRequester {

    void AICallback(ActionPath result, MoveSuggestor ms, int tested, int all);

    int GetPlayerID();

}